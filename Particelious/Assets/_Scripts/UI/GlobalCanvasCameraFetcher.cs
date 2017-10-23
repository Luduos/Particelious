using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class GlobalCanvasCameraFetcher : MonoBehaviour {
    [SerializeField]
    private Canvas CurrentCanvas = null;

    void Start()
    {
        if (null == CurrentCanvas)
            CurrentCanvas = GetComponent<Canvas>();

        SceneManager.sceneLoaded += LevelLoaded;
        SceneManager.sceneUnloaded += LevelUnloaded;
    }

    void LevelLoaded(Scene loadedScene, LoadSceneMode loadSceneMode)
    {
        ActivateBloomCamOnCanvas();
    }

    void LevelUnloaded(Scene unloadedScene)
    {
        ActivateBloomCamOnCanvas();
    }

    private void ActivateBloomCamOnCanvas()
    {
        if (CurrentCanvas)
        {
            CurrentCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            MobileBloom mobileBloomCamera = FindObjectOfType<MobileBloom>();
            if (mobileBloomCamera)
            {
                CurrentCanvas.worldCamera = mobileBloomCamera.GetComponent<Camera>();
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= LevelLoaded;
    }
}
