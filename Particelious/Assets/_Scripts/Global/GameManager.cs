using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    static protected GameManager s_Instance = null;
    static public GameManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                GameObject o = new GameObject("GameManager");
                DontDestroyOnLoad(o);
                s_Instance = o.AddComponent<GameManager>();

                Debug.LogWarning("Manually constructed GameManager object, shouldn't usually happen.");
            }
            return s_Instance;
        }
    }
    [SerializeField]
    private AState[] m_States;

    private Dictionary<string, AState> m_StateDictionary;
    private Stack<AState> m_CurrentStateStack;


    void Start()
    {
        if (null != s_Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        s_Instance = this;
        DontDestroyOnLoad(this.gameObject);

        m_CurrentStateStack = new Stack<AState>(10);

        if (null == m_States)
        {
            return;
        }

        m_StateDictionary = new Dictionary<string, AState>(m_States.Length);
        foreach (AState currentState in m_States)
        {
            m_StateDictionary.Add(currentState.GetName(), currentState);
            currentState.manager = this;
        }
        if (m_States.Length > 0)
        {
            m_CurrentStateStack.Push(m_States[0]);
            m_States[0].Enter(null);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            string CurrentStateName = "";
            AState CurrentState = m_CurrentStateStack.Peek();
            if (CurrentState)
                CurrentStateName = CurrentState.GetName();

            if (CurrentStateName.Equals(MainMenuState.GetMainMenuStateName()))
                OnExitApplication(CurrentStateName);
            else
                OnMainMenu();
        }
    }

    void OnDestroy()
    {
        if(this == s_Instance)
            s_Instance = null;
    }

    public void OnStartGame()
    {
        SwitchState(GameState.GetGameStateName());
    }

    public void OnPlayerDeath()
    {
        this.SwitchState(RestartState.GetRestartStateName());
    }

    public void OnFinishedLevel()
    {
        this.SwitchState(FinishedLevelState.GetFinishedLevelStateName());
    }

    public void OnRestart()
    {
        OnStartGame();
    }

    public void OnMainMenu()
    {
        this.SwitchState(MainMenuState.GetMainMenuStateName());
    }

    public void OnExitApplication(string from)
    {
        Application.Quit();    
    }

    private void SwitchState(string target)
    {
        AState targetState = null;

        if(null != m_StateDictionary)
            m_StateDictionary.TryGetValue(target, out targetState);

        if (targetState)
        {
            AState exitedState = ExitCurrentState();
            targetState.Enter(exitedState);
            m_CurrentStateStack.Push(targetState);
        }
    }

    private void AddState(string target)
    {
        AState targetState = null;

        if(null != m_StateDictionary)
            m_StateDictionary.TryGetValue(target, out targetState);

        if (targetState)
        {
            targetState.Enter(m_CurrentStateStack.Peek());
            m_CurrentStateStack.Push(targetState);
        }
    }

    private AState ExitCurrentState()
    {
        AState exitedState = null;
        if (m_CurrentStateStack.Count > 0)
        {
            exitedState = m_CurrentStateStack.Pop();
            if (exitedState)
            {
                exitedState.Exit(null);
            }
        }
        return exitedState;
    }
}
