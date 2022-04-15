using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Questions : MonoBehaviour
{
    public ARRaycastManager arRaycaster;
    //어디에 배치할 오브젝트인지
    public GameObject[] placedObject;

    public LayerMask layer;

    float point_x,point_y,point_z;
    Vector3 point,point2,point3;
 
    // Start is called before the first frame update
    void Start()
    {
        point_x = Camera.main.transform.position.x;
        point_y = Camera.main.transform.position.y;
        point_z = Camera.main.transform.position.z;
        point = new Vector3(point_x, point_y-1f, point_z + 1f);
        point2 = new Vector3(point_x - 0.6f, point_y-1f, point_z + 1f);
        point3 = new Vector3(point_x + 0.6f, point_y-1f, point_z + 1f);
        
    }

    // Update is called once per frame
    void Update()
    {
        Instantiate(placedObject[0], point, Quaternion.identity);
        Instantiate(placedObject[1], point2, Quaternion.identity);
        Instantiate(placedObject[2], point3, Quaternion.identity);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit))
        {
           
           // hit.collider.gameObject.GetComponentInParent<Transform>().Rotate(new Vector3(30 * Time.deltaTime, 0, 0));
           // Debug.Log(hit.collider.transform.localRotation);

        }
       
    }

    private void UpdateCenterObject()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)) ;

        List<ARRaycastHit> hits=new List<ARRaycastHit>();
        //Planes이라는 객체 닿으면 반환하겠다는 뜻
        arRaycaster.Raycast(screenCenter, hits, TrackableType.Planes);

        if (hits.Count>0)//부딪히는 평면이 있다면
        {
            Pose placementPose = hits[0].pose;//부딪히는 객체의 위치 반환. 그중 가장 먼저 부딪친 그 위치
            for (int i = 0; i < 3; i++)
            {
                placedObject[i].SetActive(true);//안보이던 객체를 눈에 보이게 바꾸는 코드
            }
            Debug.Log(placementPose.position);
            for (int i = 0; i < 3; i++)
            {
                placedObject[i].transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            }
        }
        else//부딪히는 평면이 없다면
        {
            for (int i = 0; i < 3; i++)
            {
                placedObject[i].SetActive(false);//안보이던 객체를 눈에 보이게 바꾸는 코드
            }
        }
    }
    
    private void UpdateQuestions()
    {
        if (Input.touchCount > 0)
        {
            Touch touch=Input.GetTouch(0);
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, 10, layer))
            {
                Vector2 touchpoint = touch.position;
                Instantiate(placedObject[0], touchpoint, Quaternion.identity);
                //Instantiate(placedObject[1], point2, Quaternion.identity);
                //Instantiate(placedObject[2], point3, Quaternion.identity);
            }
        }
    }
}
