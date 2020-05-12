using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    public float GroundSize = 100f;
    public float Interval = 3f;
    [Range(10, 100)] [SerializeField] private int IncreaseProb_1x1;
    [Range(10, 100)] [SerializeField] private int IncreaseProb_2x2;
    [Range(10, 100)] [SerializeField] private int DecreaseProb_2x2;
    [SerializeField] private Color BloomColor;
    [Range(0, 1)] public float LevelOffset;

    [Range(2, 10)] public int StreetInterval_X;
    [Range(2, 10)] public int StreetInterval_Y;

    [Space] [Space] [SerializeField] private PostProcessVolume PostProcessVolume;

    internal SortedDictionary<BlockType, int> IncreaseProbabilityDict = new SortedDictionary<BlockType, int>();
    internal SortedDictionary<BlockType, int> DecreaseProbabilityDict = new SortedDictionary<BlockType, int>();

    public GameObject BuildingsRoot;
    public Transform Environment;
    public Ground Ground;
    public Material[] BuildingMats;
    public Material[] HighLightMats;

    void Awake()
    {
        IncreaseProbabilityDict.Add(BlockType.Block_1x1, IncreaseProb_1x1);
        IncreaseProbabilityDict.Add(BlockType.Block_2x2, IncreaseProb_2x2);
        DecreaseProbabilityDict.Add(BlockType.Block_1x1, 0);
        DecreaseProbabilityDict.Add(BlockType.Block_2x2, DecreaseProb_2x2);
    }

    void Start()
    {
        Ground.Generate();
    }

    void Update()
    {
        IncreaseProbabilityDict[BlockType.Block_1x1] = IncreaseProb_1x1;
        IncreaseProbabilityDict[BlockType.Block_2x2] = IncreaseProb_2x2;
        DecreaseProbabilityDict[BlockType.Block_1x1] = 0;
        DecreaseProbabilityDict[BlockType.Block_2x2] = DecreaseProb_2x2;
        Bloom bloom = PostProcessVolume.profile.GetSetting<Bloom>();
        bloom.color.value = BloomColor;

        if (Input.GetKeyUp(KeyCode.F10))
        {
            Destroy(BuildingsRoot);
            BuildingsRoot = new GameObject("BuildingsRoot");
            Ground.Generate();
        }
    }

    public Material GetHighlightMat()
    {
        return HighLightMats[Random.Range(0, HighLightMats.Length)];
    }

    public Material GetBuildingMat()
    {
        return BuildingMats[Random.Range(0, BuildingMats.Length)];
    }
}