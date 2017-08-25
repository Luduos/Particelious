using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChangeInfo : MonoBehaviour {
    public float NewFrequency = 1.0f;
    public float NewAmplitude = 6.0f;

    public float Wall_PathHalfHeight = 6.0f;
    public WallSpawner.SpawnMode Wall_SpawnMode = WallSpawner.SpawnMode.BOTH;
}