using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerLife : MonoBehaviour {

    private enum PlayerState
    {
        ALIVE,
        BLINKING,
        DYING
    }
    [SerializeField]
    private GameObject m_PlayerMesh = null;

    [SerializeField]
    private BlinkReaction m_PlayerBlinkReaction;
    [SerializeField]
    private PlayerDeathReaction m_PlayerDeathReaction;
    [SerializeField]
    private uint m_NumberOfStadiums = 3;
    [SerializeField]
    private float m_MinSizeMultiplicator = 0.8f;
    [SerializeField]
    private float m_MaxSizeMultiplicator = 1.2f;

    private float m_SizeDiffPerStadium;
    private uint m_CurrentStadium;
    private float m_CurrentScale;
    private float m_OldScale;

    private PlayerState m_CurrentPlayerState = PlayerState.ALIVE;

    void Start()
    {
        float denominator = Mathf.Max(m_NumberOfStadiums - 1, 1); 
        m_SizeDiffPerStadium = Mathf.Abs(m_MaxSizeMultiplicator - m_MinSizeMultiplicator) / denominator;
        m_CurrentStadium = m_NumberOfStadiums;
        UpdateCurrentScale();
        InitBlinkReaction();
        InitDeathReaction();
    }

    private void InitBlinkReaction()
    {
        TrailRenderer PlayerTrail = m_PlayerMesh.GetComponent<TrailRenderer>();
        if (PlayerTrail)
            PlayerTrail.startWidth = m_CurrentScale;

        if (null == m_PlayerBlinkReaction)
        {
            if (null != m_PlayerMesh)
            {
                m_PlayerBlinkReaction = m_PlayerMesh.GetComponent<BlinkReaction>();
            }
            else
            {
                Debug.LogError("No Player Mesh or PlayerBlinkReaction in PlayerLife.", this);
                return;
            }
        }
        m_PlayerBlinkReaction.OnBlinkEnded.AddListener(OnBlinkingEnded);
        m_PlayerMesh.transform.localScale = new Vector3(m_CurrentScale, m_CurrentScale, 1.0f);   
    }

    private void InitDeathReaction()
    {
        if (null == m_PlayerDeathReaction)
        {
            m_PlayerDeathReaction = m_PlayerMesh.GetComponent<PlayerDeathReaction>();
            if (!m_PlayerDeathReaction)
            {
                Debug.LogError("No PlayerDeathReaction in PlayerLife.", this);
                return;
            }
        }
        m_PlayerDeathReaction.OnDeathEnded.AddListener(OnDeathEnded);
    }

    public void OnHitEnemy()
    {
        if(m_CurrentPlayerState == PlayerState.ALIVE)
        {
            m_CurrentStadium--;
            CheckIfBlinkingOrDying();
        }
    }

    private void OnBlinkingEnded()
    {
        m_CurrentPlayerState = PlayerState.ALIVE;
    }

    private void OnDeathEnded()
    {
        this.gameObject.SetActive(false);
        GameManager.instance.OnPlayerDeath();
    }

    private void UpdateCurrentScale()
    {
        m_OldScale = CalcScale(Math.Min(m_NumberOfStadiums, m_CurrentStadium + 1));
        m_CurrentScale = CalcScale(m_CurrentStadium);
    }

    private float CalcScale(float Stadium)
    {
        return m_MaxSizeMultiplicator - (m_NumberOfStadiums - Stadium) * m_SizeDiffPerStadium;
    }

    private void CheckIfBlinkingOrDying()
    {
        m_CurrentPlayerState = (m_CurrentStadium > 0) ? PlayerState.BLINKING : PlayerState.DYING;
        UpdateCurrentScale();
        if (m_CurrentPlayerState == PlayerState.BLINKING)
        {
            if(m_PlayerBlinkReaction)
                m_PlayerBlinkReaction.OnStartBlinkAnimation(m_OldScale, m_CurrentScale);
        }else if(m_CurrentPlayerState == PlayerState.DYING)
        {
            if (m_PlayerDeathReaction)
                m_PlayerDeathReaction.OnStartDeathAnimation(m_CurrentScale);
        }
    }
}