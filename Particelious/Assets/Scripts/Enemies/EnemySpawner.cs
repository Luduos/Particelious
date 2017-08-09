using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaveMovement))]
public class EnemySpawner : MonoBehaviour {

    [SerializeField] private GameObject EnemyType;
    [SerializeField] private WaveMovement PlayerWaveMovement;
    [SerializeField] private Vector3 DistanceFromPlayer = new Vector3(20.0f, 0.0f, 0.0f);
    [SerializeField] private float MinSpawnTime = 0.5f;
    [SerializeField] private float MaxSpawnTime = 1.5f;
    [SerializeField] private int RandomSeed = 0;


    private WaveMovement SpawnerMovement; // TODO: Generalize this to make any movement-patterns possible.
    private float NextSpawnTime;
    private float AccumulatedTime;

	// Use this for initialization
	void Start () {
        Random.InitState(RandomSeed);

        NextSpawnTime = Random.Range(MinSpawnTime, MaxSpawnTime);

        SpawnerMovement = this.GetComponent<WaveMovement>();
    }

    // Update is called once per frame
    void Update () {
        // Update Position
        SpawnerMovement.OscillationOrigin = PlayerWaveMovement.OscillationOrigin + DistanceFromPlayer;

        // Potentially spawn enemy
        AccumulatedTime += Time.deltaTime;
        if (AccumulatedTime > NextSpawnTime)
        {
            GameObject newEnemy = Instantiate(EnemyType, this.transform.position, this.transform.rotation);
            Destroy(newEnemy, 30.0f);
            AccumulatedTime = 0.0f;
            NextSpawnTime = Random.Range(MinSpawnTime, MaxSpawnTime);
        }

    }
}
