using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtremumParticleStarter : MonoBehaviour {

    [SerializeField]
    private WaveMovement PlayerWaveMovement = null;

    [SerializeField]
    private ParticleSystem UpperBoundSignal = null;

    [SerializeField]
    private ParticleSystem LowerBoundSignal = null;

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
    }

    private void OnStartUpperBoundSignal()
    {
        UpperBoundSignal.Play();
    }

    private void OnStartLowerBoundSignal()
    {
        LowerBoundSignal.Play();
    }
}
