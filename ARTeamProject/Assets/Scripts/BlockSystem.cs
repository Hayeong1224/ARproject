using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlockSystem : MonoBehaviour
{
    [SerializeField] //This is private but, it can be accessed by inspector
    private BlockType[] allBlockTypes;

    [HideInInspector] //Makes a variable not show up in the inspector but be serialized.
    public Dictionary<int, Block> allBlocks = new Dictionary<int, Block>();

    private void Awake()
    {
        for (int i = 0; i < allBlockTypes.Length; i++)
        {
            BlockType newBlockType = allBlockTypes[i];
            Block newBlock = new Block(i,newBlockType.blockName, newBlockType.blockMat);
            allBlocks[i] = newBlock;
            Debug.Log("Block added to dictionary " + allBlocks[i].blockName);
        }
    }
}

public class Block
{
    public int blockID;
    public string blockName;
    public Material blockMaterial;

    public Block(int id, string name, Material mat)
    {
        blockID = id;
        blockName = name;
        blockMaterial = mat;
    }
}

[Serializable] //Makes user defined class or struct can be accessed in inspector 
public struct BlockType
{
    public string blockName;
    public Material blockMat;
}
