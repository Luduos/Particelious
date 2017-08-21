using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaveMovement))]
public class WallSpawner : MonoBehaviour{

    public enum SpawnMode{
        WALLS_TOP,
        WALLS_BOTTOM,
        BOTH,
        NONE
    }

    /* public variables, used by procedural level generator */
    [SerializeField]
    public CameraController MainCameraController = null;
    private Camera MainCamera = null;
    [SerializeField]
    public WaveMovement m_PlayerWaveMovement;
    [SerializeField]
    public SpawnMode CurrentSpawnMode = SpawnMode.BOTH;
    [SerializeField]
    public float PathHalfSize = 2.5f;
    [SerializeField]
    public float WallWidthMultiplier = 2.0f;
    [SerializeField]
    public float WallDistanceMultiplier = 4.0f;
    [SerializeField]
    public float AdditionalFixedWallDistance = 0.0f;
    [SerializeField]
    public Wall WallPrefab = null;
    [SerializeField]
    public int InitialPoolSize = 32;
    [SerializeField]
    public int MaxPoolSize = 256;

    private WaveMovement m_SpawnerWaveMovement = null;
    public WaveMovement PlayerWaveMovement { get { return m_PlayerWaveMovement; } set { m_PlayerWaveMovement = value; } }

    private Vector2 LastSpawnPosition;
    private static readonly Quaternion s_FlippedQuaternion = Quaternion.Euler(new Vector3(0.0f, 0.0f, 180.0f));
    private static readonly Quaternion s_Identity = Quaternion.identity;

    private float CurrentWallWidth;
    private const float PI_QUARTER = Mathf.PI * 0.25f;
    private static readonly Vector3 m_HighestViewportSpawnPoint = new Vector3(0.0f, 1.1f, 0.0f);
    private static readonly Vector3 m_LowestViewportSpawnPoint = new Vector3(0.0f, -0.1f, 0.0f);

    protected void Start () {
        m_PlayerWaveMovement = HelperFunctions.TryGetPlayerMovement();
        LastSpawnPosition = this.transform.position;
        m_SpawnerWaveMovement = GetComponent<WaveMovement>();

        PathFollower pathFollower = GetComponent<PathFollower>();
        if(null != pathFollower)
        {
            pathFollower.FollowTarget = m_PlayerWaveMovement;
        }

        if (MainCameraController == null)
        {
            MainCameraController = FindObjectOfType<CameraController>();
        }
        MainCamera = MainCameraController.GetComponent<Camera>();
        
        Wall.s_WallPool = new CullingPooler(WallPrefab, MainCamera, InitialPoolSize, MaxPoolSize);
        
        CurrentWallWidth = WallWidthMultiplier / m_SpawnerWaveMovement.Frequency;
        UpdateSpawn();
    }

    protected void FixedUpdate () {
        // Update Speed
        UpdateSpawn();
    }

    void OnDestroy()
    {
        Wall.s_WallPool.Dispose();
    }

    // Used to check, if we have to spawn the next wall
    private void UpdateSpawn()
    {
        CurrentWallWidth = WallWidthMultiplier / m_SpawnerWaveMovement.Frequency;
        if (transform.position.x > LastSpawnPosition.x + AdditionalFixedWallDistance + (CurrentWallWidth * WallDistanceMultiplier))
        {   
            switch (CurrentSpawnMode)
            {
                case SpawnMode.BOTH:
                    {
                        SpawnTopWall();
                        LastSpawnPosition = SpawnBottomWall();
                        break;
                    }
                case SpawnMode.WALLS_TOP:
                    {
                        LastSpawnPosition = SpawnTopWall();
                        break;
                    }
                case SpawnMode.WALLS_BOTTOM:
                    {
                        LastSpawnPosition = SpawnBottomWall();
                        break;
                    }
                case SpawnMode.NONE:
                    {
                        LastSpawnPosition = this.transform.position - new Vector3(0.0f, -CurrentWallWidth, 0.0f);
                        break;
                    }
            }
        }  
    }

    private Vector3 SpawnTopWall()
    {
        Vector2 LowestWallPoint = new Vector2(this.transform.position.x, this.transform.position.y + PathHalfSize);
        Vector2 HighestViewPoint = MainCamera.ViewportToWorldPoint(m_HighestViewportSpawnPoint);
        float DistanceToTop = HighestViewPoint.y - LowestWallPoint.y;

        Vector3 SpawnPosition = LowestWallPoint + new Vector2(0.0f, DistanceToTop * 0.5f);
        if(DistanceToTop > 0)
        {
            SpawnWall(SpawnPosition, DistanceToTop, true);
        }
        return SpawnPosition;
    }

    private Vector3 SpawnBottomWall()
    {
        Vector2 HighestWallPoint = new Vector2(this.transform.position.x, this.transform.position.y - PathHalfSize);
        Vector2 LowestViewPoint = MainCamera.ViewportToWorldPoint(m_LowestViewportSpawnPoint);
        float DistanceToBottom = HighestWallPoint.y - LowestViewPoint.y;

        Vector3 SpawnPosition = HighestWallPoint - new Vector2(0.0f, DistanceToBottom * 0.5f);
        if (DistanceToBottom > 0)
        {
            SpawnWall(SpawnPosition, DistanceToBottom, false);
        }
        return SpawnPosition;
    }

    private void SpawnWall(Vector3 SpawnPosition, float Height, bool flipped)
    {
        Quaternion rotation = flipped ? s_FlippedQuaternion : s_Identity;

        GameObject createdWall = Wall.s_WallPool.Get(SpawnPosition, rotation, Height);
       
        Wall wall = createdWall.GetComponent<Wall>();
        if (wall)
        {
            Vector2 NewSize = new Vector2(CurrentWallWidth, Height);
            wall.SetSize(NewSize);
        }else
        {
            Debug.LogError("Didn't get wall.");
        }
    }
}
