using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameState : AState {
    [SerializeField]
    private string MainGameSceneName = "Main";
    [SerializeField]
    private CoreObjectController m_CoreObjectsPrefab = null;

    private static string s_GameStateName  = "GameState";
    public static string GetGameStateName() { return s_GameStateName; }

    public static System.Action OnGameSessionEnter;
    public static System.Action OnFinishedGameSessionLoading;
    public static System.Action OnGameSessionExit;

    private PlayerController m_PlayerController = null;
    public PlayerController playerController { get { return m_PlayerController; } }

    private CameraController m_CameraController = null;
    public CameraController cameraController { get { return m_CameraController; } }

    public Camera mainCamera { get { return m_CameraController.GetComponent<Camera>(); } }

    private GameObject m_CoreGameObject = null;

    public override string GetName()
    {
        return s_GameStateName;
    }

    public override void Enter(AState from)
    {
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        SceneManager.sceneLoaded += LevelLoaded;
        SceneManager.LoadScene(MainGameSceneName);

        if(null != OnGameSessionEnter)
        {
            OnGameSessionEnter.Invoke();
        }
    }
    void LevelLoaded(Scene loadedScene, LoadSceneMode loadSceneMode)
    {
        if(loadedScene.name == MainGameSceneName)
        {
           StartGamePlay();
        }
    }
    public override void Exit(AState to)
    {
        SceneManager.sceneLoaded -= LevelLoaded;
        if (null != OnGameSessionExit)
        {
            OnGameSessionExit.Invoke();
        }
    }

    private void StartGamePlay()
    {
        CoreObjectController coreCtrl = FindObjectOfType<CoreObjectController>();
        if (!coreCtrl)
        {
            coreCtrl = Instantiate(m_CoreObjectsPrefab);
        }

        m_CoreGameObject = coreCtrl.gameObject;
        m_PlayerController = coreCtrl.playerController;
        m_CameraController = coreCtrl.cameraController;

        if(null != OnFinishedGameSessionLoading)
        {
            OnFinishedGameSessionLoading.Invoke();
        }
    }
}
