using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBigScript : MonoBehaviour {

    private float speed;

    // Use this for initialization
    void Start()
    {
        speed = 2;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

}
