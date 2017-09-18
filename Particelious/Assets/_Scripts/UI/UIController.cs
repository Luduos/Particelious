using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ImplicatorInformation
{
    public RectTransform UpperImplicator;
    public RectTransform LowerImplicator;

    public Image GoingUpSignal;
    public Image GoingDownSignal;

    public float SignalAlphaMin;
    public float SignalAlphaMax;

    public float FadeDuration;
}

public class UIController : MonoBehaviour{
    [SerializeField]
    private WaveMovement PlayerWaveMovement = null;
    [SerializeField]
    private GameObject PlayerMesh = null;
    [SerializeField]
    private Camera MainCamera = null;

    [SerializeField]
    private ImplicatorInformation m_ImplicatorInfo;

    public System.Action OnTouchDown;
    public System.Action OnTouchUp;

    void Start () {
        this.enabled = false;
        UpdateAmplitudeImplicators();  
    }

    public void UpdateAmplitudeImplicators()
    {
        if (null == PlayerWaveMovement)
        {
            PlayerWaveMovement = HelperFunctions.TryGetPlayerMovement();
        }
        if (PlayerMesh && PlayerWaveMovement && MainCamera)
        {
            float CurrentScale = PlayerMesh.transform.lossyScale.y * 0.5f;

            Vector3 amplitudeOffset = new Vector3(0.0f, PlayerWaveMovement.Amplitude + CurrentScale, 0.0f);
            Vector3 minWorldPointPosition = MainCamera.transform.position - amplitudeOffset;
            Vector3 minScreenPointPosition = MainCamera.WorldToScreenPoint(minWorldPointPosition);
            float amplitudeImplicatorWidth = minScreenPointPosition.y;

            m_ImplicatorInfo.UpperImplicator.sizeDelta = new Vector2(0.0f, amplitudeImplicatorWidth);
            m_ImplicatorInfo.LowerImplicator.sizeDelta = new Vector2(0.0f, amplitudeImplicatorWidth);
        }
        else
        {
            Debug.Log("No PlayerMesh / PlayerMovement / MainCamera defined in AmplitudeImplicator");
        }

        if (null != m_ImplicatorInfo.GoingUpSignal)
        {
            PlayerWaveMovement.OnReachedTopMostPoint += OnShowGoingDownSignal;
        }
        if (null != m_ImplicatorInfo.GoingDownSignal)
        {
            PlayerWaveMovement.OnReachedBottomMostPoint += OnShowGoingUpSignal;
        }
        m_ImplicatorInfo.GoingUpSignal.CrossFadeAlpha(m_ImplicatorInfo.SignalAlphaMax, 0.0f, false);
        m_ImplicatorInfo.GoingDownSignal.CrossFadeAlpha(m_ImplicatorInfo.SignalAlphaMin, 0.0f, false);
    }

    private void OnShowGoingUpSignal()
    {
        m_ImplicatorInfo.GoingUpSignal.CrossFadeAlpha(m_ImplicatorInfo.SignalAlphaMax, m_ImplicatorInfo.FadeDuration, false);
        m_ImplicatorInfo.GoingDownSignal.CrossFadeAlpha(m_ImplicatorInfo.SignalAlphaMin, m_ImplicatorInfo.FadeDuration, false);
    }

    private void OnShowGoingDownSignal()
    {
        m_ImplicatorInfo.GoingUpSignal.CrossFadeAlpha(m_ImplicatorInfo.SignalAlphaMin, m_ImplicatorInfo.FadeDuration, false);
        m_ImplicatorInfo.GoingDownSignal.CrossFadeAlpha(m_ImplicatorInfo.SignalAlphaMax, m_ImplicatorInfo.FadeDuration, false);
    }
}