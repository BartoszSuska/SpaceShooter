using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Com.BoarShroom.GameJam
{
    public class Manager : MonoBehaviour
    {
        public AudioSource kill;
        public AudioSource death;
        public AudioSource shotPlayer;
        public AudioSource shotEnemy;
        public GameObject EndGameCanvas;

        public TMP_Text killCounter;
        public int kills;

        public GameObject[] spawns;
        public GameObject[] enemies;
        public GameObject spawnEffect;

        public float spawnTimeStart;
        float spawnTime;

        Transform spawnPos;
        GameObject spawnEffectPrefab;
        int spawnEffectNumber;

        void Start()
        {
            spawnTimeStart = 8;
            spawnTime = spawnTimeStart;
        }

        void Update()
        {
            killCounter.text = kills.ToString();

            if(spawnTime <= 0)
            {
                Destroy(spawnEffectPrefab);
                spawnEffectNumber--;
                Spawn();
            }
            else
            {
                if(spawnEffectNumber == 0)
                {
                    spawnPos = spawns[Random.Range(0, spawns.Length)].transform;
                    spawnEffectPrefab = Instantiate(spawnEffect, spawnPos.position, Quaternion.identity);
                    spawnEffectNumber++;
                }
                spawnTime -= Time.deltaTime;
            }

            GameObject Player = GameObject.Find("Player");
            if(!Player)
            {
                EndGameCanvas.SetActive(true);
                EndGameCanvas.GetComponent<EndGame>().HighScoreSet();
            }
            else
            {
                EndGameCanvas.SetActive(false);
            }
        }

        void Spawn()
        {
            

            Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPos.position, Quaternion.identity);
            
            spawnTime = spawnTimeStart;
        }
    }
}
