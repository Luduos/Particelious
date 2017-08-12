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
    public SpawnMode CurrentSpawnMode = SpawnMode.BOTH;
    [SerializeField]
    public float PathHalfSize = 2.5f;
    [SerializeField]
    public float WallWidth = 2.0f;
    [SerializeField]
    public Wall WallPrefab = null;
    [SerializeField]
    public int PoolSize = 128;

    [SerializeField]
    private WaveMovement PlayerWaveMovement;

    private Vector2 LastSpawnPosition;

	protected override void Start () {
        base.Start();
        Movement = this.GetComponent<WaveMovement>();
        GetPlayerMovement();

        if (null != StartingNode)
        {
            this.transform.position = StartingNode.transform.position;
            Movement.CurrentSpeed = PlayerWaveMovement.CurrentSpeed;
            Movement.UpdateWaveAttributes(StartingNode.GetComponent<WaveChangeInfo>());
        }
        LastSpawnPosition = this.transform.position;

        Wall.s_WallPool = new Pooler(WallPrefab.gameObject, PoolSize);
        UpdateSpawn();
    }

    protected override void FixedUpdate () {
        // Update Speed
        base.FixedUpdate();
        Movement.CurrentSpeed = PlayerWaveMovement.CurrentSpeed;
        UpdateSpawn();
    }

    // Used to check, if we have to spawn the next wall
    private void UpdateSpawn()
    {
        if(this.transform.position.x > LastSpawnPosition.x + WallWidth)
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
                        LastSpawnPosition = this.transform.position - new Vector3(0.0f, -WallWidth, 0.0f);
                        break;
                    }
            }
        }  
    }

    private Vector3 SpawnTopWall()
    {
        Vector2 LowestWallPoint = new Vector2(LastSpawnPosition.x + WallWidth, this.transform.position.y + PathHalfSize);
        Vector2 HighestViewPoint = Camera.main.ViewportToWorldPoint(Vector3.one);
        float DistanceToTop = HighestViewPoint.y - LowestWallPoint.y;

        Vector3 SpawnPosition = LowestWallPoint + new Vector2(0.0f, DistanceToTop * 0.5f);
        if(DistanceToTop > 0)
        {
            SpawnWall(SpawnPosition, DistanceToTop);
        }
        return SpawnPosition;
    }

    private Vector3 SpawnBottomWall()
    {
        Vector2 HighestWallPoint = new Vector2(LastSpawnPosition.x + WallWidth, this.transform.position.y - PathHalfSize);
        Vector2 LowestViewPoint = Camera.main.ViewportToWorldPoint(Vector3.zero);
        float DistanceToBottom = HighestWallPoint.y - LowestViewPoint.y;

        Vector3 SpawnPosition = HighestWallPoint - new Vector2(0.0f, DistanceToBottom * 0.5f);
        if (DistanceToBottom > 0)
        {
            SpawnWall(SpawnPosition, DistanceToBottom);
        }
        return SpawnPosition;
    }

    private void SpawnWall(Vector3 SpawnPosition, float Height)
    {
        GameObject createdWall = Wall.s_WallPool.Get(SpawnPosition, this.transform.rotation);
        createdWall.transform.localScale = new Vector3(WallWidth, Height, createdWall.transform.localScale.z);
        if (!createdWall.activeSelf)
        {
            Debug.Log("Inactive created Wall.", this);
        }
    }

    private void GetPlayerMovement()
    {
        if (null == PlayerWaveMovement)
        {
            GameObject Player = HelperFunctions.TryToFindPlayer();
            if (null != Player)
            {
                PlayerWaveMovement = Player.GetComponent<WaveMovement>();
            }
        }
    } 
}
