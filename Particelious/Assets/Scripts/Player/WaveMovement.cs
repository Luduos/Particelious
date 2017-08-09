using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMovement : MonoBehaviour {

    [SerializeField] [Tooltip("Frequency in Hertz")] private float m_Frequency = 0.5f;
    public float Frequency { get { return m_Frequency; } set { m_Frequency = value; HasFrequencyChanged = true; } }

    private float m_FrequencyMultiplier = 1.0f;
    public float FrequencyMultiplier { get { return m_FrequencyMultiplier; } set { m_FrequencyMultiplier = value; HasFrequencyChanged = true; } }

    [SerializeField] private float m_Amplitude = 2.0f;
    public float Amplitude { get { return m_Amplitude; } set { m_Amplitude = value; } }
    public float AmplitudeMultiplier { get; set; }

    [SerializeField] private float m_CurrentSpeed = 2.0f;
    public float CurrentSpeed { get { return m_CurrentSpeed; } set { m_CurrentSpeed = value; } }
    public float SpeedMultiplier { get; set; }

    private Vector3 m_OscillationOrigin;
    public Vector3 OscillationOrigin { get { return m_OscillationOrigin; } set { m_OscillationOrigin = value; } }

    private const float TWO_PI = Mathf.PI * 2;
    private const float EPSILON = 1e-5f;
    private Transform ObjectTransform;

    private float AccumulatedTime = 0.0f;
    private float LastFrequencyCoefficient = 0.0f;
    private float PhaseShift = 0.0f;
    private bool HasFrequencyChanged = true;

	// Use this for initialization
	void Start () {
        ObjectTransform = GetComponent<Transform>();
        m_OscillationOrigin = ObjectTransform.position;
        FrequencyMultiplier = 1.0f;
        AmplitudeMultiplier = 1.0f;
        SpeedMultiplier = 1.0f;
    }

    void Update()
    {
        // Y-Movement
        float YDisplacementByFrequency = UpdateFrequency();
        float AmplitudeCoefficient = m_Amplitude * AmplitudeMultiplier;
        Vector3 OscillationDisplacement = new Vector3(0.0f, AmplitudeCoefficient * YDisplacementByFrequency, 0.0f);
        //X-Movement
        float SpeedCoefficient = Time.deltaTime * SpeedMultiplier * CurrentSpeed;
        Vector3 PositionIncrement = new Vector3(SpeedCoefficient, 0.0f, 0.0f);

        // Apply position changes
        m_OscillationOrigin += PositionIncrement;
        ObjectTransform.position = m_OscillationOrigin + OscillationDisplacement;  
    }

    private float UpdateFrequency()
    {
        AccumulatedTime += Time.deltaTime;

        if (HasFrequencyChanged)
        {
            float CurrentFrequencyCoefficient = m_Frequency * FrequencyMultiplier;
            float CurrentPhase = (AccumulatedTime * LastFrequencyCoefficient + PhaseShift) % TWO_PI;
            float NextPhase = (AccumulatedTime * CurrentFrequencyCoefficient) % TWO_PI;
            PhaseShift = CurrentPhase - NextPhase;

            LastFrequencyCoefficient = CurrentFrequencyCoefficient;
            HasFrequencyChanged = false;
        }
        // Y-Movement increment
        float SineValue = AccumulatedTime * LastFrequencyCoefficient + PhaseShift;
        float YDisplacement = Mathf.Sin(SineValue);

       // TODO: Keep AccumulatedTime from Overspilling?
        return YDisplacement;
    }
}
