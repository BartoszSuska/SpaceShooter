using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.BoarShroom.Prototype
{
    public class GunLibrary : MonoBehaviour
    {
        public Gun[] allGuns;
        public static Gun[] guns;

        void Awake()
        {
            guns = allGuns;
        }

        public static Gun FindGun(string name)
        {
            foreach(Gun a in guns)
            {
                if (a.name.Equals(name)) return a;
            }

            return guns[0];
        }
    }
}

