using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Glowing : MonoBehaviour {

    private SpriteRenderer Renderer = null;
	// Use this for initialization
	void Start () {
        Renderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
