using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public void LevelToLoad(string LevelName)
    {
        SceneManager.LoadScene(LevelName);
    }
}
