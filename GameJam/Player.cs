using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Com.BoarShroom.GameJam
{
    public class Player : MonoBehaviour
    {
        public AudioSource shot;
        public GameObject[] bullets;
        public GameObject[] muzzle;
        float timeBtwAttack;
        public float startTimeBtwAttack;

        public Transform firePoint;

        public NavMeshAgent agent;
        Animator anim;
        Manager manager;


        void Start()
        {
            manager = GameObject.Find("Manager").GetComponent<Manager>();
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            agent.speed = 3f;
            timeBtwAttack = startTimeBtwAttack;
        }

        void Update()
        {
            //agent.speed = Mathf.Min(3f + manager.kills, 20);
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 move = new Vector3(horizontal, 0f, vertical);
            move.Normalize();
            Vector3 moveDestination = move * Time.deltaTime * agent.speed;
            transform.Translate(moveDestination, Space.World);
            anim.SetFloat("Speed", agent.speed);

            if(horizontal != 0 || vertical != 0)
            {
                transform.rotation = Quaternion.LookRotation(move);
            }
            
            if(timeBtwAttack <= 0)
            {
                GameObject muzzlePrefab = Instantiate(muzzle[0], firePoint.position, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
                GameObject bulletPrefab = Instantiate(bullets[0], firePoint.position, transform.rotation);
                bulletPrefab.GetComponent<Bullet>().Speed = agent.speed * 1.5f;
                bulletPrefab.GetComponent<Bullet>().playerBullet = true;
                timeBtwAttack = startTimeBtwAttack;
                Destroy(muzzlePrefab, 3f);
                shot.Play();
            }
            else
            {
                timeBtwAttack -= Time.deltaTime;
            }
        }
    }
}
