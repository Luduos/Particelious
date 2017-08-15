using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(WaveMovement))]
public class PlayerControl : MonoBehaviour {

    public enum AttributeType
    {
        FREQUENCY,
        AMPLITUDE,
        SPEED
    }

    [SerializeField] private AttributeType LeftSliderAttribute = AttributeType.FREQUENCY;
    [SerializeField] private AttributeType RightSliderAttribute = AttributeType.AMPLITUDE;
    [SerializeField] private AttributeType BottomSliderAttribute = AttributeType.SPEED;

    private WaveMovement m_PlayerMovement;
    private bool m_PlayerControlEnabled = true;

	void Start () {
        this.enabled = false; // no need to update
        m_PlayerMovement = this.GetComponent<WaveMovement>();
    }

    private void ChangeAttribute(AttributeType type, float value)
    {
        if(m_PlayerControlEnabled)
        {
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