using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class Question : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    public LayerMask cubelayer;
    public List<GameObject> ACube = new List<GameObject>();
    Canvas canvas;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool tap = false;
    private GameObject spawnedObject;
    private Vector2 touchPosition;
    [SerializeField]
    private GameObject objectToInstantiate;
    private ARPlaneManager mARPlaneManager;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = gameObject.GetComponentInChildren<Canvas>();
        canvas.enabled = false;
    }
    void Update()
    {

        // check input
        if (TryGetTouchPosition(out touchPosition))
        {
            Debug.Log("do touch");
            raycastAndCreateAndUpdate();
        }
    }
    // Update is called once per frame
    private void raycastAndCreateAndUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        Debug.Log("do raycast");
        // do raycast
        if (raycastManager.Raycast(ray, hits))
        {
            Debug.Log("do rr");
            // get position and rotation of the first plane hit and instantiate a gameobject
            Pose hitPose = hits[0].pose;
            spawnedObject = Instantiate(objectToInstantiate, hitPose.position, hitPose.rotation);
            // instantiate only one time
            if (!tap)
            {
                if (spawnedObject == null)
                {
                    Debug.Log("touch");
                    spawnedObject = Instantiate(objectToInstantiate, hitPose.position, hitPose.rotation);
                    DisabledPlaneDetection();
                    QuestionCube();
                }
                else
                {
                    // update position
                    spawnedObject.transform.position = hitPose.position + Vector3.up * (spawnedObject.transform.localScale.y / 2);
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
    private void QuestionCube()
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, cubelayer))
        {
            if (Input.touchCount > 0)
            {
                if (hit.collider.gameObject.name == ACube[0].name)
                {
                    Debug.Log("True " + hit.collider.gameObject);
                    canvas.enabled = true;


                }
                else
                {
                    Debug.Log("False " + hit.collider.gameObject);
                }
            }
        }
    }
}





