using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour {

    [SerializeField]
    public CameraController MainCameraController = null;
    private Camera MainCamera = null;
    [SerializeField]
    public WaveMovement m_PlayerWaveMovement;

    [SerializeField]
    public Coin m_CoinPrefab = null;

    [SerializeField]
    public int InitialPoolSize = 32;
    [SerializeField]
    public int MaxPoolSize = 256;

    public WaveMovement PlayerWaveMovement { get { return m_PlayerWaveMovement; } set { m_PlayerWaveMovement = value; } }

    private Vector2 LastSpawnPosition;

    private static readonly Vector3 m_HighestViewportSpawnPoint = new Vector3(0.0f, 1.1f, 0.0f);
    private static readonly Vector3 m_LowestViewportSpawnPoint = new Vector3(0.0f, -0.1f, 0.0f);

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
