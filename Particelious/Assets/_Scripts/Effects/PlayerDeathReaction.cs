using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/** Used to initiate death-reaction on player by shrinking him to nearly nothing, then exploding him.
 *  Needs at least a SpriteRenderer. */
[RequireComponent(typeof(SpriteRenderer), typeof(ParticleSystem))]
public class PlayerDeathReaction : MonoBehaviour {

    public UnityEvent OnDeathEnded;

    private enum PlayerDeathState
    {
        SHRINKING,
        EXPLODING
    }

    [SerializeField]
    private PlayerControl m_PlayerControl;

    [SerializeField]
    private float m_ExplosionAfterSeconds = 3.0f;

    [SerializeField]
    private bool m_ShouldPerformCameraShake = true;
    [SerializeField]
    private CameraShakeInformation m_CameraShakeInfo = new CameraShakeInformation(1.0f, 1.0f);

    private CameraController m_CameraController = null;
    private TrailRenderer m_Trail = null;
    private SpriteRenderer m_SpriteRenderer = null;
    private ParticleSystem m_DeathEffect = null;

    private PlayerDeathState m_CurrentState = PlayerDeathState.SHRINKING;

    private float m_AccumulatedDeathTime = 0.0f;
    private float m_OriginalScale;
    private float m_CurrentScale;

    // Use this for initialization
    void Start () {
		if(null == m_PlayerControl)
        {
            m_PlayerControl = FindObjectOfType<PlayerControl>() as PlayerControl;
        }
        m_CameraController = FindObjectOfType<CameraController>();
        m_Trail = GetComponent<TrailRenderer>();
        m_DeathEffect = GetComponent<ParticleSystem>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        m_AccumulatedDeathTime += Time.deltaTime;

        if(m_AccumulatedDeathTime < m_ExplosionAfterSeconds)
        {
            UpdateDeathReaction();
        }else
        {
            UpdateExplosion();
        }
    }

    public void OnStartDeathAnimation(float StartingScale)
    {
        m_OriginalScale = StartingScale;
        m_CurrentScale = StartingScale;
        if (m_PlayerControl)
        {
            m_PlayerControl.SetPlayerCurrentSpeed(0.0f);
            m_PlayerControl.SetPlayerFrequency(0.0f);
            m_PlayerControl.SetPlayerControlEnabled(false);
        }
        if (m_Trail)
        {
            m_Trail.enabled = false;
        }
        this.enabled = true;
    }

    private void UpdateDeathReaction()
    {
        m_CurrentScale = ((m_ExplosionAfterSeconds - m_AccumulatedDeathTime) / m_ExplosionAfterSeconds) * m_OriginalScale;
        m_CurrentScale = Mathf.Max(m_CurrentScale, 0.1f);
        this.transform.localScale = new Vector3(m_CurrentScale, m_CurrentScale, m_CurrentScale);
    }

    private void UpdateExplosion()
    {
        if(m_CurrentState != PlayerDeathState.EXPLODING)
        {
            m_CurrentState = PlayerDeathState.EXPLODING;
            m_SpriteRenderer.enabled = false;
            if (m_ShouldPerformCameraShake)
            {
                m_CameraController.StartCameraShake(m_CameraShakeInfo);
            }
            m_DeathEffect.Play();
        }else
        {
            if (!m_DeathEffect.isPlaying)
            {
                OnEndDeathAnimation();
            }
        }
    }

    private void OnEndDeathAnimation()
    {
        m_AccumulatedDeathTime = 0.0f;
        this.enabled = false;
        OnDeathEnded.Invoke();
    }
}
