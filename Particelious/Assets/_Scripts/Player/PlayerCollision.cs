using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerCollision : MonoBehaviour {
    [SerializeField]
    private string m_EnemyTag = "Enemy";
    [SerializeField]
    private string m_CoinTag = "Coin";

    public UnityEvent OnHit;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(m_EnemyTag))
        {
            OnHit.Invoke();
        }else if (collision.gameObject.CompareTag(m_CoinTag))
        {
            GlobalInfo.instance.CollectedCoin();
        }
        
    }
}
