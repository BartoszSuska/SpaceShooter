using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

namespace Com.BoarShroom.Prototype
{
    public class Player : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Variables

        public float speed;
        public float jumpForce;
        public int maxHealth;
        public Camera normalCam;
        public Camera weaponCam;
        public GameObject cameraParent;
        public Transform weaponParent;
        public LayerMask ground;
        public Transform groundDetector;

        public GameObject standingCollider;
        public GameObject design;
        public ProfileData playerProfile;
        public TextMeshPro playerUsername;

        Vector3 originCamera;
        float baseFOV;
        float sprintFOVModifier = 1.5f;
        float movementCounter;
        float idleCounter;
        int currentHealth;
        float aimAngle;

        Transform ui_healthBar;
        TMP_Text ui_ammo;
        TMP_Text ui_username;

        Manager manager;
        Weapons weapon;

        Rigidbody rig;
        Vector3 weaponParentOrigin;
        Vector3 targetWeaponBobPosition;
        Vector3 weaponParentCurrentPosition;

        bool isAiming;

        Vector3 normalCamTarget;
        Vector3 weaponCamTarget;

        Animator anim;

        Vector3 networkPosition;
        Quaternion networkRotation;

        #endregion

        #region MonoBehaviour CallBacks

        void Start()
        {
            manager = GameObject.Find("Manager").GetComponent<Manager>();
            weapon = GetComponent<Weapons>();

            currentHealth = maxHealth;

            cameraParent.SetActive(photonView.IsMine);
            if (!photonView.IsMine)
            {
                gameObject.layer = 11;
                foreach (Transform a in design.transform) ChangeLayersRecursively(a.gameObject, 11);
            }
            baseFOV = normalCam.fieldOfView;
            originCamera = normalCam.transform.localPosition;
            rig = GetComponent<Rigidbody>();
            weaponParentOrigin = weaponParent.localPosition;
            weaponParentCurrentPosition = weaponParentOrigin;

            if (photonView.IsMine)
            {
                ui_healthBar = GameObject.Find("HUD/Health/Bar").transform;
                ui_ammo = GameObject.Find("HUD/Ammo/Text").GetComponent<TMP_Text>();
                RefreshHealthBar();
                anim = GetComponent<Animator>();
                ui_username = GameObject.Find("HUD/Username/Text").GetComponent<TMP_Text>();
                ui_username.text = Launcher.myProfile.username;

                photonView.RPC("SyncProfile",RpcTarget.All, Launcher.myProfile.username, Launcher.myProfile.level, Launcher.myProfile.xp);
            }

        }

        void Update()
        {
            if (!photonView.IsMine)
            {
                RefreshMultiplayerState();
                return;
            }
             
            //Axies
            float t_horizontal = Input.GetAxis("Horizontal");
            float t_vertical = Input.GetAxis("Vertical");

            //Controls
            bool jump = Input.GetKeyDown(KeyCode.Space);
            bool pause = Input.GetKeyDown(KeyCode.Escape);

            //States
            bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
            bool isJumping = jump && isGrounded;

            //pause
            if(pause)
            {
                GameObject.Find("Pause").GetComponent<Pause>().TogglePause();
            }

            if(Pause.paused)
            {
                t_horizontal = 0f;
                t_vertical = 0f;
                jump = false;
                pause = false;
                isGrounded = false;
                isJumping = false;
            }

            //Crouching

            //Jumping
            if (isJumping)
            {
                rig.AddForce(Vector3.up * jumpForce);
            }

            //Head Bob
            if(!isGrounded)
            {
                //air
                HeadBob(idleCounter, 0.01f, 0.01f);
                idleCounter += 0;
                weaponParent.localPosition = Vector3.MoveTowards(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 0.4f);
            }
            else if (t_horizontal != 0 || t_vertical != 0)
            {
                //walking
                HeadBob(movementCounter, 0.03f, 0.03f);
                movementCounter += Time.deltaTime * 3f;
                weaponParent.localPosition = Vector3.MoveTowards(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 1.6f);
            }
            else
            {
                //idle
                HeadBob(idleCounter, 0.01f, 0.01f);
                idleCounter += Time.deltaTime;
                weaponParent.localPosition = Vector3.MoveTowards(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 0.4f);
            }

            //UI refresh
            RefreshHealthBar();
            weapon.RefreshAmmo(ui_ammo);

        }

        void FixedUpdate()
        {
            if (!photonView.IsMine)
            {
                rig.position = Vector3.MoveTowards(rig.position, networkPosition, Time.fixedDeltaTime);
                rig.rotation = Quaternion.RotateTowards(rig.rotation, networkRotation, Time.fixedDeltaTime * 100.0f);
            }

            //Axies
            float t_horizontal = Input.GetAxis("Horizontal");
            float t_vertical = Input.GetAxis("Vertical");

            //Controls
            bool jump = Input.GetKeyDown(KeyCode.Space);
            bool aim = Input.GetMouseButton(1);

            //States
            bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
            bool isJumping = jump && isGrounded;
            isAiming = aim;

            //pause
            if (Pause.paused)
            {
                t_horizontal = 0f;
                t_vertical = 0f;
                jump = false;
                isGrounded = false;
                isJumping = false;
                isAiming = false;
            }

            //Movement
            Vector3 t_direction = Vector3.zero;
            float t_adjustedSpeed = speed;

            t_direction = new Vector3(t_horizontal, 0, t_vertical);
            t_direction.Normalize();
            t_direction = transform.TransformDirection(t_direction);

            Vector3 t_targetVelocity = t_direction * t_adjustedSpeed * Time.deltaTime;
            t_targetVelocity.y = rig.velocity.y;
            rig.velocity = t_targetVelocity;


            //aiming
            weapon.Aim(isAiming);

            //camera & weapon adjust
            if (isAiming)
            {
                normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * weapon.currentGunData.mainFOV, Time.deltaTime * 8f);
                weaponCam.fieldOfView = Mathf.Lerp(weaponCam.fieldOfView, baseFOV * weapon.currentGunData.weaponFOV, Time.deltaTime * 8f);
            }
            else
            {
                normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.deltaTime * 8f);
                weaponCam.fieldOfView = Mathf.Lerp(weaponCam.fieldOfView, baseFOV, Time.deltaTime * 8f);
            }

            //animations
            float t_anim_horizontal = 0f;
            float t_anim_vertical = 0f;

            if(isGrounded)
            {
                t_anim_horizontal = t_direction.x;
                t_anim_vertical = t_direction.z;
            }

            //anim.SetFloat("Horizontal", t_anim_horizontal);
            //anim.SetFloat("Vertical", t_anim_vertical);
        }

        #endregion

        #region Private Methods

        void ChangeLayersRecursively(GameObject p_target, int p_layer)
        {
            p_target.layer = p_layer;
            foreach (Transform a in p_target.transform) ChangeLayersRecursively(a.gameObject, p_layer);
        }

        void HeadBob(float p_z, float p_x_intensity, float p_y_intesity)
        {
            float t_aim_adjust = 1f;
            if (isAiming) t_aim_adjust = 0.1f;
            targetWeaponBobPosition = weaponParentCurrentPosition + new Vector3(Mathf.Cos(p_z) * p_x_intensity * t_aim_adjust, Mathf.Sin(p_z * 2) * p_y_intesity * t_aim_adjust, weaponParentOrigin.z);
        }

        void RefreshHealthBar()
        {
            float t_health = (float)currentHealth / (float)maxHealth;
            ui_healthBar.localScale = Vector3.Lerp(ui_healthBar.localScale, new Vector3(t_health, 1, 1), Time.deltaTime * 8f);
        }

        [PunRPC]
        void SyncProfile(string p_username, int p_level, int p_xp)
        {
            playerProfile = new ProfileData(p_username, p_level, p_xp);
            playerUsername.text = playerProfile.username;
        }

        void RefreshMultiplayerState()
        {
            float cacheEulY = weaponParent.localEulerAngles.y;
            Quaternion targetRotation = Quaternion.identity * Quaternion.AngleAxis(aimAngle, Vector3.right);
            weaponParent.rotation = Quaternion.Slerp(weaponParent.rotation, targetRotation, Time.deltaTime * 8f);

            Vector3 finalRotation = weaponParent.localEulerAngles;
            finalRotation.y = cacheEulY;

            weaponParent.localEulerAngles = finalRotation;
        }

        #endregion

        #region Public Methods

        public void TakeDamage(int p_dmg, int p_actor)
        {
            if(photonView.IsMine)
            {
                currentHealth -= p_dmg;
                RefreshHealthBar();
                if (currentHealth <= 0)
                {
                    manager.Spawn();
                    manager.ChangeStat_S(PhotonNetwork.LocalPlayer.ActorNumber, 1, 1);

                    if(p_actor >= 0)
                    {
                        manager.ChangeStat_S(p_actor, 0, 1);
                    }

                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }

        #endregion

        #region Photon Callbacks

        public void OnPhotonSerializeView(PhotonStream p_stream, PhotonMessageInfo p_message)
        {
            if(p_stream.IsWriting)
            {
                p_stream.SendNext((int)(weaponParent.transform.localEulerAngles.x * 100f));
                p_stream.SendNext(this.rig.position);
                p_stream.SendNext(this.rig.rotation);
                p_stream.SendNext(this.rig.velocity);
            }
            else
            {
                aimAngle = (int)p_stream.ReceiveNext() / 100f;
                networkPosition = (Vector3)p_stream.ReceiveNext();
                networkRotation = (Quaternion)p_stream.ReceiveNext();
                rig.velocity = (Vector3)p_stream.ReceiveNext();

                float lag = Mathf.Abs((float)(PhotonNetwork.Time - p_message.SentServerTimestamp));
                networkPosition += (this.rig.velocity * lag); 
            }
        }

        #endregion
    }
}

