using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishedLevelState : AState {

    [SerializeField]
    private string m_FinishedLevelSceneName = "FinishedLevel";

    private static string s_FinishedLeveltStateName = "FinishedLevelState";
    public static string GetFinishedLevelStateName() { return s_FinishedLeveltStateName; }

    public override void Enter(AState from)
    {
        SceneManager.LoadScene(m_FinishedLevelSceneName);
    }

    public override void Exit(AState to)
    {

    }

    public override string GetName()
    {
        return s_FinishedLeveltStateName;
    }
}
