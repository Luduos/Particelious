﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {
    [SerializeField] private WaveMovement m_FollowTarget = null;
    [SerializeField] private Vector3 m_CameraDistance = new Vector3(0.0f, 0.0f, -10.0f);
    public Vector3 CameraDistance { get { return m_CameraDistance; } set { m_CameraDistance = value; } }
    [SerializeField] private float m_FollowSpeed = 0.3f;
    private Vector3 m_CurrentCameraVelocity = Vector3.zero;

    private Vector3 m_ShakeOffset = Vector3.zero;
    private float m_TimeToShake = 0.0f;
    private float m_ShakeIntensity = 0.0f;
    private float m_AccumulatedShakeTime = 0.0f;

	void Update () {
        Vector3 CameraOffset = m_CameraDistance;
        if(m_AccumulatedShakeTime < m_TimeToShake)
        {
            m_AccumulatedShakeTime += Time.deltaTime;
            m_ShakeOffset = new Vector2(Random.Range(-m_ShakeIntensity, m_ShakeIntensity), Random.Range(-m_ShakeIntensity, m_ShakeIntensity));
            CameraOffset += m_ShakeOffset;
        }
        Vector3 targetPosition = m_FollowTarget.OscillationOrigin + CameraOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref m_CurrentCameraVelocity, m_FollowSpeed, 50.0f, Time.deltaTime);
    }

    public void StartCameraShake(CameraShakeInformation info)
    {
        StartCameraShake(info.Duration, info.Intensity);
    }

    public void StartCameraShake(float duration, float intensity)
    {
        m_TimeToShake = duration;
        m_ShakeIntensity = intensity;
        m_AccumulatedShakeTime = 0.0f;
    }
}

[System.Serializable]
public struct CameraShakeInformation
{
    public float Duration;
    public float Intensity;

    public CameraShakeInformation(float duration, float intensity)
    {
        Duration = duration;
        Intensity = intensity;
    }
}