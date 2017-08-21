using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions {
    private static GameObject s_CurrentPlayer = null;

    public static GameObject TryToFindPlayer()
    {
        if(null == s_CurrentPlayer)
        {
            PlayerController playerControl = GameObject.FindObjectOfType<PlayerController>();
            if (null != playerControl)
            {
                s_CurrentPlayer = playerControl.gameObject;
            }
            else
            {
                s_CurrentPlayer = GameObject.FindGameObjectWithTag("Player");
            }
        }
        return s_CurrentPlayer;
    }

    public static WaveMovement TryGetPlayerMovement()
    {
        WaveMovement playerWaveMovement = null;
        if (null == s_CurrentPlayer)
        {
            TryToFindPlayer();
            if (s_CurrentPlayer)
            {
                playerWaveMovement = s_CurrentPlayer.GetComponent<WaveMovement>(); 
            }
        }
        return playerWaveMovement;
    }
}
