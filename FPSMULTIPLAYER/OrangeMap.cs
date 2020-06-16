using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OrangeMap : MonoBehaviour
{

    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.InstantiateSceneObject("CCTV", Vector3.zero, Quaternion.identity, 0);
        }
        
    }
}
