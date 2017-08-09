using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

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

    static protected GameManager s_Instance;

    [SerializeField] private GameObject Player;

    void OnEnable()
    {
        s_Instance = this;   
    }

    void OnDisable()
    {
        if (s_Instance)
            Destroy(s_Instance.gameObject);
    }

    // Use this for initialization
    void Start () {
	    if(null == Player)
        {
            FindPlayer();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FindPlayer()
    {
        PlayerControl playerControl = FindObjectOfType<PlayerControl>();
        if (null != playerControl)
        {
            Player = playerControl.gameObject;
        }
        else
        {
            Debug.LogWarning("Could not find Gameobject with PlayerControl in Scene", this);
        }
    }
}
