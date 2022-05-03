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

    public GameObject xbotton;
    public GameObject addbotton;

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
    public AudioSource clickSound;
    
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
              
               buildPos = new Vector3(Mathf.Round(point.x/ gridSize) * gridSize, Mathf.Round(point.y/ gridSize) * gridSize +0.1f, Mathf.Round(point.z/ gridSize) * gridSize);
           
            canBuild = true;
          
      
        }
            
            else if(currentTemplateBlock != null) //else if로 추가하기
            {
                Destroy(currentTemplateBlock.gameObject);
               canBuild = false;
            }
            
     

        if (canBuild && currentTemplateBlock == null)
        {
            currentTemplateBlock = Instantiate(blockTemplatePrefab[i], buildPos, Quaternion.identity);
      
        }

        if (canBuild && currentTemplateBlock != null)
        {
            currentTemplateBlock.transform.position = buildPos;

          
        }
       
    }

    
    public void PlaceBlock()
    {
        if (colmanager.rendcheck == false)
        {
            clickSound.Play();
            GameObject newBlock = Instantiate(blockPrefab[i], currentTemplateBlock.transform.position, currentTemplateBlock.transform.rotation);

            Block tempBlock = bSys.allBlocks[blockSelectCounter];
        }
      
        
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
        clickSound.Play();
    }
    public void ResetBlock()
    {
        clickSound.Play();
        GameObject[] placedSomas;
        placedSomas = GameObject.FindGameObjectsWithTag("PlacedSoma");
        if (placedSomas != null)
        {
            foreach (GameObject placedSoma in placedSomas)
                Destroy(placedSoma);
        }
    }
   

    public void yrotate()
    {
        clickSound.Play();
        currentTemplateBlock.transform.Rotate(0, 90, 0);
    }
    public void xrotate()
    {
        clickSound.Play();
        currentTemplateBlock.transform.Rotate(90, 0, 0);
    }

}
