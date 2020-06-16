using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBackground : MonoBehaviour {

    public GameObject Background;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Background")
        {
            GameObject BG = Instantiate(Background) as GameObject;
            BG.transform.position = new Vector2(transform.position.x, transform.position.y);
        }
    }
}
