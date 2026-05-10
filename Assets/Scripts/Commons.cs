using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct MinMaxRange
{
    public float Min;
    public float Max;

    public float GetRandomRange()
    {
        return Random.Range(Min, Max);
    }
}