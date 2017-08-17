using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : AState {
    [SerializeField]
    private CoreObjectController m_CoreObjectsPrefab = null;

    private PlayerController m_PlayerController = null;
    public PlayerController playerController { get { return m_PlayerController; } }

    private CameraController m_CameraController = null;
    public CameraController cameraController { get { return m_CameraController; } }

    public Camera mainCamera { get { return m_CameraController.GetComponent<Camera>(); } }

    public override string GetName()
    {
        return "GameState";
    }

    public override void Enter(AState from)
    {
        
    }

    public override void Exit(AState to)
    {
        
    }

    private void StartGamePlay()
    {
        CoreObjectController coreCtrl = Instantiate(m_CoreObjectsPrefab);
        m_PlayerController = coreCtrl.playerController;
        m_CameraController = coreCtrl.cameraController;
    }
}
