using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ARRaycastManager))]
public class Study1Manager : MonoBehaviour
{
    private ARRaycastManager raycastManager;

    [SerializeField]
    private GameObject groundToInstantiate;

    [SerializeField]
    private GameObject blockManager;

    private ARPlaneManager mARPlaneManager;

    // raycast hits
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private Vector2 touchPosition;

    private GameObject spawnedGround;

    private bool tap = false;

    private GameObject[] questions;
    private GameObject[] buttons;

    public GameObject nextPanel;
    public GameObject eduPointTxt;

    private int tryCount = 1;
    static public int eduPoint = 0; 

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        blockManager.SetActive(false);
        mARPlaneManager = GetComponent<ARPlaneManager>();
        questions = GameObject.FindGameObjectsWithTag("Question");
        if (questions != null)
        {
            foreach (GameObject question in questions)
                question.SetActive(false);
        }
        nextPanel.SetActive(false);
        buttons = GameObject.FindGameObjectsWithTag("Button");
        if (buttons != null)
        {
            foreach (GameObject button in buttons)
                button.SetActive(false);
        }

    }

    // get input in this method
    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        // touch and drag
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
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
                if (spawnedGround == null)
                {
                    //땅 생성
                    spawnedGround = Instantiate(groundToInstantiate, hitPose.position, hitPose.rotation);

                    //spawnedObject.transform.SetParent(parentCube);
                    Debug.Log("AR Plane is built on " + hitPose.position.x + " " + hitPose.position.y + " " + hitPose.position.z);
                    DisabledPlaneDetection();
                    ButtonAppear();

                    // we can build the blocks
                    blockManager.SetActive(true);

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

    public void ButtonAppear()
    {
        if (buttons != null)
        {
            foreach (GameObject button in buttons)
                button.SetActive(true);
        }
    }

    //choosing correct answer
    public void Correct()
    {
        if (nextPanel != null && eduPointTxt != null)
        {
            nextPanel.SetActive(true);
            string point = eduPoint.ToString();
            //eduPointTxt.GetComponent<Text>().text = "Educational Point: ";
            Debug.Log(point);
        }
    }

    //choosing wrong answer
    public void Wrong()
    {
        tryCount++;
    }

    private void Point()
    {
        switch(tryCount)
        {
            case 1:
                eduPoint = 100;
                break;
            case 2:
                eduPoint = 70;
                break;
            case 3:
                eduPoint = 50;
                break;
            default:
                eduPoint = 30;
                break;
        }  
    }

}
