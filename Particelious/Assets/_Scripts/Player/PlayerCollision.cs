using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerCollision : MonoBehaviour {

    private int HitCounter = 0; // Just for debugging

	// Use this for initialization
	void Start () {
	}

    private void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HitCounter++;
            Destroy(collision.gameObject);
        }
    }
}
