using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soma : MonoBehaviour
{
    [SerializeField]
    private Camera playerCamera;
    int i=0;
    //private bool buildModeOn = false;
    private bool canBuild = false;

    private BlockSystem bSys;

    [SerializeField]
    private LayerMask buildableSurfacesLayer;

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
        /*
        if (Input.GetKeyDown("e"))
        {
            buildModeOn = !buildModeOn;

            if (buildModeOn)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else 
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        if (Input.GetKeyDown("r"))
        {
            blockSelectCounter++;
            if (blockSelectCounter >= bSys.allBlocks.Count) blockSelectCounter = 0;
        }*/

        //if (buildModeOn)
        //{
            RaycastHit buildPosHIt;

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out buildPosHIt, 10, buildableSurfacesLayer))
            {
                Vector3 point = buildPosHIt.point;
            float gridSize = 0.1f;
                //grid placement
                buildPos = new Vector3(Mathf.Round(point.x/ gridSize) * gridSize, Mathf.Round(point.y/ gridSize) * gridSize +0.1f, Mathf.Round(point.z/ gridSize) * gridSize);
                canBuild = true;
                Debug.Log(buildPosHIt.collider.name);
            }
            
            else if(currentTemplateBlock != null) //else if�� �߰��ϱ�
            {
                Destroy(currentTemplateBlock.gameObject);
               canBuild = false;
            }
            
        //}
        /*
        if (!buildModeOn && currentTemplateBlock != null)
        {
            Destroy(currentTemplateBlock.gameObject);
            canBuild = false;
        }*/

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
        GameObject newBlock = Instantiate(blockPrefab[i], currentTemplateBlock.transform.position, Quaternion.identity);
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

}
