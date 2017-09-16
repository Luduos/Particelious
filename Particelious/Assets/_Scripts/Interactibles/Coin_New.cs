using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D), typeof(Rigidbody2D))]
[RequireComponent(typeof(ParticleSystem))]
public class Coin_New : MonoBehaviour {

    private SpriteRenderer m_Renderer = null;
    private CircleCollider2D m_Collider = null;
    private ParticleSystem m_ParticleSystem = null;

    void Start()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<CircleCollider2D>();
        m_ParticleSystem = GetComponent<ParticleSystem>();
    }

    void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<CircleCollider2D>();
        m_ParticleSystem = GetComponent<ParticleSystem>();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        m_ParticleSystem.Play();
        m_Renderer.enabled = false; 
        m_Collider.enabled = false;
    } 
}
