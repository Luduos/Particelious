using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleMenuLevelLoader : MonoBehaviour {

    public void LevelToLoad(string LevelName)
    {
        SceneManager.LoadScene(LevelName);
    }

    public void RestartCurrentGameLevel()
    {
        int levelID = GlobalInfo.instance.CurrentLevel;
        if(levelID >= 0)
        {
            SceneManager.LoadScene("Level_" + levelID);
        }
    }

    public void LeaveCurrentAdditiveLevel()
    {
        if(SceneManager.sceneCount > 1)
        {
            int latestSceneID = SceneManager.sceneCount - 1;
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(latestSceneID));
        }
    }
}
