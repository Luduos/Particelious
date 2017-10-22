using UnityEngine;
using UnityEngine.SceneManagement;

public class AndroidBackButtonController : MonoBehaviour {

    [SerializeField]
    private bool IsMainMenu = false;

#if UNITY_ANDROID || UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsMainMenu)
            {
                Application.Quit();
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }
#endif
}
