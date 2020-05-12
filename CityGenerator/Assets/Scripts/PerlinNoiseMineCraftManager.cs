using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerlinNoiseMineCraftManager : MonoBehaviour
{
    [SerializeField] private int ChunkSize = 32;
    [SerializeField] private float TerrainHeight = 3f;
    [SerializeField] private float GridSize = 1f;
    [SerializeField] private float HeightModule = 1f;
    [SerializeField] private int TerrainNoiseScale = 1;
    [SerializeField] private Vector2 TerrainSeedOffset = Vector2.zero;
    [SerializeField] private int ColorNoiseScale = 1;
    [SerializeField] private Color ColorOffset = new Color(0.5f, 0.5f, 0.5f);
    [SerializeField] private Material Material;

    private Mesh terrainChunkMesh;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

    void Awake()
    {
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshFilter = gameObject.AddComponent<MeshFilter>();
    }

    void Start()
    {
    }

    private void Update()
    {
        if (ChunkSize <= 0)
        {
            ChunkSize = 1;
        }

        if (ChunkSize > 32)
        {
            ChunkSize = 32;
        }

        if (TerrainHeight < 0)
        {
            TerrainHeight = 0;
        }

        if (GridSize < 0)
        {
            GridSize = 0;
        }

        if (HeightModule <= 0)
        {
            HeightModule = 0.001f;
        }

        if (TerrainNoiseScale <= 0)
        {
            TerrainNoiseScale = 1;
        }

        if (ColorNoiseScale <= 0)
        {
            ColorNoiseScale = 1;
        }

        RefreshMesh();
    }

    private void RefreshMesh()
    {
        terrainChunkMesh = new Mesh();
        Vector3[,] positions = new Vector3[ChunkSize, ChunkSize];
        for (int i = 0; i < ChunkSize; i++)
        {
            for (int j = 0; j < ChunkSize; j++)
            {
                float xPos = transform.position.x + i * GridSize;
                float zPos = transform.position.z + j * GridSize;
                float yPos = (Perlin.Noise(xPos * TerrainNoiseScale / 1000f + TerrainSeedOffset.x, zPos * TerrainNoiseScale / 1000f + TerrainSeedOffset.y) + 1) / 2 * TerrainHeight;

                positions[i, j] = new Vector3(xPos - transform.position.x, Mathf.RoundToInt(yPos / HeightModule) * HeightModule, zPos - transform.position.z);
            }
        }

        List<int> _trisList = new List<int>();
        List<Vector3> _vertList = new List<Vector3>();
        List<Color> _verColorList = new List<Color>();

        List<Vector3> _normalList = new List<Vector3>();
        for (int i = 0; i < ChunkSize; i++)
        {
            for (int j = 0; j < ChunkSize; j++)
            {
                float r = Perlin.Noise((float) (i + 59) / ChunkSize * ColorNoiseScale / 1000f, (float) (j + 119) / ChunkSize * ColorNoiseScale / 1000f);
                float g = Perlin.Noise((float) (i + 119) / ChunkSize * ColorNoiseScale / 1000f, (float) (j + 59) / ChunkSize * ColorNoiseScale / 1000f);
                float b = Perlin.Noise((float) (i + 13) / ChunkSize * ColorNoiseScale / 1000f, (float) (j + 97) / ChunkSize * ColorNoiseScale / 1000f);

                Color c = new Color(r + ColorOffset.r, g + ColorOffset.g, b + ColorOffset.b);

                // Generate top quads
                {
                    int offset = _vertList.Count;

                    Vector3 p1 = positions[i, j] + GridSize / 2 * Vector3.right + GridSize / 2 * Vector3.forward;
                    Vector3 p2 = positions[i, j] - GridSize / 2 * Vector3.right + GridSize / 2 * Vector3.forward;
                    Vector3 p3 = positions[i, j] - GridSize / 2 * Vector3.right - GridSize / 2 * Vector3.forward;
                    Vector3 p4 = positions[i, j] + GridSize / 2 * Vector3.right - GridSize / 2 * Vector3.forward;

                    _vertList.Add(p1);
                    _vertList.Add(p3);
                    _vertList.Add(p2);
                    _vertList.Add(p1);
                    _vertList.Add(p4);
                    _vertList.Add(p3);

                    _verColorList.Add(c);
                    _verColorList.Add(c);
                    _verColorList.Add(c);
                    _verColorList.Add(c);
                    _verColorList.Add(c);
                    _verColorList.Add(c);

                    Vector3 v1 = p1 - p2;
                    Vector3 v2 = p3 - p2;
                    Vector3 normal = Vector3.Cross(v1, v2).normalized;

                    _normalList.Add(normal);
                    _normalList.Add(normal);
                    _normalList.Add(normal);
                    _normalList.Add(normal);
                    _normalList.Add(normal);
                    _normalList.Add(normal);

                    for (int k = 0; k < 6; k++)
                    {
                        _trisList.Add(k + offset);
                    }
                }

                // Generate side quads
                {
                    if (j + 1 < ChunkSize && positions[i, j].y > positions[i, j + 1].y)
                    {
                        int offset = _vertList.Count;

                        Vector3 p1_1 = positions[i, j] + GridSize / 2 * Vector3.right + GridSize / 2 * Vector3.forward;
                        Vector3 p1_2 = positions[i, j] - GridSize / 2 * Vector3.right + GridSize / 2 * Vector3.forward;
                        Vector3 p2_1 = positions[i, j + 1] - GridSize / 2 * Vector3.right - GridSize / 2 * Vector3.forward;
                        Vector3 p2_2 = positions[i, j + 1] + GridSize / 2 * Vector3.right - GridSize / 2 * Vector3.forward;

                        _vertList.Add(p1_1);
                        _vertList.Add(p1_2);
                        _vertList.Add(p2_1);
                        _vertList.Add(p1_1);
                        _vertList.Add(p2_1);
                        _vertList.Add(p2_2);

                        _verColorList.Add(c);
                        _verColorList.Add(c);
                        _verColorList.Add(c);
                        _verColorList.Add(c);
                        _verColorList.Add(c);
                        _verColorList.Add(c);

                        Vector3 v1 = p1_1 - p1_2;
                        Vector3 v2 = p2_1 - p1_2;
                        Vector3 normal = Vector3.Cross(v1, v2).normalized;

                        _normalList.Add(normal);
                        _normalList.Add(normal);
                        _normalList.Add(normal);
                        _normalList.Add(normal);
                        _normalList.Add(normal);
                        _normalList.Add(normal);

                        for (int k = 0; k < 6; k++)
                        {
                            _trisList.Add(k + offset);
                        }
                    }

                    if (j > 0 && positions[i, j].y > positions[i, j - 1].y)
                    {
                        int offset = _vertList.Count;

                        Vector3 p1_1 = positions[i, j] + GridSize / 2 * Vector3.right - GridSize / 2 * Vector3.forward;
                        Vector3 p1_2 = positions[i, j] - GridSize / 2 * Vector3.right - GridSize / 2 * Vector3.forward;
                        Vector3 p2_1 = positions[i, j - 1] - GridSize / 2 * Vector3.right + GridSize / 2 * Vector3.forward;
                        Vector3 p2_2 = positions[i, j - 1] + GridSize / 2 * Vector3.right + GridSize / 2 * Vector3.forward;

                        _vertList.Add(p1_1);
                        _vertList.Add(p2_1);
                        _vertList.Add(p1_2);
                        _vertList.Add(p1_1);
                        _vertList.Add(p2_2);
                        _vertList.Add(p2_1);

                        _verColorList.Add(c);
                        _verColorList.Add(c);
                        _verColorList.Add(c);
                        _verColorList.Add(c);
                        _verColorList.Add(c);
                        _verColorList.Add(c);

                        Vector3 v1 = p1_1 - p1_2;
                        Vector3 v2 = p2_1 - p1_2;
                        Vector3 normal = Vector3.Cross(v1, v2).normalized;

                        _normalList.Add(normal);
                        _normalList.Add(normal);
                        _normalList.Add(normal);
                        _normalList.Add(normal);
                        _normalList.Add(normal);
                        _normalList.Add(normal);

                        for (int k = 0; k < 6; k++)
                        {
                            _trisList.Add(k + offset);
                        }
                    }

                    if (i + 1 < ChunkSize && positions[i, j].y > positions[i + 1, j].y)
                    {
                        int offset = _vertList.Count;

                        Vector3 p1_1 = positions[i, j] + GridSize / 2 * Vector3.right + GridSize / 2 * Vector3.forward;
                        Vector3 p1_2 = positions[i + 1, j] - GridSize / 2 * Vector3.right + GridSize / 2 * Vector3.forward;
                        Vector3 p2_1 = positions[i + 1, j] - GridSize / 2 * Vector3.right - GridSize / 2 * Vector3.forward;
                        Vector3 p2_2 = positions[i, j] + GridSize / 2 * Vector3.right - GridSize / 2 * Vector3.forward;

                        _vertList.Add(p1_1);
                        _vertList.Add(p1_2);
                        _vertList.Add(p2_1);
                        _vertList.Add(p1_1);
                        _vertList.Add(p2_1);
                        _vertList.Add(p2_2);

                        _verColorList.Add(c);
                        _verColorList.Add(c);
                        _verColorList.Add(c);
                        _verColorList.Add(c);
                        _verColorList.Add(c);
                        _verColorList.Add(c);

                        Vector3 v1 = p1_1 - p1_2;
                        Vector3 v2 = p2_1 - p1_2;
                        Vector3 normal = Vector3.Cross(v1, v2).normalized;

                        _normalList.Add(normal);
                        _normalList.Add(normal);
                        _normalList.Add(normal);
                        _normalList.Add(normal);
                        _normalList.Add(normal);
                        _normalList.Add(normal);

                        for (int k = 0; k < 6; k++)
                        {
                            _trisList.Add(k + offset);
                        }
                    }

                    if (i > 0 && positions[i, j].y > positions[i - 1, j].y)
                    {
                        int offset = _vertList.Count;

                        Vector3 p1_1 = positions[i, j] - GridSize / 2 * Vector3.right + GridSize / 2 * Vector3.forward;
                        Vector3 p1_2 = positions[i - 1, j] + GridSize / 2 * Vector3.right + GridSize / 2 * Vector3.forward;
                        Vector3 p2_1 = positions[i - 1, j] + GridSize / 2 * Vector3.right - GridSize / 2 * Vector3.forward;
                        Vector3 p2_2 = positions[i, j] - GridSize / 2 * Vector3.right - GridSize / 2 * Vector3.forward;

                        _vertList.Add(p1_1);
                        _vertList.Add(p2_1);
                        _vertList.Add(p1_2);
                        _vertList.Add(p1_1);
                        _vertList.Add(p2_2);
                        _vertList.Add(p2_1);

                        _verColorList.Add(c);
                        _verColorList.Add(c);
                        _verColorList.Add(c);
                        _verColorList.Add(c);
                        _verColorList.Add(c);
                        _verColorList.Add(c);

                        Vector3 v1 = p1_1 - p1_2;
                        Vector3 v2 = p2_1 - p1_2;
                        Vector3 normal = Vector3.Cross(v1, v2).normalized;

                        _normalList.Add(normal);
                        _normalList.Add(normal);
                        _normalList.Add(normal);
                        _normalList.Add(normal);
                        _normalList.Add(normal);
                        _normalList.Add(normal);

                        for (int k = 0; k < 6; k++)
                        {
                            _trisList.Add(k + offset);
                        }
                    }
                }
            }
        }

        terrainChunkMesh.vertices = _vertList.ToArray();
        terrainChunkMesh.colors = _verColorList.ToArray();
        terrainChunkMesh.triangles = _trisList.ToArray();
        terrainChunkMesh.normals = _normalList.ToArray();
        meshRenderer.material = Material;
        meshFilter.mesh = terrainChunkMesh;
    }
}