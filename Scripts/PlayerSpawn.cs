using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {

    public GameObject[] playerModels;
    public GameObject playerSpawn;
    public static int modelNumber;

	// Use this for initialization
	void Start () {
        Instantiate(playerModels[modelNumber], playerSpawn.transform.position, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
