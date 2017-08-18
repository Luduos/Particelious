using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    static protected GameManager s_Instance;
    static public GameManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                GameObject o = new GameObject("GameManager");
                DontDestroyOnLoad(o);
                s_Instance = o.AddComponent<GameManager>();
            }
            return s_Instance;
        }
    }

    

    [SerializeField]
    private AState[] m_States;

    private GameInfo m_GameInfo = new GameInfo(0);
    public GameInfo gameInfo { get { return m_GameInfo; } set { m_GameInfo = value; } }

    private Dictionary<string, AState> m_StateDictionary;
    private Stack<AState> m_CurrentStateStack;

    void Start()
    {
        if (null != s_Instance)
        {
            Destroy(this);
            return;
        }

        s_Instance = this;
        this.enabled = false;
        DontDestroyOnLoad(this.gameObject);

        m_CurrentStateStack = new Stack<AState>(3);
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
    void OnDestroy()
    {
        if(this == s_Instance)
            s_Instance = null;
    }

    public void OnRestart()
    {
        ExitCurrentState();
        OnStartGame();
    }

    public void OnStartGame()
    {
        SwitchState("GameState");
    }
 
    public void SwitchState(string target)
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

    public void AddState(string target)
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

    public AState ExitCurrentState()
    {
        AState exitedState = null;
        if (m_CurrentStateStack.Count > 1)
        {
            exitedState = m_CurrentStateStack.Pop();
            if (exitedState)
            {
                exitedState.Exit(m_CurrentStateStack.Peek());
            }
        }
        return exitedState;
    }
}
