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

    public static System.Action OnExitRestartState;

    public override void Enter(AState from)
    {
        SceneManager.LoadScene(m_RestartMenuSceneName);
    }

    public override void Exit(AState to)
    {
        if(null != OnExitRestartState)
        {
            OnExitRestartState.Invoke();
        }
    }

    public override string GetName()
    {
        return s_RestartStateName;
    }
}
