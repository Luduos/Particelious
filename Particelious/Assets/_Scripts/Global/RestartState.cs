using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class RestartState : AState
{
    [SerializeField]
    private string m_RestartMenuSceneName = "RestartMenu";

    private static string s_RestartStateName = "RestartState";
    public static string GetRestartStateName() { return s_RestartStateName; }

    public override void Enter(AState from)
    {
        SceneManager.LoadScene(m_RestartMenuSceneName, LoadSceneMode.Additive);
    }

    public override void Exit(AState to)
    {

    }

    public override string GetName()
    {
        return s_RestartStateName;
    }
}
