using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Eraser : MonoBehaviour
{
    public Transform[] TargetPositions;

    void Start()
    {
        RandomlyMove();
    }

    void RandomlyMove()
    {
        transform.DOMove(TargetPositions[Random.Range(0, TargetPositions.Length)].position, Random.Range(1, 3f)).OnComplete(RandomlyMove);
    }

    void Update()
    {
    }
}