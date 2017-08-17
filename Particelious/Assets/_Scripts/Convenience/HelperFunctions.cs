using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions {
    public static GameObject TryToFindPlayer()
    {
        GameObject player = null;
        PlayerController playerControl = GameObject.FindObjectOfType<PlayerController>();
        if (null != playerControl)
        {
            player = playerControl.gameObject;
        }else
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        return player;
    }
}
