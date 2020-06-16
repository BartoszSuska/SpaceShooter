using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.BoarShroom.GameJam
{
    public class EnemyTower : MonoBehaviour
    {
        public GameObject muzzle;
        public GameObject bullet;
        public Transform firePoint;
        public AudioSource shot;

        GameObject Player;

        float timeBtwAttack;

        void Start()
        {
            Player = GameObject.Find("Player");
            timeBtwAttack = Player.GetComponent<Player>().startTimeBtwAttack + 0.5f;
        }

        void Update()
        {
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
