using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionImplicationController : MonoBehaviour {

    [SerializeField]
    private WaveMovement PlayerWaveMovement = null;

    [SerializeField]
    private Image UpperBoundSignal = null;

    [SerializeField]
    private Image LowerBoundSignal = null;

	// Use this for initialization
	void Start () {
        this.enabled = false;	
        if(null == PlayerWaveMovement)
        {
            PlayerWaveMovement = HelperFunctions.TryGetPlayerMovement();
        }
        if(null != UpperBoundSignal)
        {
            PlayerWaveMovement.OnReachedTopMostPoint += OnStartUpperBoundSignal;
        }
        if (null != LowerBoundSignal)
        {
            PlayerWaveMovement.OnReachedBottomMostPoint += OnStartLowerBoundSignal;
        }
        OnStartUpperBoundSignal();
    }

    private void OnStartUpperBoundSignal()
    {
        UpperBoundSignal.enabled = true;
        LowerBoundSignal.enabled = false;
    }

    private void OnStartLowerBoundSignal()
    {
        UpperBoundSignal.enabled = false;
        LowerBoundSignal.enabled = true;
    }
}
