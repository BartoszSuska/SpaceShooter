using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour {

    public static int health;
    private int numOfHp;

    public Image[] hp;
    public Sprite fullHp;
    public Sprite emptyHp;
    public GameObject[] hpImages;

	// Use this for initialization
	void Start () {
        health = 3;
        numOfHp = 3;

    }
	
	// Update is called once per frame
	void Update () {
        hpImages[0] = GameObject.Find("HP1");
        hpImages[1] = GameObject.Find("HP2");
        hpImages[2] = GameObject.Find("HP3");
        hp[0] = hpImages[0].GetComponent<Image>();
        hp[1] = hpImages[1].GetComponent<Image>();
        hp[2] = hpImages[2].GetComponent<Image>();
        if (health > numOfHp)
        {
            health = numOfHp;
        }

        for (int i = 0; i < hp.Length; i++)
        {
            if (i < health)
            {
                hp[i].sprite = fullHp;
            }
            else
                hp[i].sprite = emptyHp;
        }

        if (health <= 0)
        {
            SceneManager.LoadScene(0);
        }

	}

    void OnTriggerEnter2D(Collider2D enemy)
    {
        if(enemy.gameObject.tag == "Enemy")
        {
            health--;
            Destroy(enemy.gameObject);
        }

        if (enemy.gameObject.tag == "BossWeapon")
        {
            health--;
            Destroy(enemy.gameObject);
        }
    }
}
