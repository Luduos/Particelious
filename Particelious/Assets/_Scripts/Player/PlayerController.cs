using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DigitalRubyShared;
[RequireComponent(typeof(WaveMovement), typeof(FingersScript))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float m_InterpolationMultiplierUp = 1.5f;
    [SerializeField]
    private float m_InterpolationMultiplierDown = 3.0f;

    private float m_CurrentInterpolationMultiplier = 1.0f;

    private LongPressGestureRecognizer tapGesture;

    private WaveMovement m_PlayerMovement;
    private bool m_PlayerControlEnabled = true;

    private float m_CurrentValue = 0.0f;
    private float m_GoalValue = 0.0f;

	void Start () {
        //this.enabled = false; // no need to update
        m_PlayerMovement = this.GetComponent<WaveMovement>();

        //m_InterPolationCoefficient = 1.0f / (m_InputMaxValue - m_InputMinValue);
        CreateTapGestures();
        m_CurrentInterpolationMultiplier = m_InterpolationMultiplierUp;
    }

    private void CreateTapGestures()
    {
        tapGesture = new LongPressGestureRecognizer();
        tapGesture.MinimumDurationSeconds = 0.0f;
        tapGesture.Updated += TapGestureCallback;
        FingersScript.Instance.AddGesture(tapGesture); 
    }

    private void TapGestureCallback(GestureRecognizer gesture, ICollection<GestureTouch> touches)
    {
        if(touches.Count > 0)
        {
            OnTouchDown();
        }
        else
        {
            OnTouchUp();
        }
    }

    void Update()
    {
        if (m_PlayerControlEnabled)
        {
            m_CurrentValue = Mathf.Lerp(m_CurrentValue, m_GoalValue, Time.deltaTime * m_CurrentInterpolationMultiplier);
            //m_CurrentValue = m_GoalValue;
            m_PlayerMovement.FrequencyMultiplier = m_CurrentValue;
        }
    }

    public void OnTouchDown()
    {
        m_GoalValue = 1.0f;
        m_CurrentInterpolationMultiplier = m_InterpolationMultiplierUp;
    }

    public void OnTouchUp()
    {
        m_GoalValue = 0.0f;
        m_CurrentInterpolationMultiplier = m_InterpolationMultiplierDown;
    }

    private void ChangeAttribute(float value)
    {
        if(m_PlayerControlEnabled)
        {
            /*
            if (value < m_InputMinValue)
            {
                value = 0.0f;
            }
            else if (value > m_InputMaxValue)
            {
                value = 1.0f;
            }
            else
            {
                value -= m_InputMinValue;
                value *= m_InterPolationCoefficient;
            }*/
            m_PlayerMovement.FrequencyMultiplier = value;             
        }
    }

    public void OnSliderChanged(Slider ChangedSlider)
    {
        /*
        float CurrentSliderValue = ChangedSlider.value;
        ChangeAttribute(CurrentSliderValue);
        */
    }

    public void SetPlayerCurrentSpeed(float NewPlayerSpeed)
    {
        m_PlayerMovement.CurrentSpeed = NewPlayerSpeed;
    }

    public void SetPlayerFrequency(float NewPlayerFrequency)
    {
        m_PlayerMovement.Frequency = NewPlayerFrequency;
    }

    public void SetPlayerControlEnabled(bool enabled)
    {
        m_PlayerControlEnabled = enabled;
    }
}