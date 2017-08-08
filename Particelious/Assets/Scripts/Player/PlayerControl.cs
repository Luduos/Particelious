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

    [SerializeField] private Text DebugText = null;
    [SerializeField] private AttributeType LeftSliderAttribute = AttributeType.FREQUENCY;
    [SerializeField] private AttributeType RightSliderAttribute = AttributeType.AMPLITUDE;
    [SerializeField] private AttributeType BottomSliderAttribute = AttributeType.SPEED;

    private WaveMovement playerMovement;

	void Start () {
        playerMovement = this.GetComponent<WaveMovement>();
    }

    private void ChangeAttribute(AttributeType type, float value)
    {
        if(null != playerMovement)
        {
            switch (type)
            {
                case AttributeType.FREQUENCY:
                    playerMovement.FrequencyMultiplier = value;
                    break;
                case AttributeType.AMPLITUDE:
                    playerMovement.AmplitudeMultiplier = value;
                    break;
                case AttributeType.SPEED:
                    playerMovement.SpeedMultiplier = value;
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
        LogToDebugText("Left Slider-Value: " + CurrentSliderValue);
    }

    public void OnRightSliderChanged(Slider ChangedSlider)
    {
        float CurrentSliderValue = ChangedSlider.value;
        ChangeAttribute(RightSliderAttribute, CurrentSliderValue);
        LogToDebugText("Right Slider-Value: " + CurrentSliderValue);
    }

    public void OnBottomSliderChanged(Slider ChangedSlider)
    {
        float CurrentSliderValue = ChangedSlider.value;
        ChangeAttribute(BottomSliderAttribute, CurrentSliderValue);
        LogToDebugText("Bottom Slider-Value: " + CurrentSliderValue);
    }

    private void LogToDebugText(string message)
    {
        if (null != DebugText)
        {
            DebugText.text = message;
        }
    }
}