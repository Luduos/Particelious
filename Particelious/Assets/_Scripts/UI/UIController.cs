using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour{
    [SerializeField]
    private WaveMovement PlayerWaveMovement = null;
    [SerializeField]
    private GameObject PlayerMesh = null;
    [SerializeField]
    private Camera MainCamera = null;

    [SerializeField]
    private RectTransform UpperImplicator = null;
    [SerializeField]
    private RectTransform LowerImplicator = null;

    [SerializeField]
    private Image GoingUpSignal = null;
    [SerializeField]
    private Image GoingDownSignal = null;

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

            UpperImplicator.sizeDelta = new Vector2(0.0f, amplitudeImplicatorWidth);
            LowerImplicator.sizeDelta = new Vector2(0.0f, amplitudeImplicatorWidth);
        }
        else
        {
            Debug.Log("No PlayerMesh / PlayerMovement / MainCamera defined in AmplitudeImplicator");
        }

        if (null != GoingUpSignal)
        {
            PlayerWaveMovement.OnReachedTopMostPoint += OnShowGoingDownSignal;
        }
        if (null != GoingDownSignal)
        {
            PlayerWaveMovement.OnReachedBottomMostPoint += OnShowGoingUpSignal;
        }
        GoingUpSignal.CrossFadeAlpha(1.0f, 0.0f, false);
        GoingDownSignal.CrossFadeAlpha(0.05f, 0.0f, false);
    }

    private void OnShowGoingUpSignal()
    {
        GoingUpSignal.CrossFadeAlpha(1.0f, 0.25f, false);
        GoingDownSignal.CrossFadeAlpha(0.05f, 0.25f, false);
    }

    private void OnShowGoingDownSignal()
    {
        GoingUpSignal.CrossFadeAlpha(0.05f, 0.25f, false);
        GoingDownSignal.CrossFadeAlpha(1.0f, 0.25f, false);
    }
}