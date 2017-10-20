using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(Rigidbody2D))]
[RequireComponent(typeof(ParticleSystem))]
public class Wall : Cullable {
    public static CullingPooler s_WallPool;

    private float m_ParticleCountMultiplier = 10.0f;

    private SpriteRenderer m_Renderer = null;
    private BoxCollider2D m_Collider = null;
    private ParticleSystem m_ParticleSystem = null;

    void Start()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<BoxCollider2D>();
        m_ParticleSystem = GetComponent<ParticleSystem>();
        Reset();
    }

    void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<BoxCollider2D>();
        m_ParticleSystem = GetComponent<ParticleSystem>();
        Reset();
    }

    public void SetSize(float Width, float Height)
    {
        Vector2 NewSize = new Vector2(Width, Height);
        SetSize(NewSize);
    }

    public void SetSize(Vector2 NewSize)
    {
        m_Renderer.size = NewSize;
        m_Collider.size = NewSize;
        var shape = m_ParticleSystem.shape;
        shape.scale = NewSize;
        var emission = m_ParticleSystem.emission;
        emission.rateOverTime = NewSize.x * NewSize.y * m_ParticleCountMultiplier;
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
