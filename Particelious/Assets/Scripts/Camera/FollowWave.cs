using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWave : MonoBehaviour {

    [SerializeField] private WaveMovement m_FollowTarget = null;

    [SerializeField] private Vector3 m_CameraDistance = new Vector3(0.0f, 0.0f, -10.0f);
    public Vector3 CameraDistance { get { return m_CameraDistance; } set { m_CameraDistance = value; } }

    private Transform FollowerTransform;
	// Use this for initialization
	void Start () {
        FollowerTransform = this.GetComponent<Transform>();
    }

	void FixedUpdate () {
        FollowerTransform.position = m_FollowTarget.OscillationOrigin + m_CameraDistance;
    }
}
