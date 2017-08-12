using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D), typeof(Rigidbody2D))]
public class Wall : MonoBehaviour {
    public static Pooler s_WallPool;
    public static float s_LiveTime = 15.0f;

    private float m_CurrentLiveTime = 0.0f;

    void Update()
    {
        m_CurrentLiveTime += Time.deltaTime;
        if (m_CurrentLiveTime > s_LiveTime)
        {
            Reset();
            s_WallPool.Free(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Reset();
        s_WallPool.Free(this.gameObject);
    }

    private void Reset()
    {
        m_CurrentLiveTime = 0.0f;
    }

}
