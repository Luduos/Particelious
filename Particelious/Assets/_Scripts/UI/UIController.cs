using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {
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

    void Start () {
        this.enabled = false;
        UpdateAmplitudeImplicators();  
    }

    public void UpdateAmplitudeImplicators()
    {
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
    }
	
	
}
