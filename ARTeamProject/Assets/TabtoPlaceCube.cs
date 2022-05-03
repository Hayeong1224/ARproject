using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TabtoPlaceCube : MonoBehaviour
{
    GameObject TrueCanvas, NextCanvas;

    private ARRaycastManager raycastManager;

    public GameObject AnswerObject;

    [SerializeField]
    private GameObject objectToInstantiate;


    private ARPlaneManager mARPlaneManager;

    // raycast hits
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private Vector2 touchPosition;

    private GameObject spawnedObject;

    private bool tap = false;

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        mARPlaneManager = GetComponent<ARPlaneManager>();
        TrueCanvas = GameObject.FindGameObjectWithTag("TrueText");
        NextCanvas = GameObject.FindGameObjectWithTag("Next");
        TrueCanvas.SetActive(false);
        NextCanvas.SetActive(false);
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
        if (raycastManager.Raycast(ray, hits))
        {
            // get position and rotation of the first plane hit and instantiate a gameobject
            Pose hitPose = hits[0].pose;
            Debug.Log("1");
            // instantiate only one time
            if (!tap)
            {
                Debug.Log("2");
                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(objectToInstantiate, hitPose.position, hitPose.rotation);
                    // we can build the blocks
                    
                    DisabledPlaneDetection();
                }
                else
                {
                    // update position
                    spawnedObject.transform.position = hitPose.position + Vector3.up * (spawnedObject.transform.localScale.y / 2);
                }
                tap = true;
            }
            QuestionCube();
        }
    }

    //평면 인식 더 이상 하기 않게 설정
    private void DisabledPlaneDetection()
    {
        mARPlaneManager.enabled = false;
        foreach (ARPlane plane in mARPlaneManager.trackables)
            plane.gameObject.SetActive(false);
    }
    private void QuestionCube()
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.touchCount > 0)
            {
                if (hit.collider.gameObject.name == AnswerObject.name)
                {
                    Debug.Log("True " + hit.collider.gameObject);


                    Destroy(objectToInstantiate);

                }
                else
                {
                    Debug.Log("False " + hit.collider.gameObject);
                    //사운드 넣기 - 틀림
                }
            }
        }
    }
}
