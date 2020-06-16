using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Com.BoarShroom.GameJam
{
    public class EnemyShooter : MonoBehaviour
    {
        NavMeshAgent agent;
        GameObject Player;
        Animator anim;
        Manager manager;

        public GameObject muzzle;
        public GameObject bullet;
        public Transform firePoint;
        public AudioSource shot;
        float timeBtwAttack;

        float speed;

        void Start()
        {
            manager = GameObject.Find("Manager").GetComponent<Manager>();
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            Player = GameObject.Find("Player");

            agent.speed = 1f + Player.GetComponent<NavMeshAgent>().speed / 3;
            timeBtwAttack = Player.GetComponent<Player>().startTimeBtwAttack + 0.5f;
        }

        void Update()
        {
            agent.destination = Player.transform.position;
            anim.SetFloat("Speed", agent.speed);

            transform.LookAt(Player.transform.position);

            if (timeBtwAttack <= 0)
            {
                GameObject MuzzlePrefab = Instantiate(muzzle, firePoint.position, Quaternion.identity);
                MuzzlePrefab.transform.LookAt(Player.transform.position);
                GameObject BulletPrefab = Instantiate(bullet, firePoint.position, Quaternion.identity);
                BulletPrefab.GetComponent<Bullet>().Speed = Player.GetComponent<Player>().agent.speed * 0.5f;
                BulletPrefab.transform.LookAt(Player.transform.position);
                BulletPrefab.GetComponent<Bullet>().playerBullet = false;
                timeBtwAttack = Player.GetComponent<Player>().startTimeBtwAttack + 0.5f;
                shot.Play();
            }
            else
            {
                timeBtwAttack -= Time.deltaTime;
            }
        }
    }
}
