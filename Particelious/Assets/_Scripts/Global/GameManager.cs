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

    private GameInfo m_GameInfo = new GameInfo(0);
    public GameInfo gameInfo { get { return m_GameInfo; } }

    public GameManager()
    {
        s_Instance = this;
    }

    void OnDestroy()
    {
        s_Instance = null;
    }

 
}
