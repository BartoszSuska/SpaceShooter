using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    public GameObject[] weaponObject;
    public Transform shotPointLeft;
    public Transform shotPointMiddle;
    public Transform shotPointRight;
    public static int weapon;

    private float timeBtwShots;
    public float startTimeBtwShots1;
    public float startTimeBtwShots2;
    public float startTimeBtwShots3;
    public float startTimeBtwShots4;
    public static float powerUpLeft;

    // Use this for initialization
    void Start () {
        weapon = 1;
	}
	
	// Update is called once per frame
	void Update () {
        if (timeBtwShots <= 0)
        {
            if (weapon == 1)
            {
                Instantiate(weaponObject[0], shotPointLeft.position, shotPointLeft.rotation);
                Instantiate(weaponObject[0], shotPointRight.position, shotPointRight.rotation);
                timeBtwShots = startTimeBtwShots1;
            }

            else if (weapon == 2)
            {
                Instantiate(weaponObject[1], shotPointLeft.position, shotPointLeft.rotation);
                Instantiate(weaponObject[1], shotPointRight.position, shotPointRight.rotation);
                timeBtwShots = startTimeBtwShots2;
            }

            else if (weapon == 3)
            {
                Instantiate(weaponObject[2], shotPointLeft.position, Quaternion.Euler(new Vector3(0, 0, 25)));
                Instantiate(weaponObject[2], shotPointMiddle.position, shotPointMiddle.rotation);
                Instantiate(weaponObject[2], shotPointRight.position, Quaternion.Euler(new Vector3(0, 0, -25)));
                timeBtwShots = startTimeBtwShots3;
            }

            else if (weapon == 4)
            {
                Instantiate(weaponObject[3], shotPointLeft.position, shotPointLeft.rotation);
                Instantiate(weaponObject[3], shotPointRight.position, shotPointRight.rotation);
                timeBtwShots = startTimeBtwShots4;
            }
        }
        else
            timeBtwShots -= Time.deltaTime;

        if (powerUpLeft > 0)
        {
            powerUpLeft -= Time.deltaTime;
        }
        else
            weapon = 1;
    }
}
