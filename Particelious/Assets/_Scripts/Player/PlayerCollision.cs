using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerCollision : MonoBehaviour {

    private int HitCounter = 0; // Just for debugging

    public UnityEvent OnHit;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            OnHit.Invoke();
            HitCounter++;
        }
    }
}
