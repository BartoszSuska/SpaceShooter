using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumAlienScript : MonoBehaviour {

    private float speed;

    // Use this for initialization
    void Start()
    {
        speed = 6;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}
