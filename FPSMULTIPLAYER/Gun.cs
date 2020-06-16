using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.BoarShroom.Prototype
{
    [CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
    public class Gun : ScriptableObject
    {
        public string name;
        public GameObject prefab;
        public GameObject display;
        public float firerate;
        public float aimSpeed;
        public float bloom;
        public float recoil;
        public float kickback;
        public float bloomAim;
        public int damage;
        public int ammo;
        public int magazineSize;
        public float reloadTime;
        public float equipTime;
        public int burst; //0 semi | 1 auto | 2+ burst fire
        [Range(0, 1)] public float mainFOV;
        [Range(0, 1)] public float weaponFOV;
        public AudioClip gunshotSound;
        public float pitchRandomization;
        public float shotVolume;
        public int bullets;
        public bool recovery;
        public bool scope;
        public float shootTime;
        public GameObject PS_bullet;
        public GameObject Muzzle;

        private int magazine; //current magazine
        private int stash; //current ammo

        public void Initialize()
        {
            stash = ammo;
            magazine = magazineSize;
        }

        public bool FireBullet()
        {
            if (magazine > 0)
            {
                magazine -= 1;
                Debug.Log(magazine);
                return true;
            }
            else return false;
        }

        public void Reload()
        {
            stash += magazine;
            magazine = Mathf.Min(magazineSize, stash);
            stash -= magazine;
        }

        public int GetStash()
        {
            return stash;
        }

        public int GetMagazine()
        {
            return magazine;
        }
    }
}

