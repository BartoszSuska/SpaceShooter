using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public float speed;
    public float downSpeed;
    public float timeDownSpeed;
    private bool right;
    public float health;
    private float timeBtwShot;
    public float startTimeBtwShot;
    public GameObject weapon1;
    public GameObject DestroyParticle;

    public Transform shotPointLeft;
    public Transform shotPointRight;
    private bool rightShot;

    private Vector2 screenBounds;
    public GameObject Diamond;

    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Update()
    {
        if(timeDownSpeed > 0)
        {
            transform.Translate(Vector2.down * downSpeed * Time.deltaTime);
            timeDownSpeed -= Time.deltaTime;
        }

        if (timeBtwShot <= 0)
        {
            if (rightShot)
            {
                Instantiate(weapon1, shotPointRight.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                rightShot = false;
                timeBtwShot = startTimeBtwShot;
            }
            else
            {
                Instantiate(weapon1, shotPointLeft.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                rightShot = true;
                timeBtwShot = startTimeBtwShot;
            }
        }
        else
        {
            timeBtwShot -= Time.deltaTime;
        }

        if(right)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        if(health <= 0)
        {
            Instantiate(DestroyParticle, transform.position, Quaternion.identity);
            spawnDiamond();
            Destroy(this.gameObject);
            ScoreScript.bossAlive = false;
        }
    }

    private void GiveDiamond()
    {

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    void switchDir()
    {
        if (right)
            right = false;
        else
            right = true;
    }
       

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "BossBorder")
        {
            switchDir();
        }
    }

    private void spawnDiamond()
    {
        GameObject diamond = Instantiate(Diamond) as GameObject;
        diamond.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y * 1);
    }
}
