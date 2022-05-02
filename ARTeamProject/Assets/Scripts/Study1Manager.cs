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

    /*
    [SerializeField]
    private GameObject objectToInstantiate;*/

    [SerializeField]
    private GameObject groundToInstantiate;

    [SerializeField]
    private GameObject blockManager;

    private ARPlaneManager mARPlaneManager;

    // raycast hits
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private Vector2 touchPosition;

    private GameObject spawnedGround;
    //private GameObject spawnedObject;

    private bool tap = false;

    private GameObject[] questions;
    private GameObject[] buttons;

    public GameObject checkText;

    public Transform parentCube;

    static public List<Vector3> answerPos = new List<Vector3>();

    //static public List<Vector3> questionPos = new List<Vector3>();

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
        checkText.SetActive(false);
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
                    //blockManager.SendMessage("GetRotation", spawnedObject.transform.rotation);

                    /*
                    for (int i = 0; i < spawnedObject.transform.childCount; i++)
                    {
                        Vector3 pos = spawnedObject.transform.GetChild(i).gameObject.transform.position;
                        Debug.Log("block of QuestionCube is built on " + pos.x + " " + pos.y + " " + pos.z);


                        Vector3 gridPos = new Vector3(Mathf.Round(pos.x / gridSize) * gridSize, Mathf.Round(pos.y / gridSize) * gridSize, Mathf.Round(pos.z / gridSize) * gridSize);
                        //spawnedObject.transform.GetChild(i).gameObject.transform.position = gridPos;
                        Debug.Log("block of QuestionCube is built on " + pos.x + " " + pos.y + " " + pos.z);
                    }*/



                    /*
                    answerPos.Add(spawnedObject.transform.GetChild(3).gameObject.transform.position + Vector3.right * 0.1f);
                    answerPos.Add(spawnedObject.transform.GetChild(10).gameObject.transform.position + Vector3.up * 0.1f);
                    answerPos.Add(spawnedObject.transform.GetChild(15).gameObject.transform.position + Vector3.up * 0.1f);
                    answerPos.Add(spawnedObject.transform.GetChild(11).gameObject.transform.position + Vector3.right * 0.1f);
                    answerPos.Add(spawnedObject.transform.GetChild(11).gameObject.transform.position + Vector3.right * 0.2f);
                    answerPos.Add(spawnedObject.transform.GetChild(11).gameObject.transform.position + Vector3.back * 0.1f);
                    answerPos.Add(spawnedObject.transform.GetChild(11).gameObject.transform.position + Vector3.back * 0.1f + Vector3.right * 0.1f);
                    answerPos.Add(spawnedObject.transform.GetChild(11).gameObject.transform.position + Vector3.back * 0.1f + Vector3.right * 0.2f);

                    foreach (Vector3 pos in answerPos)
                        Debug.Log("answerPos is " + pos.x + " " + pos.y + " " + pos.z);
                    */

                    //QuestionAppear();

                    //DisabledPlaneDetection();
                }
                /*
                else
                {
                    // update position
                    spawnedObject.transform.position = hitPose.position + Vector3.up * (spawnedObject.transform.localScale.y / 2);
                }
                */
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

    public void Correct()
    {
        if (checkText != null)
            checkText.SetActive(true);
    }

    private Vector3 Parsedot(Vector3 pos)
    {
        var strX = pos.x.ToString("0.00");
        var strY = pos.y.ToString("0.0");
        var strZ = pos.z.ToString("0.00");
        Vector3 newPos = new Vector3(float.Parse(strX), float.Parse(strY), float.Parse(strZ));
        return newPos;
    }
    /*
    public void LocateQuestionCube() 
    {

        Instantiate(groundToInstantiate, hitPose.position, hitPose.rotation);
    }*/
    /*
    public Vector3 GridPos(Vector3 point)
    {
        return new Vector3(Mathf.Round(point.x / gridSize) * gridSize, Mathf.Round(point.y / gridSize) * gridSize + 0.05f, Mathf.Round(point.z / gridSize) * gridSize);
    }*/
}
