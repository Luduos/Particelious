using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Canvas))]
public class GlobalCanvasCameraFetcher : MonoBehaviour {


    private Canvas CurrentCanvas = null;
    // Use this for initialization

    void Start()
    {
        if (null == CurrentCanvas)
            CurrentCanvas = GetComponent<Canvas>();

        GameState.OnFinishedGameSessionLoading += LevelLoaded;
        SceneManager.sceneLoaded += LevelLoaded;
    }


    void LevelLoaded()
    {
        if (CurrentCanvas)
        {
            CurrentCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            CurrentCanvas.worldCamera = FindObjectOfType<CameraController>().GetComponent<Camera>();
        }
    }

    void LevelLoaded(Scene loadedScene, LoadSceneMode loadSceneMode)
    {
        if (CurrentCanvas)
        {
            CurrentCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            CurrentCanvas.worldCamera = Camera.main;
        }
    }

    private void OnDestroy()
    {
        GameState.OnFinishedGameSessionLoading -= LevelLoaded;
        SceneManager.sceneLoaded -= LevelLoaded;

    }

}
