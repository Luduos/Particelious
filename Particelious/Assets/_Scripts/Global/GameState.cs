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
    [SerializeField]
    private GameObject m_WallSpawnerPrefab = null;

    private static string s_GameStateName  = "GameState";
    public static string GetGameStateName() { return s_GameStateName; }

    private PlayerController m_PlayerController = null;
    public PlayerController playerController { get { return m_PlayerController; } }

    private CameraController m_CameraController = null;
    public CameraController cameraController { get { return m_CameraController; } }

    public Camera mainCamera { get { return m_CameraController.GetComponent<Camera>(); } }

    private GameObject m_CoreGameObject = null;
    private GameInfo m_CurrentGameInfo;

    public override string GetName()
    {
        return s_GameStateName;
    }

    public override void Enter(AState from)
    {
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        SceneManager.sceneLoaded += LevelLoaded;
        SceneManager.LoadScene(MainGameSceneName);
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
    }

    private void StartGamePlay()
    {
        m_CurrentGameInfo = manager.gameInfo;

        CoreObjectController coreCtrl = Instantiate(m_CoreObjectsPrefab);
        if (m_CoreGameObject)
            Destroy(m_CoreGameObject);
        m_CoreGameObject = coreCtrl.gameObject;
        m_PlayerController = coreCtrl.playerController;
        m_CameraController = coreCtrl.cameraController;
        Instantiate(m_WallSpawnerPrefab);
    }
}
