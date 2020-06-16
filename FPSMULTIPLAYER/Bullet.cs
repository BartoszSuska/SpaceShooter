using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    public float speed;
    public GameObject bullethole;
    public int damage;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.layer == 8)
        {
            speed = 0;

            //hole instantiate
            ContactPoint contact = col.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            Instantiate(bullethole, pos, rot);

            Destroy(this.gameObject);
        }

        else if(col.gameObject.layer == 11)
        {
            col.transform.root.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, damage, PhotonNetwork.LocalPlayer.ActorNumber);
            Destroy(this.gameObject);
        }

        else if(col.gameObject.layer == 9)
        {
            Destroy(this.gameObject);
        }
    }
}
