using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using PriorityQueueDemo;

public class SchoolingFishManager : MonoSingleton<SchoolingFishManager>
{
    [SerializeField] private float MinSize = 0.7f;
    [SerializeField] private float MaxSize = 1.2f;

    [SerializeField] private int FishNumber = 100;

    public SortedDictionary<int, Fish> Fishes = new SortedDictionary<int, Fish>();
    public List<GameObject> Foods = new List<GameObject>();
    public List<GameObject> Threatenings = new List<GameObject>();

    private int idGenerator = 0;

    void Start()
    {
        for (int i = 0; i < FishNumber; i++)
        {
            Fish fish = GameObjectPoolManager.Instance.PoolDict[GameObjectPoolManager.PrefabNames.Fish].AllocateGameObject<Fish>(transform);
            fish.transform.localScale = Vector3.one * Random.Range(MinSize, MaxSize);
            fish.transform.position = new Vector3(Random.Range(-100, 100), Random.Range(0, 100), Random.Range(-50, 50));
            fish.ID = idGenerator++;
            fish.Velocity = Vector3.zero;
            fish.VelocityLast = Vector3.zero;
            Fishes.Add(fish.ID, fish);
        }
    }

    public float SchoolingUpdateInterval = 1f;
    private float SchoolingUpdateIntervalTick = 0f;
    public float SchoolingSizeInterval = 3f;
    private float SchoolingSizeIntervalTick = 0f;

    [SerializeField] private float MaxVelocity = 50f;
    [SerializeField] private float MaxVelocity_Side = 10f;
    [SerializeField] private float MaxVelocity_Back = 5f;

    [Space] [SerializeField] private int FollowNeighborNumber = 10;
    [SerializeField] private float FoodGrav = 100f;
    [SerializeField] private float ThreateningGrav = -100f;
    [SerializeField] private float NeighborPullGrav = 30f;
    [SerializeField] private float NeighborPushGrav = -120f;
    [SerializeField] private float NeighborAxisPushGrav = 2f;
    [SerializeField] private float NeighborDistThreshold = 2f;
    private float NeighborDistThreshold_Final = 2f;
    [SerializeField] private float NeighborFollowDistThreshold = 20f;

    [SerializeField] private float ThreatenDistanceThreshold = 30f;
    [SerializeField] private float ThreatenDistanceSpeedUp = 2f;

    void Update()
    {
        //SchoolingUpdateIntervalTick += Time.deltaTime;
        //if (SchoolingUpdateIntervalTick > SchoolingUpdateInterval)
        //{
        SchoolingUpdate();
        //    SchoolingUpdateIntervalTick = 0;
        //}

        SchoolingSizeIntervalTick += Time.deltaTime;
        if (SchoolingSizeIntervalTick > SchoolingSizeInterval)
        {
            NeighborDistThreshold_Final = NeighborDistThreshold * Mathf.Pow(10, Random.Range(-0.5f, 6f));
            SchoolingSizeIntervalTick = 0;
        }
    }

    void SchoolingUpdate()
    {
        PriorityQueue<float, Fish> fishDistHeap = new PriorityQueue<float, Fish>();

        for (int i = 0; i < FishNumber; i++)
        {
            Fish fish = Fishes[i];
            Vector3 grav = Vector3.zero;

            foreach (GameObject food in Foods)
            {
                Vector3 diff = food.transform.position - fish.transform.position;
                grav += FoodGrav * diff / diff.magnitude / diff.magnitude;
            }

            bool nearThreatenings = false;
            foreach (GameObject thre in Threatenings)
            {
                Vector3 diff = thre.transform.position - fish.transform.position;
                if (diff.magnitude < ThreatenDistanceThreshold)
                {
                    nearThreatenings = true;
                }

                grav += ThreateningGrav * diff / diff.magnitude / diff.magnitude;
            }

            for (int j = 0; j < FishNumber; j++)
            {
                if (i == j) continue;
                Vector3 diff = fish.transform.position - Fishes[j].transform.position;
                Vector3 localDiff = fish.transform.InverseTransformVector(diff);
                localDiff.z /= NeighborAxisPushGrav;
                diff = fish.transform.TransformVector(localDiff);
                float dist = (diff).magnitude;
                fishDistHeap.Add(new KeyValuePair<float, Fish>(dist, Fishes[j]));
            }

            for (int m = 0; m < FollowNeighborNumber; m++)
            {
                KeyValuePair<float, Fish> kv = fishDistHeap.Dequeue();
                if (kv.Key > NeighborFollowDistThreshold) continue;
                Vector3 diff = kv.Value.transform.position - fish.transform.position;
                if (diff.magnitude > NeighborDistThreshold_Final)
                {
                    grav += NeighborPullGrav * diff / diff.magnitude / diff.magnitude;
                }
                else
                {
                    grav += NeighborPushGrav * diff / diff.magnitude / diff.magnitude;
                }
            }

            fishDistHeap.Clear();

            float threatenDistanceSpeedUp = nearThreatenings ? ThreatenDistanceSpeedUp : 1f;

            Vector3 localVelocity = fish.transform.InverseTransformVector(grav);
            Vector2 v_xy = new Vector2(localVelocity.x, localVelocity.y);
            v_xy = Vector2.ClampMagnitude(v_xy, MaxVelocity_Side * threatenDistanceSpeedUp);
            localVelocity.z = Mathf.Clamp(localVelocity.z, MaxVelocity_Back * threatenDistanceSpeedUp, MaxVelocity * threatenDistanceSpeedUp);
            localVelocity.x = v_xy.x;
            localVelocity.y = v_xy.y;

            fish.Velocity = Vector3.Lerp(fish.Velocity, fish.transform.TransformVector(localVelocity), Time.deltaTime * 5);
        }

        foreach (KeyValuePair<int, Fish> kv in Fishes)
        {
            Quaternion rot_ori = kv.Value.transform.rotation;
            kv.Value.transform.LookAt(kv.Value.transform.position + kv.Value.Velocity * 10);
            Quaternion rot_tar = kv.Value.transform.rotation;
            kv.Value.transform.rotation = rot_ori;
            kv.Value.transform.rotation = Quaternion.Lerp(rot_ori, rot_tar, Time.deltaTime * 20);

            kv.Value.transform.DOMove(kv.Value.transform.position + kv.Value.Velocity * Time.deltaTime, Time.deltaTime).SetEase(Ease.Linear);
        }
    }
}