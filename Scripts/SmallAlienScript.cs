using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAlienScript : MonoBehaviour {

    private int speed;

	// Use this for initialization
	void Start () {
        speed = 4;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.down * speed * Time.deltaTime);        
    }
}
