using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.BoarShroom.GameJam
{
    public class Bullet : MonoBehaviour
    {
        public int Damage;
        public float Speed;

        public bool playerBullet;

        GameObject player;
        GameObject manager;
        GameObject cam;

        void Start()
        {
            player = GameObject.Find("Player");
            manager = GameObject.Find("Manager");
            cam = GameObject.Find("Main Camera");
            if(!playerBullet)
            {
                transform.LookAt(player.transform.position + new Vector3(0f, 1f, 0f));
            }
        }

        void Update()
        {
            if(playerBullet)
            {
                transform.Translate(Vector3.forward * Speed * Time.deltaTime * (-1f));
            }
            else if(!playerBullet)
            {
                transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            }
            
        }

        void OnTriggerEnter(Collider col)
        {
            if(col.gameObject.tag == "Enemy" && playerBullet)
            {
                manager.GetComponent<Manager>().kill.Play();
                if (player.GetComponent<Player>().startTimeBtwAttack > 0.5f) { player.GetComponent<Player>().startTimeBtwAttack -= 0.5f; }
                if (player.GetComponent<Player>().agent.speed <= 15f)
                {
                    player.GetComponent<Player>().agent.speed += 0.5f;
                    cam.GetComponent<MainCamera>().posYmax += 0.2f;
                }
                if (manager.GetComponent<Manager>().spawnTimeStart > 1f) { manager.GetComponent<Manager>().spawnTimeStart--; }
                manager.GetComponent<Manager>().kills++;
                Destroy(col.gameObject);
            }
            else if(col.gameObject.tag == "Player" && !playerBullet)
            {
                manager.GetComponent<Manager>().death.Play();
                //Destroy(col.gameObject);
                Destroy(gameObject);
            }
            else if(col.gameObject.tag == "Wall")
            {
                Destroy(gameObject);
            }
        }
    }
}
