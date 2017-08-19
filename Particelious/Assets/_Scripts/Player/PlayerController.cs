using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(WaveMovement))]
public class PlayerController : MonoBehaviour {

    public enum AttributeType
    {
        FREQUENCY,
        AMPLITUDE,
        SPEED
    }

    [SerializeField] private AttributeType LeftSliderAttribute = AttributeType.FREQUENCY;
    [SerializeField] private AttributeType RightSliderAttribute = AttributeType.AMPLITUDE;
    [SerializeField] private AttributeType BottomSliderAttribute = AttributeType.SPEED;

    [SerializeField]
    private float m_InputMinValue = 0.2f;
    [SerializeField]
    private float m_InputMaxValue = 0.8f;
    private float m_InterPolationCoefficient;

    private WaveMovement m_PlayerMovement;
    private bool m_PlayerControlEnabled = true;

	void Start () {
        this.enabled = false; // no need to update
        m_PlayerMovement = this.GetComponent<WaveMovement>();

        m_InterPolationCoefficient = 1.0f / (m_InputMaxValue - m_InputMinValue);
    }

    private void ChangeAttribute(AttributeType type, float value)
    {
        if(m_PlayerControlEnabled)
        {
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
            }
            switch (type)
            {
                case AttributeType.FREQUENCY:
                    m_PlayerMovement.FrequencyMultiplier = value;
                    break;
                case AttributeType.AMPLITUDE:
                    m_PlayerMovement.AmplitudeMultiplier = value;
                    break;
                case AttributeType.SPEED:
                    m_PlayerMovement.SpeedMultiplier = value;
                    break;
                default:
                    Debug.LogError("Invalid AttributeType", this);
                    break;
            }
        }
    }

    public void OnLeftSliderChanged(Slider ChangedSlider)
    {
        float CurrentSliderValue = ChangedSlider.value;
        ChangeAttribute(LeftSliderAttribute, CurrentSliderValue);
    }

    public void OnRightSliderChanged(Slider ChangedSlider)
    {
        float CurrentSliderValue = ChangedSlider.value;
        ChangeAttribute(RightSliderAttribute, CurrentSliderValue);
    }

    public void OnBottomSliderChanged(Slider ChangedSlider)
    {
        float CurrentSliderValue = ChangedSlider.value;
        ChangeAttribute(BottomSliderAttribute, CurrentSliderValue);
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