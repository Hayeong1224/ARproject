using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Questions : MonoBehaviour
{
    public ARRaycastManager arRaycaster;
    //��� ��ġ�� ������Ʈ����
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
        //Planes�̶�� ��ü ������ ��ȯ�ϰڴٴ� ��
        arRaycaster.Raycast(screenCenter, hits, TrackableType.Planes);

        if (hits.Count>0)//�ε����� ����� �ִٸ�
        {
            Pose placementPose = hits[0].pose;//�ε����� ��ü�� ��ġ ��ȯ. ���� ���� ���� �ε�ģ �� ��ġ
            for (int i = 0; i < 3; i++)
            {
                placedObject[i].SetActive(true);//�Ⱥ��̴� ��ü�� ���� ���̰� �ٲٴ� �ڵ�
            }
            Debug.Log(placementPose.position);
            for (int i = 0; i < 3; i++)
            {
                placedObject[i].transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            }
        }
        else//�ε����� ����� ���ٸ�
        {
            for (int i = 0; i < 3; i++)
            {
                placedObject[i].SetActive(false);//�Ⱥ��̴� ��ü�� ���� ���̰� �ٲٴ� �ڵ�
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
