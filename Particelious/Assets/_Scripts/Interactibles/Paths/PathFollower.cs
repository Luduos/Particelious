using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(WaveMovement))]
public class PathFollower : MonoBehaviour {

    [SerializeField]
    public PathNode StartingNode = null;

    public System.Action<PathNode> OnReachedNode;
    public UnityEvent OnPassedLastNode;

    protected PathNode NextNode;
    protected WaveMovement Movement;

    private bool m_IsFollowing = true;
    protected bool IsFollowing { get { return m_IsFollowing; } }

    // Use this for initialization
    public virtual void Start () {
        NextNode = StartingNode;
        m_IsFollowing = true;
       
        if (null != StartingNode)
            transform.position = StartingNode.transform.position;
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
            OnReachedNode.Invoke(CurrentNode);
            if(null == NextNode)
            {
                OnPassedLastNode.Invoke();
            }
        }
    }
}
