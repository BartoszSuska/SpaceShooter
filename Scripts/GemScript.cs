using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour {

    private int speed;
    public int score;

	// Use this for initialization
	void Start () {
        speed = 4;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Border")
        {
            Destroy(this.gameObject);
        }

        if (col.gameObject.tag == "Player")
        {
            ScoreScript.scoreCounter += score;
            Destroy(this.gameObject);
        }
    }
}
