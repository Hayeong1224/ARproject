using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Study1BuildingSystem : MonoBehaviour
{
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

    private int blockSelectCounter = 0;

    private Quaternion questionRotation;

    private List<Vector3> blockPos = new List<Vector3>();

    [SerializeField]
    private GameObject objectToInstantiate;

    private bool questionIsLocated = false;

    [SerializeField]
    private GameObject study1Manager;

    private GameObject[] placedBlocks;

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
            buildPos = new Vector3(Mathf.Round(point.x / gridSize) * gridSize, Mathf.Round(point.y / gridSize) * gridSize + 0.05f, Mathf.Round(point.z / gridSize) * gridSize);
            canBuild = true;
        }
        else if (currentTemplateBlock != null)
        {
            Destroy(currentTemplateBlock.gameObject);
            canBuild = false;

        }

        if (canBuild && currentTemplateBlock == null)
        {
            currentTemplateBlock = Instantiate(blockTemplatePrefab, buildPos, Quaternion.identity);
            currentTemplateBlock.transform.rotation = questionRotation;
        }

        if (canBuild && currentTemplateBlock != null)
        {
            currentTemplateBlock.transform.position = buildPos;
        }
    }


    public void PlaceBlock()
    {
        // if question is not located, question appear
        if (!questionIsLocated)
        {
            LocateQuestion();
            questionIsLocated = true;
        }  
        // add block
        else
        {
            GameObject newBlock = Instantiate(blockPrefab, buildPos, Quaternion.identity);
            newBlock.transform.rotation = questionRotation;
            Block tempBlock = bSys.allBlocks[blockSelectCounter];
            newBlock.name = tempBlock.blockName + ".Block";
            newBlock.GetComponent<MeshRenderer>().material = tempBlock.blockMaterial;
            blockPos.Add(buildPos);
            Debug.Log("Player build a block on" + buildPos.x + " " + buildPos.y + " " + buildPos.z);
        }
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

    public void LocateQuestion()
    {
        buildPos = new Vector3(buildPos.x, buildPos.y + 0.1f, buildPos.z);
        //Instantiate questionCube
        GameObject parent = Instantiate(objectToInstantiate, buildPos, Quaternion.identity);

        //questionUI appear
        study1Manager.SendMessage("QuestionAppear");

        Debug.Log("The question is located on " + buildPos);

        List<Vector3> QuestionPos = new List<Vector3>();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Vector3 pos = parent.transform.GetChild(i).gameObject.transform.position;
            QuestionPos.Add(pos);
            Debug.Log("Block of questionCube is built on " + pos.x + " " + pos.y + " " + pos.z);
        }
    }

}
