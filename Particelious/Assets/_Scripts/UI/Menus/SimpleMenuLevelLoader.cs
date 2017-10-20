using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleMenuLevelLoader : MonoBehaviour {

    public void LevelToLoad(string LevelName)
    {
        SceneManager.LoadScene(LevelName);
    }
}
