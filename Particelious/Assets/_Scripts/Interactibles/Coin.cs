using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D), typeof(Rigidbody2D))]
[RequireComponent(typeof(ParticleSystem))]
public class Coin : Cullable {
    public static CullingPooler s_CoinPool;

    private SpriteRenderer m_Renderer = null;
    private CircleCollider2D m_Collider = null;
    private ParticleSystem m_ParticleSystem = null;

    void Start()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<CircleCollider2D>();
        m_ParticleSystem = GetComponent<ParticleSystem>();
        Reset();
    }

    void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<CircleCollider2D>();
        m_ParticleSystem = GetComponent<ParticleSystem>();
        Reset();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_ParticleSystem.Play();
            m_Renderer.enabled = false;
            m_Collider.enabled = false;
        }
    }

    public override void Reset()
    {
        this.enabled = false;
        m_Renderer.enabled = true;
        m_Collider.enabled = true;
    }
}
