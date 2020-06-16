using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.BoarShroom.Prototype
{
    public class Sway : MonoBehaviourPunCallbacks
    {
        #region Variables

        public float intensity;
        public float smooth;
        public bool isMine;

        Quaternion origin_rotation;

        #endregion

        #region MonoBehaviour CallBacks

        void Start()
        {
            origin_rotation = transform.localRotation;
        }

        void Update()
        {
            if (Pause.paused) return;
            UpdateSway();
        }

        #endregion

        #region Private Methods

        void UpdateSway()
        {
            //controls
            float t_xMouse = Input.GetAxis("Mouse X");
            float t_yMouse = Input.GetAxis("Mouse Y");

            if(!isMine)
            {
                t_xMouse = 0;
                t_yMouse = 0;
            }

            //calculate target rotation
            Quaternion t_adjX = Quaternion.AngleAxis(-intensity * t_xMouse, Vector3.up);
            Quaternion t_adjY = Quaternion.AngleAxis(intensity * t_yMouse, Vector3.right);
            Quaternion target_rotation = origin_rotation * t_adjX * t_adjY;

            //rotate towards target rotation
            transform.localRotation = Quaternion.Lerp(transform.localRotation, target_rotation, Time.deltaTime * smooth);
        }

        #endregion
    }
}

