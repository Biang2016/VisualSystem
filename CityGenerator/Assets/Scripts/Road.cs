using UnityEngine;
using System.Collections;

public class Road : MonoBehaviour
{
    private MeshRenderer MeshRenderer;

    void Awake()
    {
        MeshRenderer = GetComponent<MeshRenderer>();
    }

    public void Initialize()
    {
        MeshRenderer.materials[0] = GameManager.Instance.GetBuildingMat();
    }
}