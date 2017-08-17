using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaveMovement))]
public class WallSpawner : PathFollower {

    public enum SpawnMode{
        WALLS_TOP,
        WALLS_BOTTOM,
        BOTH,
        NONE
    }

    /* public variables, used by procedural level generator */
    [SerializeField]
    public CameraController MainCamera = null;
    [SerializeField]
    private WaveMovement m_PlayerWaveMovement;
    [SerializeField]
    public SpawnMode CurrentSpawnMode = SpawnMode.BOTH;
    [SerializeField]
    public float PathHalfSize = 2.5f;
    [SerializeField]
    public float WallWidth = 2.0f;
    [SerializeField]
    public float WallDistanceMultiplier = 4.0f;
    [SerializeField]
    public Wall WallPrefab = null;
    [SerializeField]
    public int InitialPoolSize = 32;
    [SerializeField]
    public int MaxPoolSize = 256;

    
    public WaveMovement PlayerWaveMovement { get { return m_PlayerWaveMovement; } set { m_PlayerWaveMovement = value; } }

    private Vector2 LastSpawnPosition;
    private static readonly Quaternion s_FlippedQuaternion = Quaternion.Euler(new Vector3(0.0f, 0.0f, 180.0f));
    private static readonly Quaternion s_Identity = Quaternion.identity;

    private float CurrentWallWidth;
    private const float PI_QUARTER = Mathf.PI * 0.25f;

	protected override void Start () {
        base.Start();
        Movement = this.GetComponent<WaveMovement>();
        GetPlayerMovement();

        if (null != StartingNode)
        {
            transform.position = StartingNode.transform.position;
            Movement.CurrentSpeed = m_PlayerWaveMovement.CurrentSpeed;
            Movement.UpdateWaveAttributes(StartingNode.GetComponent<WaveChangeInfo>());
        }
        LastSpawnPosition = this.transform.position;

        Camera cullingCamera = null;
        if(MainCamera == null)
        {
            MainCamera = FindObjectOfType<CameraController>();
        }
        cullingCamera = MainCamera.GetComponent<Camera>();
        
        Wall.s_WallPool = new CullingPooler(WallPrefab, cullingCamera, InitialPoolSize, MaxPoolSize);
        
        CurrentWallWidth = WallWidth / Movement.Frequency;
        UpdateSpawn();
    }

    protected override void FixedUpdate () {
        // Update Speed
        base.FixedUpdate();
        Movement.CurrentSpeed = m_PlayerWaveMovement.CurrentSpeed;
        UpdateSpawn();
    }

    void OnDestroy()
    {
        Wall.s_WallPool.Dispose();
    }

    // Used to check, if we have to spawn the next wall
    private void UpdateSpawn()
    {
        CurrentWallWidth = WallWidth / Movement.Frequency;
        if (transform.position.x > LastSpawnPosition.x + (CurrentWallWidth * WallDistanceMultiplier))
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
        Vector2 HighestViewPoint = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.1f, 0.0f));
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
        Vector2 LowestViewPoint = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, -0.1f, 0.0f));
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

    private void GetPlayerMovement()
    {
        if (null == m_PlayerWaveMovement)
        {
            PlayerController Player = FindObjectOfType<PlayerController>();
            if (null != Player)
            {
                m_PlayerWaveMovement = Player.GetComponent<WaveMovement>();
            }
        }
    } 
}
