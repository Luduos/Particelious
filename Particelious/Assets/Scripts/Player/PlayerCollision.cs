using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerCollision : MonoBehaviour {

    [SerializeField] private Text DebugText;
    private int HitCounter = 0; // Just for debugging

	// Use this for initialization
	void Start () {
	}

    private void Update()
    {
        if(null != DebugText)
        {
            DebugText.text = "Time: " + (int) Time.time + " Hits: " + HitCounter;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Trigger");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HitCounter++;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HitCounter++;
        }
    }
}
