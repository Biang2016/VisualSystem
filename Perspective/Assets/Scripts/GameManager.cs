using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Prefab;

    public float Radius = 5f;
    public float RoundOffset = 2f;
    public int RoundCount = 10;
    public int PrefabPerRound = 10;

    private List<GameObject> Prefabs = new List<GameObject>();

    void Update()
    {
        foreach (GameObject p in Prefabs)
        {
            DestroyImmediate(p);
        }

        Prefabs.Clear();

        float angle = 360f / PrefabPerRound;
        for (int round = 0; round < RoundCount; round++)
        {
            for (int i = 0; i < PrefabPerRound; i++)
            {
                float prefabOffset = RoundOffset / PrefabPerRound;
                Vector3 pos = new Vector3(Mathf.Sin(angle * i * Mathf.Deg2Rad) * Radius, Mathf.Cos(angle * i * Mathf.Deg2Rad) * Radius, prefabOffset * i + RoundOffset * round);
                GameObject go = Instantiate(Prefab, transform);
                Prefabs.Add(go);
                go.transform.position = pos;
            }
        }
    }
}