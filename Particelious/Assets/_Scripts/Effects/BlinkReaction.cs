using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/** Used to initiate blinking on the current figure. Current GameObject needs at least a SpriteRenderer*/
[RequireComponent(typeof(SpriteRenderer))]
public class BlinkReaction : MonoBehaviour {

    public UnityEvent OnBlinkEnded;

    [SerializeField]
    private Color m_BlinkTint = new Color(1.0f, 1.0f, 1.0f, 0.7f);
    [SerializeField]
    private float m_BlinkingAnimationDuration = 2.0f;
    [SerializeField]
    private float m_BlinkingSpeed = 12.0f;
    [SerializeField]
    [Range(0.1f, 0.9f)]
    private float m_BlinkAnimationThreshold = 0.7f;

    [SerializeField]
    private bool m_ShouldPerformCameraShake = true;
    [SerializeField]
    private CameraShakeInformation m_CameraShakeInfo = new CameraShakeInformation(1.0f, 1.0f);

    private TrailRenderer m_Trail = null;
    private SpriteRenderer m_Renderer = null;
    private CameraController m_CameraController = null;

    private Color m_OriginalRendererColor;
    private Color m_OriginalTrailColor;

    private float m_AccumulatedBlinkTime = 0.0f;
    private float m_OldScale;
    private float m_NewScale;

    // Use this for initialization
    void Start () {
        m_Trail = gameObject.GetComponent<TrailRenderer>();
        m_Renderer = gameObject.GetComponent<SpriteRenderer>();
        m_CameraController = FindObjectOfType<CameraController>();

        this.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        m_AccumulatedBlinkTime += Time.deltaTime;
        if (m_AccumulatedBlinkTime > m_BlinkingAnimationDuration)
        {
            OnEndBlinkAnimation();
        }
        else
        {
            float BlinkAnimationFactor = Mathf.Sin(m_AccumulatedBlinkTime * m_BlinkingSpeed);
            if (BlinkAnimationFactor > m_BlinkAnimationThreshold)
            {
                SetScale(m_OldScale);
            }
            else if (BlinkAnimationFactor < -m_BlinkAnimationThreshold)
            {
                SetScale(m_NewScale);
            }
        }
    }

    public void OnStartBlinkAnimation(float OldScale, float NewScale)
    {
        m_OldScale = OldScale;
        m_NewScale = NewScale;

        m_OriginalRendererColor = m_Renderer.color;
        m_Renderer.color = m_BlinkTint;
        if (m_Trail)
        {
            m_OriginalTrailColor = m_Trail.startColor;
            Color trailTint = m_OriginalTrailColor;
            trailTint.a = m_BlinkTint.a;
            m_Trail.startColor = trailTint;
        }
        if(m_ShouldPerformCameraShake && m_CameraController)
        {
            m_CameraController.StartCameraShake(m_CameraShakeInfo);
        }

        this.enabled = true;
    }

    private void OnEndBlinkAnimation()
    {
        m_Renderer.color = m_OriginalRendererColor;
        if (m_Trail)
            m_Trail.startColor = m_OriginalTrailColor;

        SetScale(m_NewScale);
        m_AccumulatedBlinkTime = 0.0f;

        this.enabled = false;
        OnBlinkEnded.Invoke();
    }

    private void SetScale(float Factor)
    {
        if (Factor > 0.0f)
        {
            this.transform.localScale = new Vector3(Factor, Factor, Factor);
            if (m_Trail)
            {
                m_Trail.startWidth = Factor;
            }
        }
    }  
}
