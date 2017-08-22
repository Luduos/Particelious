using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaveMovement))]
public class CoinSpawner : MonoBehaviour {
    
    [SerializeField]
    public CameraController MainCameraController = null;
    [SerializeField]
    public WaveMovement m_PlayerWaveMovement = null;
    [SerializeField]
    public WallSpawner m_WallsToAlignTo = null;
    [SerializeField]
    public Coin m_CoinPrefab = null;
    [SerializeField]
    public int InitialPoolSize = 32;
    [SerializeField]
    public int MaxPoolSize = 256;

    public WaveMovement PlayerWaveMovement { get { return m_PlayerWaveMovement; } set { m_PlayerWaveMovement = value; } }

    private Camera MainCamera = null;
    private Vector2 LastSpawnPosition;

    private static readonly Vector3 m_HighestViewportSpawnPoint = new Vector3(0.0f, 1.1f, 0.0f);
    private static readonly Vector3 m_LowestViewportSpawnPoint = new Vector3(0.0f, -0.1f, 0.0f);

    // Use this for initialization
    void Start () {
        if (MainCameraController == null)
        {
            MainCameraController = FindObjectOfType<CameraController>();
        }
        MainCamera = MainCameraController.GetComponent<Camera>();

        m_PlayerWaveMovement = HelperFunctions.TryGetPlayerMovement();
        LastSpawnPosition = this.transform.position;

        Coin.s_CoinPool = new CullingPooler(m_CoinPrefab, MainCamera, InitialPoolSize, MaxPoolSize);
    }

    void OnDestroy()
    {
        Coin.s_CoinPool.Dispose();
    }

    // Update is called once per frame
    void Update () {
		
	} 
}
