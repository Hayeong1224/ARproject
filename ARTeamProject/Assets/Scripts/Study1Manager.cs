using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

[RequireComponent(typeof(ARRaycastManager))]
public class Study1Manager : MonoBehaviour
{
    private ARRaycastManager raycastManager;

    [SerializeField]
    private GameObject objectToInstantiate;

    //[SerializeField]
    //private GameObject blockManager;

    private ARPlaneManager mARPlaneManager;

    // raycast hits
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private Vector2 touchPosition;

    private GameObject spawnedObject;

    private bool tap = false;

    public GameObject[] questions;

    public Text checkText;

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        //blockManager.SetActive(false);
        mARPlaneManager = GetComponent<ARPlaneManager>();
        questions = GameObject.FindGameObjectsWithTag("Question");
        if (questions != null)
        {
            foreach (GameObject question in questions)
                question.SetActive(false);
        }
    }

    // get input in this method
    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        // touch and drag
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;

    }

    // Update is called once per frame
    void Update()
    {
        // check input
        if (TryGetTouchPosition(out touchPosition))
        {
            raycastAndCreateAndUpdate();
        }
    }

    // instantiate new gameobject based on raycast
    private void raycastAndCreateAndUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);

        // do raycast
        if (raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
        {
            // get position and rotation of the first plane hit and instantiate a gameobject
            Pose hitPose = hits[0].pose;

            // instantiate only one time
            if (!tap)
            {
                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(objectToInstantiate, hitPose.position, hitPose.rotation);
                    spawnedObject.transform.Rotate(Vector3.up * Time.deltaTime);
                    QuestionAppear();
                    // we can build the blocks
                    //blockManager.SetActive(true);
                    //blockManager.SendMessage("GetRotation", spawnedObject.transform.rotation);
                    DisabledPlaneDetection();
                }
                else
                {
                    // update position
                    spawnedObject.transform.position = hitPose.position + Vector3.up * (spawnedObject.transform.localScale.y / 2);
                    spawnedObject.transform.Rotate(Vector3.up * Time.deltaTime);
                }
                tap = true;
            }
        }
    }

    //평면 인식 더 이상 하기 않게 설정
    private void DisabledPlaneDetection()
    {
        mARPlaneManager.enabled = false;
        foreach (ARPlane plane in mARPlaneManager.trackables)
            plane.gameObject.SetActive(false);
    }

    public void QuestionAppear()
    {
        if (questions != null)
        {
            foreach (GameObject question in questions)
                question.SetActive(true);
        }

    }

    public void Correct()
    {
        if (checkText != null)
            checkText.text = "T R U E";
    }
}
