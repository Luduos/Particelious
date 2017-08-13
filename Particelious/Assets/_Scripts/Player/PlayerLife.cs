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
    private GameObject PlayerFigure = null;
    private TrailRenderer PlayerTrail = null;
    private SpriteRenderer PlayerRenderer = null;
    [SerializeField]
    private uint m_NumberOfStadiums = 3;
    [SerializeField]
    private float m_MinSizeMultiplicator = 0.3f;
    [SerializeField]
    private float m_MaxSizeMultiplicator = 1.0f;

    [SerializeField]
    private Color m_BlinkTint = new Color(1.0f, 1.0f, 1.0f, 0.7f);
    private Color OriginalPlayerColor;
    private Color OriginalTrailColor;
    [SerializeField]
    private float m_BlinkingTime = 2.0f;
    [SerializeField]
    private float m_BlinkingSpeed = 3.0f;

    private float m_SizeDiffPerStadium;
    private uint m_CurrentStadium;
    private PlayerState m_CurrentPlayerState = PlayerState.ALIVE;

    private float m_AccumulatedBlinkTime = 0.0f;
    private float m_CurrentScale;
    private float m_OldScale;

    void Start()
    {
        PlayerTrail = PlayerFigure.GetComponent<TrailRenderer>();
        PlayerRenderer = PlayerFigure.GetComponent<SpriteRenderer>();
        m_SizeDiffPerStadium = Mathf.Abs(m_MaxSizeMultiplicator - m_MinSizeMultiplicator) / (m_NumberOfStadiums - 1);
        m_CurrentStadium = m_NumberOfStadiums;
        UpdateCurrentScale();
        SetScale(m_CurrentScale);
    }

    public void OnHitEnemy()
    {
        if(m_CurrentPlayerState == PlayerState.ALIVE)
        {
            OriginalPlayerColor = PlayerRenderer.color;
            OriginalTrailColor = PlayerTrail.startColor;
            Color trailTint = OriginalTrailColor;
            trailTint.a = m_BlinkTint.a;
            PlayerTrail.startColor = trailTint;
            PlayerRenderer.color = m_BlinkTint;
            m_CurrentStadium--;
            UpdateCurrentScale();
            CheckIfBlinkingOrDying();
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        switch (m_CurrentPlayerState)
        {
            case PlayerState.ALIVE:
                break;
            case PlayerState.BLINKING:
                UpdateBlinking();
                break;
            case PlayerState.DYING:
                UpdateDying();
                break;
        }
	}

    private void UpdateBlinking()
    {
        m_AccumulatedBlinkTime += Time.deltaTime;
        if(m_AccumulatedBlinkTime > m_BlinkingTime)
        {
            OnLeaveBlink();
        }
        else
        {
            float BlinkAnimationFactor = Mathf.Sin(m_AccumulatedBlinkTime * m_BlinkingSpeed);
            if(BlinkAnimationFactor > 0.7f)
            {
                SetPlayerRendererEnabled(true);
                SetScale(m_OldScale);
            }else if(BlinkAnimationFactor < - 0.7f)
            {
                SetPlayerRendererEnabled(true);
                SetScale(m_CurrentScale);
            }
        }
    }

    private void OnLeaveBlink()
    {
        PlayerRenderer.color = OriginalPlayerColor;
        PlayerTrail.startColor = OriginalTrailColor;

        SetPlayerRendererEnabled(true);
        SetScale(m_CurrentScale);
        m_AccumulatedBlinkTime = 0.0f;
        m_CurrentPlayerState = (m_CurrentStadium > 0) ? PlayerState.ALIVE : PlayerState.DYING;
    }

    private void SetPlayerRendererEnabled(bool enabled)
    {
        if(PlayerFigure)
        {
            PlayerFigure.SetActive(enabled);
        }
    }

    private void SetScale(float Factor)
    {
        if(Factor > 0.0f)
        {
            PlayerFigure.transform.localScale = new Vector3(Factor, Factor, 1.0f);
            if (PlayerTrail)
            {
                PlayerTrail.startWidth = Factor;
            }
        }
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
    }

    private void UpdateDying()
    {

    }
}