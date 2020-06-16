using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {

    public GameObject[] Meteor;
    public GameObject[] MeteorBig;
    public GameObject[] SmallAlien;
    public GameObject[] MediumAlien;
    private Vector2 screenBounds;

    private float timeBtwSpawn1;
    public float startTimeBtwSpawn1;
    private float timeBtwSpawn2;
    public float startTimeBtwSpawn2;
    private float timeBtwSpawn3;
    public float startTimeBtwSpawn3;
    private float timeBtwSpawn4;
    public float startTimeBtwSpawn4;
    public GameObject Boss;


    // Use this for initialization
    void Start () {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
	}
	
    private void spawn1()
    {
        GameObject enemy = Instantiate(Meteor[Random.Range(0, 2)]) as GameObject;
        enemy.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y * 1);
    }

    private void spawn2()
    {
        GameObject enemy = Instantiate(MeteorBig[Random.Range(0, 2)]) as GameObject;
        enemy.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y * 1);
    }

    private void spawn3()
    {
        GameObject enemy = Instantiate(SmallAlien[0]) as GameObject;
        enemy.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y * 1);
    }

    private void spawn4()
    {
        GameObject enemy = Instantiate(MediumAlien[0]) as GameObject;
        enemy.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y * -1);
    }

    private void spawnBoss()
    {
        GameObject boss = Instantiate(Boss) as GameObject;
        boss.transform.position = new Vector2(0, screenBounds.y * 1);
    }

    // Update is called once per frame
    void Update () {

        if(!ScoreScript.bossAlive)
        {
            if (timeBtwSpawn1 <= 0)
            {
                spawn1();
                timeBtwSpawn1 = startTimeBtwSpawn1;
            }
            else
            {
                timeBtwSpawn1 -= Time.deltaTime;
            }

            if (ScoreScript.scoreCounter > 5)
            {
                if (timeBtwSpawn2 <= 0)
                {
                    spawn2();
                    timeBtwSpawn2 = startTimeBtwSpawn2;
                }
                else
                {
                    timeBtwSpawn2 -= Time.deltaTime;
                }
            }

            if (ScoreScript.scoreCounter > 25)
            {
                if (timeBtwSpawn3 <= 0)
                {
                    spawn3();
                    timeBtwSpawn3 = startTimeBtwSpawn3;
                }
                else
                {
                    timeBtwSpawn3 -= Time.deltaTime;
                }
            }

            if (ScoreScript.scoreCounter > 35)
            {
                if (timeBtwSpawn4 <= 0)
                {
                    spawn4();
                    timeBtwSpawn4 = startTimeBtwSpawn4;
                }
                else
                {
                    timeBtwSpawn4 -= Time.deltaTime;
                }
            }
        }


        if(ScoreScript.boss)
        {
            spawnBoss();
            ScoreScript.boss = false;
            ScoreScript.bossTime = 0;
            ScoreScript.bossAlive = true;
        }
    }
}
