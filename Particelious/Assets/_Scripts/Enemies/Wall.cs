using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(Rigidbody2D))]
[RequireComponent(typeof(ParticleSystem))]
public class Wall : MonoBehaviour {
    public static Pooler s_WallPool;
    public static float s_LiveTime = 15.0f;

    private float m_CurrentLiveTime = 0.0f;

    private SpriteRenderer m_Renderer = null;
    private BoxCollider2D m_Collider = null;
    private ParticleSystem m_ParticleSystem = null;

    void Start()
    {
        m_Renderer = this.gameObject.GetComponent<SpriteRenderer>();
        m_Collider = this.gameObject.GetComponent<BoxCollider2D>();
    }

    void Awake()
    {
        m_Renderer = this.gameObject.GetComponent<SpriteRenderer>();
        m_Collider = this.gameObject.GetComponent<BoxCollider2D>();
        m_ParticleSystem = this.gameObject.GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        m_CurrentLiveTime += Time.deltaTime;
        if (m_CurrentLiveTime > s_LiveTime)
        {
            Reset();
            s_WallPool.Free(this.gameObject);
        }
    }

    // This doesn't work for some reason.
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
        shape.box = NewSize;
        var emission = m_ParticleSystem.emission;
        emission.rateOverTime = NewSize.x * NewSize.y * 100.0f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        m_ParticleSystem.Play();
        m_Renderer.enabled = false;
        m_Collider.enabled = false;
    }

    private void Reset()
    {
        m_CurrentLiveTime = 0.0f;
        m_Renderer.enabled = true;
        m_Collider.enabled = true;
    }

}
