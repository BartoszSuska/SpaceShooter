using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.BoarShroom.Prototype
{
    public class Pickup : MonoBehaviourPunCallbacks
    {
        public Gun weapon;
        public float cooldown;
        public List<GameObject> targets;
        public GameObject gunDisplay;

        bool isDisabled;
        float wait;

        void Start()
        {
            foreach (Transform t in gunDisplay.transform) Destroy(t.gameObject);

            GameObject newDisplay = Instantiate(weapon.display, gunDisplay.transform.position, gunDisplay.transform.rotation) as GameObject;
            newDisplay.transform.SetParent(gunDisplay.transform);
        }

        void Update()
        {
            if(isDisabled)
            {
                if (wait >= 0)
                {
                    wait -= Time.deltaTime;
                }
                else
                {
                    Enable();
                }
            }

        }

        void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody == null) return;
            if(other.attachedRigidbody.gameObject.tag.Equals("Player"))
            {
                Weapons weaponController = other.attachedRigidbody.gameObject.GetComponent<Weapons>();
                weaponController.photonView.RPC("PickupWeapon", RpcTarget.All, weapon.name);
                photonView.RPC("Disable", RpcTarget.All);
            }
        }

        [PunRPC]
        public void Disable()
        {
            isDisabled = true;
            wait = cooldown;

            foreach (GameObject a in targets) a.SetActive(false);
        }

        void Enable()
        {
            isDisabled = false;
            wait = 0;

            foreach (GameObject a in targets) a.SetActive(true);
        }
    }
}

