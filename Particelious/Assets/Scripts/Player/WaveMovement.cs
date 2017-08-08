using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMovement : MonoBehaviour {

    [SerializeField] [Tooltip("Frequency in Hertz")] private float m_Frequency = 0.5f;
    public float Frequency { get { return m_Frequency; } set { m_Frequency = value; } }
    public float FrequencyMultiplier { get; set; }

    [SerializeField] private float m_Amplitude = 2.0f;
    public float Amplitude { get { return m_Amplitude; } set { m_Amplitude = value; } }
    public float AmplitudeMultiplier { get; set; }

    [SerializeField] private float m_CurrentSpeed = 2.0f;
    public float CurrentSpeed { get { return m_CurrentSpeed; } set { m_CurrentSpeed = value; } }
    public float SpeedMultiplier { get; set; }

    private Vector3 m_OscillationOrigin;
    public Vector3 OscillationOrigin { get { return m_OscillationOrigin; } set { m_OscillationOrigin = value; } }

    private const float TWO_PI = Mathf.PI * 2;
    private Transform ObjectTransform;

    private float AccumulatedTime = 0.0f;
    private float YDisplacementByFrequency = 0.0f;

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

        UpdateFrequency();
        float AmplitudeCoefficient = m_Amplitude * AmplitudeMultiplier;
        Vector3 OscillationDisplacement = new Vector3(0.0f, AmplitudeCoefficient * YDisplacementByFrequency, 0.0f);
        Debug.Log(YDisplacementByFrequency);
        //X-Movement
        float SpeedCoefficient = Time.deltaTime * SpeedMultiplier * CurrentSpeed;
        Vector3 PositionIncrement = new Vector3(SpeedCoefficient, 0.0f, 0.0f);

        // Apply position changes
        m_OscillationOrigin += PositionIncrement;
        ObjectTransform.position = m_OscillationOrigin + OscillationDisplacement;

        
    }

    private void UpdateFrequency()
    {
        // TODO: Idee: Alle 0.05 Sekunden den nächsten Punkt (+ 0.05 Sekunden) auf der Sinus-Kurve berechnen und während AccumulatedTime < 0.05 ist, dorthin interpolieren.
        // Nach den 0.05 Sekunden wird mit dem neuesten FrequencyMultiplier der nächste Punkt berechnet.
        AccumulatedTime += Time.deltaTime;
        // Y-Movement increment
        float frequencyComponent = TWO_PI *  m_Frequency * AccumulatedTime;
        YDisplacementByFrequency = FrequencyMultiplier * Mathf.Cos(frequencyComponent);

        // keep AccumulatedTime from overspilling
        if ((m_Frequency * AccumulatedTime) > 1)
        {
            AccumulatedTime = 0;
        }
    }
}
