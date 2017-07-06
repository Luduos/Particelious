using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMovement : MonoBehaviour {

    [SerializeField] [Tooltip("Frequency in Hertz")] private float m_Frequency = 0.5f;
    public float Frequency { get { return m_Frequency; } set { m_Frequency = value; } }

    [SerializeField] private float m_Amplitude = 2.0f;
    public float Amplitude { get { return m_Amplitude; } set { m_Amplitude = value; } }

    [SerializeField] private float m_CurrentSpeed = 2.0f;
    public float CurrentSpeed { get { return m_CurrentSpeed; } set { m_CurrentSpeed = value; } }

    private Vector3 m_OscillationOrigin;
    public Vector3 OscillationOrigin { get { return m_OscillationOrigin; } set { m_OscillationOrigin = value; } }

    private const float TWO_PI = Mathf.PI * 2;
    private Transform ObjectTransform;

    private float AccumulatedTime { get; set; }

	// Use this for initialization
	void Start () {
        ObjectTransform = GetComponent<Transform>();
        m_OscillationOrigin = ObjectTransform.position;

    }

    void FixedUpdate()
    { 
        AccumulatedTime += Time.deltaTime;

        // Y-Movement
        float YDisplacement = Mathf.Sin(TWO_PI * m_Frequency * AccumulatedTime);
        Vector3 OscillationDisplacement = m_Amplitude * new Vector3(0.0f, YDisplacement, 0.0f);

        //X-Movement
        Vector3 PositionIncrement = Time.deltaTime * new Vector3(CurrentSpeed, 0.0f, 0.0f);

        // Apply position changes
        m_OscillationOrigin += PositionIncrement;
        ObjectTransform.position = m_OscillationOrigin + OscillationDisplacement;

        // keep AccumulatedTime from overspilling
        while (AccumulatedTime > 1)
        {
            AccumulatedTime -= 1;
        }
    }
}
