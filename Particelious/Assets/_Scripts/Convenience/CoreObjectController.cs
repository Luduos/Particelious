using UnityEngine;

public class CoreObjectController : MonoBehaviour {

    [SerializeField] 
    private PlayerController m_PlayerController = null;
    public PlayerController playerController { get { return m_PlayerController; } }

    [SerializeField]
    private CameraController m_CameraController = null;
    public CameraController cameraController { get { return m_CameraController; } }

    // Use this for initialization
    void Start () {
        this.enabled = false;	
	}
}
