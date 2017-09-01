using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(Rigidbody2D))]
[RequireComponent(typeof(ParticleSystem))]
public class Enemy : MonoBehaviour {

    private float m_ParticleCountMultiplier = 5.0f;

    private SpriteRenderer m_Renderer = null;
    private BoxCollider2D m_Collider = null;
    private ParticleSystem m_ParticleSystem = null;
    private WaveMovement m_WaveMovement = null;

    void Start()
    {
        FindComponents();
        SetSize(transform.localScale);
    }

    private void FindComponents()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<BoxCollider2D>();
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_WaveMovement = GetComponent<WaveMovement>();
        if (m_WaveMovement)
            m_WaveMovement.enabled = false;
    }

    public void SetSize(Vector2 NewSize)
    {
        //var shape = m_ParticleSystem.shape;
        //shape.box = NewSize;
        var emission = m_ParticleSystem.emission;
        emission.rateOverTime = NewSize.x * NewSize.y * m_ParticleCountMultiplier;
    }

    private void OnBecameVisible()
    {
        if (m_WaveMovement)
            m_WaveMovement.enabled = true;
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject, m_ParticleSystem.main.startLifetime.constant);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Die();
    }

    private void OnParticleTrigger()
    {
        Die();
    }

    public void Die()
    {
        m_ParticleSystem.Play();
        m_Renderer.enabled = false;
        m_Collider.enabled = false;
    }
}
