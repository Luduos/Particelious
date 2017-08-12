using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaveMovement))]
public abstract class PathFollower : MonoBehaviour {

    [SerializeField]
    public PathNode StartingNode = null;

    protected PathNode NextNode;
    protected WaveMovement Movement;

    private bool m_IsFollowing = true;
    protected bool IsFollowing { get { return m_IsFollowing; } }

    // Use this for initialization
    protected virtual void Start () {
        NextNode = StartingNode;
        m_IsFollowing = true;
        Movement = this.GetComponent<WaveMovement>();
        Movement.UpdateWaveAttributes(StartingNode.GetComponent<WaveChangeInfo>());

    }

    protected virtual void FixedUpdate()
    {
        if (null != NextNode)
        {
            CheckPath();
        }
    }

    private void CheckPath()
    {
        // Check if we have passed the current node
        if (NextNode.transform.position.x < this.transform.position.x)
        {
            PathNode CurrentNode = NextNode;
            NextNode = NextNode.Child;
            if (null != CurrentNode)
            {
                Movement.UpdateWaveAttributes(CurrentNode.GetComponent<WaveChangeInfo>());
            }else
            {
                m_IsFollowing = false;
            }
            Destroy(CurrentNode.gameObject);
        }
    }
}
