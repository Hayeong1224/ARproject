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
        else if (currentTemplateBlock != null) //else if로 추가하기
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
        GameObject newBlock = Instantiate(blockPrefab, buildPos, Quaternion.identity);
        newBlock.transform.rotation = questionRotation;
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

    public void GetRotation(Quaternion rotation)
    {
        questionRotation = rotation;
    }
}
