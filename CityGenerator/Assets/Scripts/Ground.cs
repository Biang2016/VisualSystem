using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour
{
    public GameObject RoadPrefab_X;
    public GameObject RoadPrefab_Y;

    void Start()
    {
    }

    public void Generate()
    {
        int count_i = 0;
        int count_j = 0;
        for (float i = 0; i < GameManager.Instance.GroundSize; i += GameManager.Instance.Interval)
        {
            count_i++;
            for (float j = 0; j < GameManager.Instance.GroundSize; j += GameManager.Instance.Interval)
            {
                count_j++;
                if (count_i % GameManager.Instance.StreetInterval_X == 0 || count_j % GameManager.Instance.StreetInterval_Y == 0)
                {
                    Vector3 pos = new Vector3(i - GameManager.Instance.GroundSize / 2f, 0, j - GameManager.Instance.GroundSize / 2f);
                    Quaternion rot = Quaternion.Euler(-89.9f, 0f, 0);
                    GameObject prefab = null;
                    if (count_i % GameManager.Instance.StreetInterval_X == 0 && count_j % GameManager.Instance.StreetInterval_Y != 0)
                    {
                        prefab = RoadPrefab_X;
                    }
                    else if (count_j % GameManager.Instance.StreetInterval_Y == 0 && count_i % GameManager.Instance.StreetInterval_X != 0)
                    {
                        prefab = RoadPrefab_Y;
                    }

                    if (prefab)
                    {
                        GameObject go = Instantiate(prefab, pos, rot, GameManager.Instance.BuildingsRoot.transform);
                        Road road = go.GetComponent<Road>();
                        road.Initialize();
                    }
                }
                else
                {
                    Vector3 pos = new Vector3(i - GameManager.Instance.GroundSize / 2f, 0, j - GameManager.Instance.GroundSize / 2f);
                    GameObject prefab = BlockManager.Instance.GetBlock(pos, BlockType.Block_2x2, BlockPart.Medium);
                    if (prefab)
                    {
                        Quaternion rot = Quaternion.Euler(-89.9f, 90 * Random.Range(0, 4), 0);
                        GameObject go = Instantiate(prefab, pos, rot, GameManager.Instance.BuildingsRoot.transform);
                        BuildingBlock bb = go.GetComponent<BuildingBlock>();
                        bb.Initialize();
                    }
                }
            }
        }
    }
}