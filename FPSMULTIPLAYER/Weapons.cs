using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

namespace Com.BoarShroom.Prototype
{
    public class Weapons : MonoBehaviourPunCallbacks
    {
        #region Variables

        public List<Gun> loadout;
        public Gun currentGunData;
        public Transform weaponParent;
        public GameObject bulletholePrefab;
        public LayerMask canBeShot;
        public bool isAiming;
        public AudioClip hitmarkerSound;
        public AudioSource sfx;
        public float hitmarkerWaitStart;
        public ParticleSystem PS_Blood;
        public GameObject scope;
        public Camera weaponCam;

        float currentCooldown;
        int currentIndex;
        bool isReloading;
        bool isEquiping;
        Image hitmarker;
        float hitmarkerWait;

        GameObject currentWeapon;

        #endregion

        #region MonoBehaviour CallBacks

        void Start()
        {
            if(photonView.IsMine)
            {
                foreach (Gun a in loadout) a.Initialize();
            }
            
            hitmarker = GameObject.Find("HUD/Hitmarker/Image").GetComponent<Image>();
            hitmarker.color = new Color(1, 1, 1, 0);
            Equip(0);
        }

        void Update()
        {
            if (photonView.IsMine && Pause.paused) return;

            if (photonView.IsMine && Input.GetKeyDown(KeyCode.Alpha1) && !isAiming) { photonView.RPC("Equip", RpcTarget.All, 0); }
            if (photonView.IsMine && Input.GetKeyDown(KeyCode.Alpha2) && !isAiming) { photonView.RPC("Equip", RpcTarget.All, 1); }

            if (currentWeapon != null)
            {
                if(photonView.IsMine)
                {

                    //Aim(Input.GetMouseButton(1));

                    if(loadout[currentIndex].burst != 1)
                    {
                        if (Input.GetMouseButtonDown(0) && currentCooldown <= 0 && !isReloading && !isEquiping)
                        {
                            if (loadout[currentIndex].FireBullet())
                            {
                                photonView.RPC("Shoot", RpcTarget.All);                                                                
                            }
                            else photonView.RPC("ReloadRPC", RpcTarget.All);
                        }
                    }
                    else if(loadout[currentIndex].burst == 1)
                    {
                        if (Input.GetMouseButton(0) && currentCooldown <= 0 && !isReloading && !isEquiping)
                        {
                            if (loadout[currentIndex].FireBullet())
                            {
                                photonView.RPC("Shoot", RpcTarget.All);
                            }
                            else photonView.RPC("ReloadRPC", RpcTarget.All);
                        }
                    }

                    if(Input.GetKeyDown(KeyCode.R) && !isReloading && !isEquiping)
                    {
                        photonView.RPC("ReloadRPC", RpcTarget.All);
                    }

                    //cooldown
                    if (currentCooldown > 0)
                    {
                        currentCooldown -= Time.deltaTime;
                    }
                }

                //weapon position elasticity
                currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
            }

            if(photonView.IsMine)
            {
                if(hitmarkerWait > 0)
                {
                    hitmarkerWait -= Time.deltaTime;
                }
                else
                {
                    hitmarker.color = Color.Lerp(hitmarker.color, new Color(1, 1, 1, 0), Time.deltaTime * hitmarkerWaitStart);
                }
            }
        }

        #endregion

        #region Private Methods

        [PunRPC]
        private void ReloadRPC()
        {
            StartCoroutine(Reload(loadout[currentIndex].reloadTime));
        }

        IEnumerator Reload(float p_wait)
        {
            isReloading = true;
            currentWeapon.GetComponent<Animator>().Play("Reload", 0, 0);
            yield return new WaitForSeconds(p_wait);
            loadout[currentIndex].Reload();
            isReloading = false;
        }

        IEnumerator Equip(float p_wait)
        {
            isEquiping = true;
            currentWeapon.GetComponent<Animator>().Play("Equip", 0, 0);
            yield return new WaitForSeconds(p_wait);
            isEquiping = false;
        }

        IEnumerator ScopeShoot(float p_wait)
        {
            currentWeapon.GetComponent<Animator>().Play("Shoot", 0, 0);
            yield return new WaitForSeconds(p_wait);
        }

        [PunRPC]
        void Equip(int p_id)
        {
            if (currentWeapon != null)
            {
                StopCoroutine("Reload");
                StopCoroutine("Equip");
                Destroy(currentWeapon);
            }

            currentIndex = p_id;

            GameObject t_newWeapon = Instantiate(loadout[p_id].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
            t_newWeapon.transform.localPosition = Vector3.zero;
            t_newWeapon.transform.localEulerAngles = Vector3.zero;
            t_newWeapon.GetComponent<Sway>().isMine = photonView.IsMine;

            if (photonView.IsMine) ChangeLayersRecursively(t_newWeapon, 10);
            else ChangeLayersRecursively(t_newWeapon, 0);

            currentWeapon = t_newWeapon;
            currentGunData = loadout[p_id];

            StartCoroutine(Equip(loadout[currentIndex].equipTime));
        }

        void ChangeLayersRecursively(GameObject p_target, int p_layer)
        {
            p_target.layer = p_layer;
            foreach (Transform a in p_target.transform) ChangeLayersRecursively(a.gameObject, p_layer);
        }

        [PunRPC]
        void PickupWeapon(string name)
        {
            Gun newGun = GunLibrary.FindGun(name);
            newGun.Initialize();

            if (loadout.Count >= 2)
            {
                if(loadout[1] == newGun)
                {
                    loadout[1] = newGun;
                }
                else if(loadout[1] != newGun)
                {
                    loadout[1] = newGun;
                    if(currentIndex == 1)
                    {
                        Equip(currentIndex);
                    }
                }
                
            }
            else
            {
                loadout.Add(newGun);
            }

            scope.SetActive(false);
            weaponCam.enabled = true;
        }

        public void Aim(bool p_isAiming)
        {
            if (!currentWeapon || isEquiping) return;

            Transform t_anchor = currentWeapon.transform.Find("Anchor");
            Transform t_state_ADS = currentWeapon.transform.Find("States/ADS");
            Transform t_state_hip = currentWeapon.transform.Find("States/Hip");

            if (p_isAiming)
            {
                //aim
                t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_ADS.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
                isAiming = true;
                if(currentGunData.scope)
                {
                    scope.SetActive(true);
                    weaponCam.enabled = false;
                }
            }
            else
            {
                //hip
                t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_hip.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
                isAiming = false;
                if(currentGunData.scope)
                {
                    scope.SetActive(false);
                    weaponCam.enabled = true;
                }
            }
        }

        //IEnumerator HideWeaponWithScope(GameObject p_target, bool decision)
        //{
        //    if(p_target.GetComponent<MeshRenderer>())
        //    {
        //        p_target.GetComponent<MeshRenderer>().enabled = decision;
        //    }
        //    foreach (Transform a in p_target.transform) HideWeaponWithScope(a.gameObject, decision);
        //}

        [PunRPC]
        void Shoot()
        {
            Transform t_spawn = transform.Find("Cameras/Normal Camera").transform;

            //cooldwon
            currentCooldown = loadout[currentIndex].firerate;

            for (int i = 0; i < Mathf.Max(1, currentGunData.bullets); i++)
            {

                //bloom
                Vector3 t_bloom = t_spawn.position + t_spawn.forward * 1000f;
                if (Input.GetMouseButton(1))
                {
                    t_bloom += Random.Range(-loadout[currentIndex].bloomAim, loadout[currentIndex].bloomAim) * t_spawn.up;
                    t_bloom += Random.Range(-loadout[currentIndex].bloomAim, loadout[currentIndex].bloomAim) * t_spawn.right;
                }
                else
                {
                    t_bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * t_spawn.up;
                    t_bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * t_spawn.right;
                }
                t_bloom -= t_spawn.position;
                t_bloom.Normalize();


                //bullet
                if(photonView.IsMine)
                {
                    Vector3 bulletPosition = currentWeapon.transform.Find("Anchor/Design/Model/Barrel").position;
                    Instantiate(currentGunData.PS_bullet, bulletPosition, Quaternion.LookRotation(t_bloom));
                }


                //muzzle
                Transform barrel = currentWeapon.transform.Find("Anchor/Design/Model/Barrel");
                GameObject t_ps_barrel = Instantiate(currentGunData.Muzzle, barrel.position, Quaternion.LookRotation(t_bloom));
                Destroy(t_ps_barrel.gameObject, 8f);



                //raycast
                RaycastHit t_hit = new RaycastHit();
                if (Physics.Raycast(t_spawn.position, t_bloom, out t_hit, 1000f, canBeShot))
                {
                    

                    if (t_hit.collider.gameObject.layer == 8)
                    {
                        GameObject t_newBulletHole = Instantiate(bulletholePrefab, t_hit.point + t_hit.normal * 0.001f, Quaternion.identity) as GameObject;
                        t_newBulletHole.transform.LookAt(t_hit.point + t_hit.normal);
                        Destroy(t_newBulletHole, 5f);
                    }

                    if (photonView.IsMine)
                    {
                        //shooting other player in network
                        if (t_hit.collider.gameObject.layer == 11)
                        {
                            //t_hit.collider.transform.root.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[currentIndex].damage, PhotonNetwork.LocalPlayer.ActorNumber);
                            //hitmarker.color = Color.white;
                            sfx.PlayOneShot(hitmarkerSound);
                            //hitmarkerWait = hitmarkerWaitStart;
                            ParticleSystem t_ps_blood = Instantiate(PS_Blood, t_hit.point + t_hit.normal * 0.001f, Quaternion.identity);
                            t_ps_blood.transform.LookAt(t_hit.point + t_hit.normal);
                            t_ps_blood.Play();
                            Destroy(t_ps_blood, 15f);
                        }
                    }
                }
            }




            //sound
            sfx.clip = currentGunData.gunshotSound;
            sfx.pitch = 1 - currentGunData.pitchRandomization + Random.Range(-currentGunData.pitchRandomization, currentGunData.pitchRandomization);
            sfx.volume = currentGunData.shotVolume;
            sfx.Play();

            //gun fx
            currentWeapon.transform.Rotate(-loadout[currentIndex].recoil, 0, 0);
            currentWeapon.transform.position -= currentWeapon.transform.forward * loadout[currentIndex].kickback;

            //animation
            if(!currentGunData.scope)
            {
                currentWeapon.GetComponent<Animator>().Play("Shoot", 0, 0);
            }
            else if (currentGunData.scope)
            {
                StartCoroutine(ScopeShoot(loadout[currentIndex].shootTime));
            }

        }

        [PunRPC]
        private void TakeDamage(int p_dmg, int p_actor)
        {
            GetComponent<Player>().TakeDamage(p_dmg, p_actor);
        }

        #endregion

        #region Public Methods

        public void RefreshAmmo(TMP_Text p_text)
        {
            int t_magazine = loadout[currentIndex].GetMagazine();
            int t_stash = loadout[currentIndex].GetStash();

            p_text.text = t_magazine.ToString("D2") + " / " + t_stash.ToString("D2");
        }

        #endregion
    }
}

