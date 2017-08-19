using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuState : AState
{
    [SerializeField] private string m_MainMenuLevelName = "_MainMenu";

    private static string s_MainMenuStateName = "MainMenuState";
    public static string GetMainMenuStateName() { return s_MainMenuStateName; }

    public override void Enter(AState from)
    {
        string ActiveSceneName = SceneManager.GetActiveScene().name;
        if (!ActiveSceneName.Equals(m_MainMenuLevelName))
        {
            SceneManager.LoadScene(m_MainMenuLevelName);
        }
    }

    public override void Exit(AState to)
    {
    }

    public override string GetName()
    {
        return s_MainMenuStateName;
    }
}
