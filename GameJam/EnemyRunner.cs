using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Com.BoarShroom.GameJam
{
    public class EnemyRunner : MonoBehaviour
    {
        NavMeshAgent agent;
        GameObject Player;
        Animator anim;
        Manager manager;

        float speed;

        void Start()
        {
            manager = GameObject.Find("Manager").GetComponent<Manager>();
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            Player = GameObject.Find("Player");
            
            agent.speed = 1f + Player.GetComponent<NavMeshAgent>().speed / 2;
        }

        void Update()
        {
            agent.destination = Player.transform.position;
            anim.SetFloat("Speed", agent.speed);
        }
    }
}
