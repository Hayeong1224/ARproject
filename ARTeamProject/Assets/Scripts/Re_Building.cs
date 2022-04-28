using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Re_Building : MonoBehaviour
{

    //private bool buildModeOn = false;
    private bool canBuild = false;

    private BlockSystem bSys;

    [SerializeField]
    private LayerMask buildableSurfacesLayer;

    private Vector3 buildPos;

    private GameObject currentTemplateBlock;

    [SerializeField]
    private GameObject blockTemplatePrefab;
    [SerializeField]
    private GameObject blockPrefab;

    [SerializeField]
    //private Material templateMaterial;

    private int blockSelectCounter = 0;

    Vector3 rotateChange = new Vector3(0, 0, 0);



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
            buildPos = new Vector3(Mathf.Round(point.x / gridSize) * gridSize, Mathf.Round(point.y / gridSize) * gridSize + 0.05f, Mathf.Round(point.z / gridSize) * gridSize);
            canBuild = true;
        }
        else if (currentTemplateBlock != null) //else if�� �߰��ϱ�
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
            currentTemplateBlock = Instantiate(blockTemplatePrefab, buildPos, Quaternion.identity);

            currentTemplateBlock.transform.localEulerAngles = rotateChange;
            Debug.Log(currentTemplateBlock.transform.localEulerAngles);
            //currentTemplateBlock.GetComponent<MeshRenderer>().material = templateMaterial;
        }

        if (canBuild && currentTemplateBlock != null)
        {
            currentTemplateBlock.transform.position = buildPos;
            currentTemplateBlock.transform.localEulerAngles = rotateChange;
            Debug.Log("2" + currentTemplateBlock.transform.localEulerAngles);
            /*if (Input.GetMouseButtonDown(0))
            {
                PlaceBlock();
            }*/
        }
    }

    public void PlaceBlock()
    {
        GameObject newBlock = Instantiate(blockPrefab, buildPos, Quaternion.identity);
        newBlock.transform.localEulerAngles = rotateChange;
        Block tempBlock = bSys.allBlocks[blockSelectCounter];
        newBlock.name = tempBlock.blockName + ".Block";
        newBlock.GetComponent<MeshRenderer>().material = tempBlock.blockMaterial;
    }

    public void ChangeColor()
    {
        blockSelectCounter++;
        if (blockSelectCounter >= bSys.allBlocks.Count) blockSelectCounter = 0;
    }

    public void ResetBlock()
    {
        GameObject[] placedBlocks;
        placedBlocks = GameObject.FindGameObjectsWithTag("PlacedBlock");
        if (placedBlocks != null)
        {
            foreach (GameObject placedBlock in placedBlocks)
                Destroy(placedBlock);
        }
    }
    public void RotateBlocks()
    {

        Debug.Log("turn!");
        GameObject[] placedBlocks;
        placedBlocks = GameObject.FindGameObjectsWithTag("PlacedBlock");
        GameObject ground = GameObject.FindWithTag("Ground");
        //parent 
        GameObject parentObj = new GameObject("Parent");
        if (ground != null)
        {
            parentObj.transform.position = ground.transform.position;
            Debug.Log(parentObj.transform.position);
            ground.transform.parent = parentObj.gameObject.transform;
        }
        else
        {
            Debug.Log("No ground!");
        }
        if (placedBlocks != null)
        {
            foreach (GameObject block in placedBlocks)
            {
                block.transform.parent = parentObj.gameObject.transform;
            }
        }
        else
        {
            Debug.Log("No block!");
        }

        rotateChange += new Vector3(0, 3, 0);
        parentObj.transform.localEulerAngles = new Vector3(0, 3, 0);

        //remove parent
        foreach (GameObject block in placedBlocks)
        {
            block.transform.parent = null;
        }
        ground.transform.parent = null;


    }
}