using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnUpdateable
{
    void UpdateSpawnAttributes(SpawnChangeInfo UpdatedInfo);
}

[RequireComponent(typeof(PathFollower), typeof(WaveMovement))]
public class SpawnController : MonoBehaviour {

    [SerializeField]
    public WaveMovement FollowTarget = null;
    [SerializeField]
    public float m_DistanceToTarget = 15.0f;

    private List<ISpawnUpdateable> m_ToUpdateOnReachedPathNode = new List<ISpawnUpdateable>();

    private PathFollower m_PathFollower = null;
    private WaveMovement m_WaveMovement = null;
    // Use this for initialization
    void Start () {
        if(null == FollowTarget)
        {
            FollowTarget = HelperFunctions.TryGetPlayerMovement();
        }
        m_PathFollower = GetComponent <PathFollower>();
        m_PathFollower.OnReachedNode += OnReachedPathNode;

        m_WaveMovement = GetComponent<WaveMovement>();
        m_ToUpdateOnReachedPathNode.Add(m_WaveMovement);

        WallSpawner wallSpawner = GetComponent<WallSpawner>();
        if (wallSpawner)
        {
            m_ToUpdateOnReachedPathNode.Add(wallSpawner);
        }
    }

    private void Update()
    {
        if (null != FollowTarget)
            m_WaveMovement.CurrentSpeed = FollowTarget.CurrentSpeed;
    }

    void OnReachedPathNode(PathNode pathNode)
    {
        SpawnChangeInfo changedInfo = pathNode.GetComponent<SpawnChangeInfo>();
        if(null != changedInfo)
        {
            foreach (ISpawnUpdateable current in m_ToUpdateOnReachedPathNode)
            {
                if(null != current)
                {
                    current.UpdateSpawnAttributes(changedInfo);
                }
                else
                {
                    m_ToUpdateOnReachedPathNode.Remove(current);
                }
            }
        }
    }
}
