using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
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

    private List<Vector3> blockPos = new List<Vector3>();

    private GameObject[] placedBlocks;

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


            /*
            //if any block is located in buildPos -> we can not build the block there.
            foreach (Vector3 pos in blockPos)
            {
                if (buildPos.Equals(pos))
                {
                    canBuild = false;
                    break;
                } 
            }*/
            
        }
        else if (currentTemplateBlock != null) //else if¡¤? ?©¬¡Æ¢®??¡¾?
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
        GameObject newBlock = Instantiate(blockPrefab, buildPos, Quaternion.identity);
        Block tempBlock = bSys.allBlocks[blockSelectCounter];
        newBlock.name = tempBlock.blockName + ".Block";
        newBlock.GetComponent<MeshRenderer>().material = tempBlock.blockMaterial;
        blockPos.Add(buildPos);

    }

    public void ChangeColor()
    {
        blockSelectCounter++;
        if (blockSelectCounter >= bSys.allBlocks.Count) blockSelectCounter = 0;
    }

    public void ResetBlock()
    {
        placedBlocks = GameObject.FindGameObjectsWithTag("PlacedBlock");
        if (placedBlocks != null)
        {
            foreach (GameObject placedBlock in placedBlocks)
                Destroy(placedBlock);
        }
        blockPos.Clear();
    }

    public void Undo()
    {
        placedBlocks = GameObject.FindGameObjectsWithTag("PlacedBlock");
        if (placedBlocks != null && blockPos.Count > 0)
        {
            foreach (GameObject placedBlock in placedBlocks)
                if (placedBlock.transform.position == blockPos[blockPos.Count - 1])
                {
                    Destroy(placedBlock);
                    blockPos.RemoveAt(blockPos.Count - 1);
                }
        }
    }
}