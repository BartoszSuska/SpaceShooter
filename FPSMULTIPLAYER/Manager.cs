using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using TMPro;

namespace Com.BoarShroom.Prototype
{
    public class PlayerInfo
    {
        public ProfileData profile;
        public int actor;
        public short kills;
        public short deaths;

        public PlayerInfo (ProfileData p, int a, short k, short d)
        {
            this.profile = p;
            this.actor = a;
            this.kills = k;
            this.deaths = d;
        }
    }

    public enum GameState
    {
        Waiting = 0,
        Starting = 1,
        Playing = 2,
        Ending = 3
    }

    public class Manager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        #region Fields

        public string player_prefab_string;
        public Transform[] spawn_points;
        public GameObject player_prefab;

        public List<PlayerInfo> playerInfo = new List<PlayerInfo>();
        public int myind;
        public int mainmenu = 0;
        public int killCount;
        public bool perpetual = false;
        public GameObject mapCam;

        Text ui_myKills;
        Text ui_myDeaths;
        Transform ui_Leaderboard;
        Transform ui_endgame;

        GameState state = GameState.Waiting;

        #endregion

        #region Codes

        public enum EventCodes : byte
        {
            NewPlayer,
            UpdatePlayers,
            ChangeStat,
            NewMatch
        }

        #endregion

        #region MB Callbacks

        void Start()
        {
            mapCam.SetActive(false);
            ValidateConnection();
            InitializeUI();
            NewPlayer_S(Launcher.myProfile);
            Spawn();
        }

        void Update()
        {
            if(state == GameState.Ending)
            {
                return;
            }

            if (Input.GetKey(KeyCode.Tab))
            {
                Leaderboard(ui_Leaderboard);
            }
            else if(!Input.GetKey(KeyCode.Tab))
            {
                ui_Leaderboard.gameObject.SetActive(false);
            }
        }

        void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        #endregion

        #region Photon

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code >= 200) return;

            EventCodes e = (EventCodes)photonEvent.Code;
            object[] o = (object[])photonEvent.CustomData;

            switch (e)
            {
                case EventCodes.NewPlayer:
                    NewPlayer_R(o);
                    break;

                case EventCodes.UpdatePlayers:
                    UpdatePlayers_R(o);
                    break;

                case EventCodes.ChangeStat:
                    ChangeStat_R(o);
                    break;
            }
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            SceneManager.LoadScene(mainmenu);
        }

        #endregion

        #region Methods

        public void Spawn()
        {
            Transform t_spawn = spawn_points[Random.Range(0, spawn_points.Length)];

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Instantiate(player_prefab_string, t_spawn.position, t_spawn.rotation);
            }
            else
            {
                Debug.Log("Working");
                GameObject newPlayer = Instantiate(player_prefab, t_spawn.position, t_spawn.rotation) as GameObject;
            }
            
        }

        void InitializeUI()
        {
            ui_Leaderboard = GameObject.Find("HUD").transform.Find("Leaderboard").transform;
            ui_endgame = GameObject.Find("Canvas").transform.Find("EndGame").transform;

            RefreshMyStats();
        }

        void RefreshMyStats()
        {
            
        }

        void Leaderboard (Transform p_lb)
        {
            for(int i = 2; i < p_lb.childCount; i++)
            {
                Destroy(p_lb.GetChild(i).gameObject);
            }

            //set header
            p_lb.Find("Header/Mode").GetComponent<TMP_Text>().text = "Deathmatch";
            p_lb.Find("Header/Map").GetComponent<TMP_Text>().text = "Prototype";

            //cache prefab
            GameObject playerCard = p_lb.GetChild(1).gameObject;
            playerCard.SetActive(false);

            //sort
            List<PlayerInfo> sorted = SortPlayers(playerInfo);

            //display
            foreach (PlayerInfo a in sorted)
            {
                GameObject newCard = Instantiate(playerCard, p_lb) as GameObject;

                newCard.transform.Find("LevelDisplay/LevelText").GetComponent<TMP_Text>().text = a.profile.level.ToString("00");
                newCard.transform.Find("NicknameDisplay/Nickname").GetComponent<TMP_Text>().text = a.profile.username;
                newCard.transform.Find("KillsAndDeaths/KillsDisplay/Text").GetComponent<TMP_Text>().text = a.kills.ToString();
                newCard.transform.Find("KillsAndDeaths/DeathsDisplay/Text").GetComponent<TMP_Text>().text = a.deaths.ToString();

                newCard.SetActive(true);
            }

            p_lb.gameObject.SetActive(true);
        }

        List<PlayerInfo> SortPlayers (List<PlayerInfo> p_info)
        {
            List<PlayerInfo> sorted = new List<PlayerInfo>();

            while (sorted.Count < p_info.Count)
            {
                //set defaults
                short highest = -1;
                PlayerInfo selection = p_info[0];

                //grab next highest player
                foreach (PlayerInfo a in p_info)
                {
                    if (sorted.Contains(a)) continue;
                    if(a.kills > highest)
                    {
                        selection = a;
                        highest = a.kills;
                    }
                }

                //add player
                sorted.Add(selection);
            }

            return sorted;
        }

        void ValidateConnection()
        {
            if (PhotonNetwork.IsConnected) return;
            SceneManager.LoadScene(mainmenu);
        }

        void StateCheck()
        {
            if(state == GameState.Ending)
            {
                EndGame();
            }
        }

        void ScoreCheck()
        {
            bool detectwin = false;

            //check if any player has met the win conditions
            foreach(PlayerInfo a in playerInfo)
            {
                //deathmatch
                if(a.kills >= killCount)
                {
                    detectwin = true;
                    break;
                }
            }

            //did we find a winner?
            if(detectwin)
            {
                //we are the master clinet? is the game still going?
                if(PhotonNetwork.IsMasterClient && state != GameState.Ending)
                {
                    //if so tell the other players that a winner has been detected
                    UpdatePlayers_S((int)GameState.Ending, playerInfo);
                }
            }
        }

        void EndGame()
        {
            state = GameState.Ending;

            //disable room
            if(PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.DestroyAll();

                if(!perpetual)
                {
                    PhotonNetwork.CurrentRoom.IsVisible = false;
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                }
            }

            //activate map camera
            mapCam.SetActive(true);

            //show end game ui
            ui_endgame.gameObject.SetActive(true);
            Leaderboard(ui_endgame.Find("Leaderboard"));

            StartCoroutine(End(10f));
        }

        #endregion

        #region Events

        public void NewPlayer_S(ProfileData p)
        {
            object[] package = new object[6];

            package[0] = p.username;
            package[1] = p.level;
            package[2] = p.xp;
            package[3] = PhotonNetwork.LocalPlayer.ActorNumber;
            package[4] = (short)0;
            package[5] = (short)0;

            PhotonNetwork.RaiseEvent(
                (byte)EventCodes.NewPlayer,
                package,
                new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient },
                new SendOptions { Reliability = true }
                );
        }

        public void NewPlayer_R(object[] data)
        {
            PlayerInfo p = new PlayerInfo(
                new ProfileData(
                    (string)data[0],
                    (int)data[1],
                    (int)data[2]
                    ),
                (int)data[3],
                (short)data[4],
                (short)data[5]
                );

            playerInfo.Add(p);

            UpdatePlayers_S((int)state, playerInfo);
        }

        public void UpdatePlayers_S(int state, List<PlayerInfo> info)
        {
            object[] package = new object[info.Count + 1];

            package[0] = state;
            for (int i = 0; i < info.Count; i++)
            {
                object[] piece = new object[6];

                piece[0] = info[i].profile.username;
                piece[1] = info[i].profile.level;
                piece[2] = info[i].profile.xp;
                piece[3] = info[i].actor;
                piece[4] = info[i].kills;
                piece[5] = info[i].deaths;

                package[i + 1] = piece;
            }

            PhotonNetwork.RaiseEvent(
                (byte)EventCodes.UpdatePlayers,
                package,
                new RaiseEventOptions { Receivers = ReceiverGroup.All },
                new SendOptions { Reliability = true }
                );
        }

        public void UpdatePlayers_R (object[] data)
        {
            state = (GameState)data[0];
            playerInfo = new List<PlayerInfo>();

            for(int i = 1; i < data.Length; i++)
            {
                object[] extract = (object[])data[i];
                PlayerInfo p = new PlayerInfo(
                    new ProfileData(
                        (string)extract[0],
                        (int)extract[1],
                        (int)extract[2]
                        ),
                    (int)extract[3],
                    (short)extract[4],
                    (short)extract[5]
                    );

                playerInfo.Add(p);

                if (PhotonNetwork.LocalPlayer.ActorNumber == p.actor) myind = i - 1;
            }

            StateCheck();
        }

        public void ChangeStat_S(int actor, byte stat, byte amt)
        {
            object[] package = new object[] { actor, stat, amt };

            PhotonNetwork.RaiseEvent(
                (byte)EventCodes.ChangeStat,
                package,
                new RaiseEventOptions { Receivers = ReceiverGroup.All },
                new SendOptions { Reliability = true }
                );
        }

        public void ChangeStat_R(object[] data)
        {
            int actor = (int)data[0];
            byte stat = (byte)data[1];
            byte amt = (byte)data[2];

            for(int i = 0; i < playerInfo.Count; i++)
            {
                if(playerInfo[i].actor == actor)
                {
                    switch(stat)
                    {
                        case 0:
                            playerInfo[i].kills += amt;
                            Debug.Log($"Player {playerInfo[i].profile.username} : kills = {playerInfo[i].kills}");
                            break;

                        case 1:
                            playerInfo[i].deaths += amt;
                            Debug.Log($"Player {playerInfo[i].profile.username} : deaths = {playerInfo[i].deaths}");
                            break;
                    }

                    if (ui_Leaderboard.gameObject.activeSelf) Leaderboard(ui_Leaderboard);

                    break;
                }
            }

            ScoreCheck();
        }

        public void NewMatch_S()
        {
            PhotonNetwork.RaiseEvent(
                (byte)EventCodes.NewMatch,
                null,
                new RaiseEventOptions { Receivers = ReceiverGroup.All },
                new SendOptions { Reliability = true }
                );
        }

        public void NewMatch_R()
        {
            //set game to waiting
            state = GameState.Waiting;

            //deactivate map camera
            mapCam.SetActive(false);

            //hide end game ui
            ui_endgame.gameObject.SetActive(false);

            //reset scores
            foreach(PlayerInfo p in playerInfo)
            {
                p.kills = 0;
                p.deaths = 0;
            }

            //reset ui
            RefreshMyStats();

            Spawn();
        }
        #endregion

        #region Coroutines

        IEnumerator End (float p_wait)
        {
            yield return new WaitForSeconds(p_wait);

            if(perpetual)
            {
                if(PhotonNetwork.IsMasterClient)
                {
                    NewMatch_S();
                }
            }
            else
            {
                //disconnect
                PhotonNetwork.AutomaticallySyncScene = false;
                PhotonNetwork.LeaveRoom();
            }
        }

        #endregion
    }
}

