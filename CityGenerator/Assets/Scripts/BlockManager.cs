using UnityEngine;
using System.Collections.Generic;

public class BlockManager : MonoSingleton<BlockManager>
{
    private SortedDictionary<BlockType, SortedDictionary<BlockPart, List<GameObject>>> BlockDict = new SortedDictionary<BlockType, SortedDictionary<BlockPart, List<GameObject>>>();

    public const int BlockPartCount = 4;

    void Awake()
    {
        BuildingBlock[] bbs = Resources.FindObjectsOfTypeAll<BuildingBlock>();

        foreach (BuildingBlock bb in bbs)
        {
            if (!BlockDict.ContainsKey(bb.BlockType))
            {
                BlockDict.Add(bb.BlockType, new SortedDictionary<BlockPart, List<GameObject>>());
            }

            if (!BlockDict[bb.BlockType].ContainsKey(bb.BlockPart))
            {
                BlockDict[bb.BlockType].Add(bb.BlockPart, new List<GameObject>());
            }

            BlockDict[bb.BlockType][bb.BlockPart].Add(bb.gameObject);
        }
    }

    public GameObject GetBlock(Vector3 pos, BlockType blockType, BlockPart parentBlockPart)
    {
        BlockPart bp = parentBlockPart;
        BlockType resBlockType = blockType;

        bool increaseBlockPart = Random.Range(0, 100) < GameManager.instance.IncreaseProbabilityDict[blockType];
        if (pos.y < 1f)
        {
            increaseBlockPart = false;
        }

        while (increaseBlockPart && bp != BlockPart.None)
        {
            bp = (BlockPart) (((int) bp) + 1);
            increaseBlockPart = Random.Range(0, 100) < GameManager.instance.IncreaseProbabilityDict[blockType];
        }

        bool decreaseBlockSize = Random.Range(0, 100) < GameManager.instance.DecreaseProbabilityDict[resBlockType];
        if (pos.y < 1f)
        {
            decreaseBlockSize = false;
        }

        while (decreaseBlockSize && resBlockType != BlockType.Block_1x1)
        {
            resBlockType = (BlockType) (((int) resBlockType) - 1);
            decreaseBlockSize = Random.Range(0, 100) < GameManager.instance.DecreaseProbabilityDict[resBlockType];
        }

        if (bp == BlockPart.None) return null;

        BlockDict.TryGetValue(resBlockType, out SortedDictionary<BlockPart, List<GameObject>> dict);
        if (dict != null)
        {
            dict.TryGetValue(bp, out List<GameObject> list);
            if (list != null && list.Count > 0)
            {
                return list[Random.Range(0, list.Count)];
            }
        }

        return null;
    }
}