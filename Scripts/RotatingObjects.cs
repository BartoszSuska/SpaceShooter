using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObjects : MonoBehaviour {

    private float rotatingSpeed;

    void Awake()
    {
        rotatingSpeed = Random.Range(0.3f, 1.5f);
    }

	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 0, rotatingSpeed);
	}
}
