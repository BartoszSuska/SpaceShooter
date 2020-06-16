using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOnce : MonoBehaviour {

    public float timeBeforeDestroy;

	// Use this for initialization
	void Start () {


    }
	
	// Update is called once per frame
	void Update () {
        timeBeforeDestroy -= Time.deltaTime;
        if(timeBeforeDestroy <= 0)
        {
            Destroy(this.gameObject);
        }
	}
}
