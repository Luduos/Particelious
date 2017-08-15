using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerControl))]
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
        m_SizeDiffPerStadium = Mathf.Abs(m_MaxSizeMultiplicator - m_MinSizeMultiplicator) / (m_NumberOfStadiums - 1);
        m_CurrentStadium = m_NumberOfStadiums;
        UpdateCurrentScale();
        InitBlinkReaction();
    }

    private void InitBlinkReaction()
    {
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
        TrailRenderer PlayerTrail = m_PlayerMesh.GetComponent<TrailRenderer>();
        if (PlayerTrail)
            PlayerTrail.startWidth = m_CurrentScale;
    }

    public void OnHitEnemy()
    {
        if(m_CurrentPlayerState == PlayerState.ALIVE)
        {
            CheckIfBlinkingOrDying();
        }
    }

    private void OnBlinkingEnded()
    {
        m_CurrentPlayerState = PlayerState.ALIVE;
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
        if(m_CurrentPlayerState == PlayerState.BLINKING)
        {
            m_CurrentStadium--;
            UpdateCurrentScale();
            if(m_PlayerBlinkReaction)
                m_PlayerBlinkReaction.StartBlinking(m_OldScale, m_CurrentScale);
        }else
        {
            // TODO: Dying
        }
    }
}