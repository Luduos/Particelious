using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions {
    public static GameObject TryToFindPlayer()
    {
        GameObject player = null;
        PlayerControl playerControl = GameObject.FindObjectOfType<PlayerControl>();
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
