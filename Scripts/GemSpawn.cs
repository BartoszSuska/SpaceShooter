using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawn : MonoBehaviour
{

    public GameObject[] powerUps;
    private Vector2 screenBounds;

    private float timeBtwSpawn1;

    private int item;

    // Use this for initialization
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    private void spawn1(int randomPowerUp)
    {
        GameObject powerUp = Instantiate(powerUps[randomPowerUp]) as GameObject;
        powerUp.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y * 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBtwSpawn1 <= 0)
        {
            int whichPowerUp = Random.Range(1, 100);
            if (whichPowerUp <= 25)
            {
                item = 0;
            }
            else if(whichPowerUp <= 50)
            {
                item = 1;
            }
            else if(whichPowerUp <= 75)
            {
                item = 2;
            }
            else if (whichPowerUp <= 100)
            {
                item = 3;
            }
            spawn1(item);
            timeBtwSpawn1 = Random.Range(3f, 8f);
        }
        else
        {
            timeBtwSpawn1 -= Time.deltaTime;
        }
    }
}
