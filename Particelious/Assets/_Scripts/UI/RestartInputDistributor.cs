using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartInputDistributor : MonoBehaviour {

	public void OnRestart()
    {
        GameManager.instance.OnRestart();
    }
}
