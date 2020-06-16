using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

    Text Score;
    public static float scoreCounter;
    public static bool boss;
    public static float bossTime;
    public static bool bossAlive;

	// Use this for initialization
	void Start () {
        Score = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if(HealthSystem.health <= 0)
        {
            SaveSystem.SaveScore(this);
        }
        if(!bossAlive)
        {
            if (!boss)
            {
                bossTime += Time.deltaTime;
                scoreCounter += Time.deltaTime;
            }

            if ((int)bossTime == 10)
            {
                boss = true;
            }
        }

        Score.text = "Score: " + scoreCounter.ToString("F0");
	}
}
