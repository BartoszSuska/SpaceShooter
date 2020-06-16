using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour {

    public static float lastGameDiamonds;
    public float speed;
    public AudioSource pickUpAudio;

	// Use this for initialization
	void Start () {
        GameObject Audio = GameObject.Find("/AudioEffects/DiamondsAudio");
        pickUpAudio = Audio.GetComponent<AudioSource>();
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
            lastGameDiamonds += 1;
            pickUpAudio.Play();
            Destroy(this.gameObject);
        }
    }
}
