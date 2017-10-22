using UnityEngine;
using UnityEngine.SceneManagement;

public class AndroidIngameBackButtonController : MonoBehaviour {

//if UNITY_ANDROID || UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        }
    }
//endif
}
