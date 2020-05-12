using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlock : MonoBehaviour
{
    public BlockPart BlockPart;
    public BlockType BlockType;

    private BlockPivot[] BlockPivots;
    private MeshRenderer MeshRenderer;

    void Awake()
    {
        BlockPivots = GetComponentsInChildren<BlockPivot>();
        MeshRenderer = GetComponent<MeshRenderer>();
    }

    public void Initialize()
    {
        MeshRenderer.materials[0] = GameManager.Instance.GetBuildingMat();
        MeshRenderer.materials[1].SetColor("_EmissionColor", GameManager.Instance.GetHighlightMat().GetColor("_EmissionColor"));
        GenerateBlocksAbove();
    }

    public void GenerateBlocksAbove()
    {
        foreach (BlockPivot bp in BlockPivots)
        {
            GameObject prefab = BlockManager.Instance.GetBlock(transform.position, bp.BlockType, BlockPart);
            if (prefab)
            {
                Vector3 pos = bp.transform.position + GameManager.Instance.LevelOffset * 0.1f * GameManager.Instance.Interval * new Vector3(Random.Range(-1, 1f), 0, Random.Range(-1, 1f));
                Quaternion rot = Quaternion.Euler(-89.9f, 90 * Random.Range(0, 4), 0);
                GameObject go = Instantiate(prefab, pos, rot, transform);
                BuildingBlock bb = go.GetComponent<BuildingBlock>();
                bb.Initialize();
            }
        }
    }
}