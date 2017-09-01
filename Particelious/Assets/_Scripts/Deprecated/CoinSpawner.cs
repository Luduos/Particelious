using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CoinSpawnerInfo
{
    [Range(-1.0f, 1.0f)]
    public float StartProbability;
    [Range(0.0f, 1.0f)]
    public float ProbabilityIncrease;
    [Range(0.1f, 10.0f)]
    public float TimeToProbabilityIncrease;
    [Range(0.0f, 2.0f)]
    public float RandomOffsetAddition;
}

[RequireComponent(typeof(WaveMovement))]
public class CoinSpawner : MonoBehaviour {

    [SerializeField]
    CoinSpawnerInfo SpawnerInfo = new CoinSpawnerInfo();
    [SerializeField]
    public CameraController MainCameraController = null;
    [SerializeField]
    public WaveMovement m_PlayerWaveMovement = null;
    [SerializeField]
    public WallSpawner m_WallsToAlignTo = null;
    [SerializeField]
    public Coin m_CoinPrefab = null;
    [SerializeField]
    public CullingPoolerInfo PoolingInfo = new CullingPoolerInfo(32, 128);

    public WaveMovement PlayerWaveMovement { get { return m_PlayerWaveMovement; } set { m_PlayerWaveMovement = value; } }

    //private static readonly Vector3 m_HighestViewportSpawnPoint = new Vector3(0.0f, 1.1f, 0.0f);
    //private static readonly Vector3 m_LowestViewportSpawnPoint = new Vector3(0.0f, -0.1f, 0.0f);
    private static readonly Quaternion s_Identity = Quaternion.identity;

    private Camera MainCamera = null;
    private float CoinPrefabRadius = 0.5f;
    private float TwoTimesCoinPrefabRadius;
    private Vector2 m_LastSpawnPosition;

    private float m_TimeSinceLastProbabilityTick = 0.0f;
    private float m_CurrentSpawnProbability = 0.0f;

    // Use this for initialization
    void Start () {
        if (MainCameraController == null)
        {
            MainCameraController = FindObjectOfType<CameraController>();
        }
        MainCamera = MainCameraController.GetComponent<Camera>();

        if(null == m_PlayerWaveMovement)
        {
            m_PlayerWaveMovement = HelperFunctions.TryGetPlayerMovement();
        }

        m_LastSpawnPosition = this.transform.position;
        CircleCollider2D coinCollider = m_CoinPrefab.GetComponent<CircleCollider2D>();
        if (coinCollider)
        {
            CoinPrefabRadius = SpawnerInfo.RandomOffsetAddition + coinCollider.radius * m_CoinPrefab.transform.localScale.magnitude;
        }
        TwoTimesCoinPrefabRadius = 2.0f * CoinPrefabRadius;

        Coin.s_CoinPool = new CullingPooler(m_CoinPrefab, MainCamera, PoolingInfo);
    }

    void OnDestroy()
    {
        if(null != Coin.s_CoinPool)
            Coin.s_CoinPool.Dispose();
    }

    // Update is called once per frame
    void Update () {
        m_TimeSinceLastProbabilityTick += Time.deltaTime;
        if(m_TimeSinceLastProbabilityTick > SpawnerInfo.TimeToProbabilityIncrease)
        {
            m_CurrentSpawnProbability += SpawnerInfo.ProbabilityIncrease;
            m_TimeSinceLastProbabilityTick = 0.0f;
        }
        if(m_CurrentSpawnProbability > UnityEngine.Random.value)
        {
            float randomOffset = UnityEngine.Random.Range(-CoinPrefabRadius, CoinPrefabRadius);
            Vector2 spawnPosition = this.transform.position + new Vector3(0.0f, randomOffset, 0.0f);
            if (spawnPosition.x != m_LastSpawnPosition.x)
            {
                Coin.s_CoinPool.Get(spawnPosition, s_Identity, TwoTimesCoinPrefabRadius);
                m_LastSpawnPosition = spawnPosition;
            }
            m_CurrentSpawnProbability = SpawnerInfo.StartProbability;
        }
    }
}
