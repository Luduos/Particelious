using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDistributor : MonoBehaviour {

    public void OnStartGame()
    {
        GameManager.instance.OnStartGame();
    }

	public void OnRestart()
    {
        GameManager.instance.OnRestart();
    }

    public void OnMainMenu(string from)
    {
        GameManager.instance.OnMainMenu();
    }

    public void OnExitApplication(string from)
    {
        GameManager.instance.OnExitApplication(from);
    }
}
