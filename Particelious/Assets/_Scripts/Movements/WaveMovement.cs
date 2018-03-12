using System;
using UnityEngine;
using UnityEngine.Events;

public class WaveMovement : MonoBehaviour, ISpawnUpdateable {

    [SerializeField]
    private bool m_Vertical = true;
    public bool Vertical{ get { return m_Vertical; } set { m_Vertical = value; } }

    [SerializeField] [Tooltip("Frequency in Hertz")] private float m_Frequency = 0.5f;
    public float Frequency { get { return m_Frequency; } set { m_Frequency = value; m_HasFrequencyChanged = true; } }

    private float m_FrequencyMultiplier = 1.0f;
    public float FrequencyMultiplier { get { return m_FrequencyMultiplier; } set { m_FrequencyMultiplier = value; m_HasFrequencyChanged = true; } }

    [SerializeField] private float m_Amplitude = 2.0f;
    public float Amplitude { get { return m_Amplitude; } set { m_Amplitude = value; OnAmplitudeChanged.Invoke(); } }
    public float AmplitudeMultiplier { get; set; }

    [SerializeField] private float m_CurrentSpeed = 2.0f;
    public float CurrentSpeed { get { return m_CurrentSpeed; } set { m_CurrentSpeed = value; } }
    public float SpeedMultiplier { get; set; }

    public UnityEvent OnAmplitudeChanged;
    public Action OnReachedTopMostPoint;
    public Action OnReachedBottomMostPoint;

    private Vector3 m_OscillationOrigin;
    public Vector3 OscillationOrigin { get { return m_OscillationOrigin; } set { m_OscillationOrigin = value; } }

    private float m_PhaseShift = 0.0f;
    public float PhaseShift { get { return m_PhaseShift; } }

    private static readonly float TWO_PI = Mathf.PI * 2;
    private static readonly float EPSILON = 1e-5f;

    private float m_AccumulatedTime = 0.0f;
    private float m_LastFrequencyCoefficient = 0.0f;
   
    private bool m_HasFrequencyChanged = true;

    private float m_OldYDisplacement = 0.0f;
    private bool m_IsGoingUp = true;
    public bool IsGoingUp { get { return m_IsGoingUp; } }

    // Use this for initialization
    void Start () {
        m_OscillationOrigin = this.transform.position;
        FrequencyMultiplier = 1.0f;
        AmplitudeMultiplier = 1.0f;
        SpeedMultiplier = 1.0f;
    }

    void Update()
    {
        // Y-Movement
        float YDisplacementByFrequency = UpdateFrequency();
        float AmplitudeCoefficient = m_Amplitude * AmplitudeMultiplier;

        float OscillationDisplacementCoefficient = AmplitudeCoefficient * YDisplacementByFrequency;
        Vector3 OscillationDisplacement;
        if (Vertical)
        {
            OscillationDisplacement = new Vector3(0.0f, OscillationDisplacementCoefficient, 0.0f);
        }
        else
        {
            OscillationDisplacement = new Vector3(OscillationDisplacementCoefficient, 0.0f, 0.0f);
        }
        //X-Movement
        float SpeedCoefficient = Time.deltaTime * SpeedMultiplier * CurrentSpeed;
        Vector3 PositionIncrement = new Vector3(SpeedCoefficient, 0.0f, 0.0f);
        // Check if we reached a minimum or maximum
        CheckForExtremum(YDisplacementByFrequency);
        // Apply position changes
        m_OscillationOrigin = new Vector3(m_OscillationOrigin.x, m_OscillationOrigin.y) + PositionIncrement;
        this.transform.position = m_OscillationOrigin + OscillationDisplacement;
        // Update Old Displacement
        m_OldYDisplacement = YDisplacementByFrequency;
    }

    public void UpdateSpawnAttributes(SpawnChangeInfo UpdatedAttributes)
    {
        if(null != UpdatedAttributes)
        {
            Frequency = UpdatedAttributes.NewFrequency;
            Amplitude = UpdatedAttributes.NewAmplitude;
        }
    }

    private float UpdateFrequency()
    {
        m_AccumulatedTime += Time.deltaTime;

        if (m_HasFrequencyChanged)
        {
            float CurrentFrequencyCoefficient = m_Frequency * FrequencyMultiplier;
            float CurrentPhase = (m_AccumulatedTime * m_LastFrequencyCoefficient + m_PhaseShift) % TWO_PI;
            float NextPhase = (m_AccumulatedTime * CurrentFrequencyCoefficient) % TWO_PI;
            m_PhaseShift = CurrentPhase - NextPhase;

            m_LastFrequencyCoefficient = CurrentFrequencyCoefficient;
            m_HasFrequencyChanged = false;
        }
        // Y-Movement increment
        float SineValue = m_AccumulatedTime * m_LastFrequencyCoefficient + m_PhaseShift;
        float YDisplacement = Mathf.Sin(SineValue);

       // TODO: Keep AccumulatedTime from Overspilling?
        return YDisplacement;
    }

    private void CheckForExtremum(float CurrentYDisplacement)
    {
       if(m_IsGoingUp && CurrentYDisplacement < m_OldYDisplacement - EPSILON)
        {
            m_IsGoingUp = false;
            if(null != OnReachedTopMostPoint)
                OnReachedTopMostPoint();
        }else if(!m_IsGoingUp && CurrentYDisplacement > m_OldYDisplacement + EPSILON)
        {
            m_IsGoingUp = true;
            if(null != OnReachedBottomMostPoint)
                OnReachedBottomMostPoint();
        }
    }
}