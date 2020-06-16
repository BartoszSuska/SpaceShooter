using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.BoarShroom.Prototype
{
    public class PlayerVisual : MonoBehaviour
    {
        #region Variables

        public GameObject playerDesign;
        public Transform head;
        public Texture[] colors;
        public GameObject[] masks;

        #endregion

        #region MonoBehaviour CallBacks

        void Start()
        {
            Color();
            Mask();
        }

        #endregion

        #region Methods

        public void Color()
        {
            int textureNumber = Random.Range(0, colors.Length);
            playerDesign.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.SetTexture("_MainTexture", colors[textureNumber]);
        }

        public void Mask()
        {
            GameObject mask = Instantiate(masks[Random.Range(0, masks.Length)], head.position, head.rotation, head);
        }
        


        #endregion
    }
}

