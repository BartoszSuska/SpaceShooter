using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {

    public int weapon;
    private int speed;
    private float powerUpLength = 5f;

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
            PlayerShooting.weapon = weapon;
            PlayerShooting.powerUpLeft = powerUpLength;
            Destroy(this.gameObject);
        }
    }

}
