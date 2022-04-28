using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
public class soma : MonoBehaviour
{
    public ARRaycastManager aRRaycaster;
    [SerializeField]
    private Camera playerCamera;
    int i=0;
    //private bool buildModeOn = false;
    private bool canBuild = false;

    
    private BlockSystem bSys;

    [SerializeField]
    private LayerMask buildableSurfacesLayer;

    private Collision detection;
    private Vector3 buildPos;

    private GameObject currentTemplateBlock;

    [SerializeField]
    private GameObject[] blockTemplatePrefab;
    [SerializeField]
    private GameObject[] blockPrefab;

    [SerializeField]
    //private Material templateMaterial;

    private int blockSelectCounter = 0;
    
    
    private void Start()
    {
        bSys = GetComponent<BlockSystem>();
    }

    private void Update()
    {
      

        RaycastHit buildPosHIt;
            
              

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out buildPosHIt, 10, buildableSurfacesLayer))
            {
                Vector3 point = buildPosHIt.point;
                 float gridSize = 0.1f;
                //grid placement
               buildPos = new Vector3(Mathf.Round(point.x/ gridSize) * gridSize, Mathf.Round(point.y/ gridSize) * gridSize +0.1f, Mathf.Round(point.z/ gridSize) * gridSize);
                //canBuild = true;
                /*
            Touch touch = Input.GetTouch(0);

            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (aRRaycaster.Raycast(touch.position, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;
                buildPos = hitPose.position;
            }
                */
            canBuild = true;
          
            //OnTriggerEnter(buildPosHIt.collider.tag);
            //Debug.Log(buildPosHIt.collider.name);
        }
            
            else if(currentTemplateBlock != null) //else if로 추가하기
            {
                Destroy(currentTemplateBlock.gameObject);
               canBuild = false;
            }
            
     

        if (canBuild && currentTemplateBlock == null)
        {
            currentTemplateBlock = Instantiate(blockTemplatePrefab[i], buildPos, Quaternion.identity);
            //currentTemplateBlock.GetComponent<MeshRenderer>().material = templateMaterial;
        }

        if (canBuild && currentTemplateBlock != null)
        {
            currentTemplateBlock.transform.position = buildPos;

            /*if (Input.GetMouseButtonDown(0))
            {
                PlaceBlock();
            }*/
        }
       
    }

    
    public void PlaceBlock()
    {
        GameObject newBlock = Instantiate(blockPrefab[i], currentTemplateBlock.transform.position, currentTemplateBlock.transform.rotation);
        Block tempBlock = bSys.allBlocks[blockSelectCounter];
        newBlock.name = tempBlock.blockName + ".Block";
        newBlock.GetComponent<MeshRenderer>().material = tempBlock.blockMaterial;
    }

    public void ChangeColor()
    {
        blockSelectCounter++;
        if (blockSelectCounter >= bSys.allBlocks.Count) blockSelectCounter = 0;
    }

    public void ChangeShape()
    {
     
        if(i==6)
        {
            i = 0;
        }
        else
        {
            i++;
        }
        Destroy(currentTemplateBlock.gameObject);
        currentTemplateBlock = Instantiate(blockTemplatePrefab[i]);
    }
    public void ResetBlock()
    {
        GameObject[] placedSomas;
        placedSomas = GameObject.FindGameObjectsWithTag("PlacedSoma");
        if (placedSomas != null)
        {
            foreach (GameObject placedSoma in placedSomas)
                Destroy(placedSoma);
        }
    }
    public void touchmove()
    {
        Touch touch = Input.GetTouch(0);

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if(aRRaycaster.Raycast(touch.position,hits,UnityEngine.XR.ARSubsystems.TrackableType.Planes))
        {
            Pose hitPose = hits[0].pose;
        }
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlacedSoma"))
        {
            currentTemplateBlock.gameObject.SetActive(false);

        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlacedSoma"))
        {
            currentTemplateBlock.gameObject.SetActive(true);

        }
    }

    public void yrotate()
    {
        currentTemplateBlock.transform.Rotate(0, 90, 0);
    }
    public void xrotate()
    {
        currentTemplateBlock.transform.Rotate(90, 0, 0);
    }

}
