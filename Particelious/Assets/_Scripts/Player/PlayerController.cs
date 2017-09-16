using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
[RequireComponent(typeof(WaveMovement))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float m_InterpolationMultiplierUp = 1.5f;
    [SerializeField]
    private float m_InterpolationMultiplierDown = 3.0f;

    private float m_CurrentInterpolationMultiplier = 1.0f;

    private WaveMovement m_PlayerMovement;
    private bool m_PlayerControlEnabled = true;

    private float m_CurrentValue = 0.0f;
    private float m_GoalValue = 0.0f;

	void Start () {
        //this.enabled = false; // no need to update
        m_PlayerMovement = this.GetComponent<WaveMovement>();

        //m_InterPolationCoefficient = 1.0f / (m_InputMaxValue - m_InputMinValue);
        
        m_CurrentInterpolationMultiplier = m_InterpolationMultiplierUp;
    }

    void Update()
    {
        UpdateInput();
        m_CurrentValue = Mathf.Lerp(m_CurrentValue, m_GoalValue, Time.deltaTime * m_CurrentInterpolationMultiplier);
        m_PlayerMovement.FrequencyMultiplier = m_CurrentValue;   
    }

    private void UpdateInput()
    {
        bool shouldMove = false;

#if UNITY_ANDROID || UNITY_IOS
        shouldMove = Input.touchCount > 0;
#endif
#if UNITY_STANDALONE || UNITY_EDITOR
        shouldMove = Input.anyKey;
#endif
        if (shouldMove)
        {
            OnTouchDown();
        }
        else
        {
            OnTouchUp();
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