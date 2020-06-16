using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.BoarShroom.Prototype
{
    public class NetworkPlayer : MonoBehaviourPun, IPunObservable
    {
        public Player Player;
        public Vector3 RemotePlayerPosition;

        // Start is called before the first frame update
        void Start()
        {
            if(!photonView.IsMine)
            {
                
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine)
                return;

            var LagDistance = RemotePlayerPosition - transform.position;

            if(LagDistance.magnitude > 5f)
            {
                transform.position = RemotePlayerPosition;
                LagDistance = Vector3.zero;
            }
            else if(LagDistance.magnitude < 0.11f)
            {
                
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(stream.IsWriting)
            {
                stream.SendNext(transform.position);
            }
            else
            {
                RemotePlayerPosition = (Vector3)stream.ReceiveNext();
            }
        }
    }
}
