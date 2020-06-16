using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor1Script : MonoBehaviour {

    private float speed;

	// Use this for initialization
	void Start () {
        speed = 4;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
	}

}
