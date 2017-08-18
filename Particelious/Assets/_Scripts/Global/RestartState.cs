using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class RestartState : AState
{
    [SerializeField]
    private string RestartMenuSceneName = "RestartMenu";

    public override void Enter(AState from)
    {
        SceneManager.LoadScene(RestartMenuSceneName, LoadSceneMode.Additive);
    }

    public void OnRestart()
    {
        manager.ExitCurrentState();
        manager.SwitchState("GameState");
    }

    public override void Exit(AState to)
    {

    }

    public override string GetName()
    {
        return "RestartState";
    }
}
