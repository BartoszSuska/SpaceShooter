using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.BoarShroom.GameJam
{
    public class MainCamera : MonoBehaviour
    {
        public Transform Player;
        float posY;
        public float posYmax;

        void Start()
        {
            posY = posYmax;
        }

        void Update()
        {
            transform.position = new Vector3(Player.position.x, posY, Player.position.z);
            GetComponent<Camera>().orthographicSize = posY;

            if(posY < posYmax)
            {
                posY += 0.005f;
            }
        }
    }
}
