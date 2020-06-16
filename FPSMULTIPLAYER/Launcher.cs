using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

namespace Com.BoarShroom.Prototype
{
    [System.Serializable]
    public class ProfileData
    {
        public string username;
        public int level;
        public int xp;

        public ProfileData()
        {
            this.username = "DEFAULT";
            this.level = 0;
            this.xp = 0;
        }

        public ProfileData(string u, int l, int x)
        {
            this.username = u;
            this.level = l;
            this.xp = x;
        }

        object[] ConvertToObjectArr()
        {
            object[] ret = new object[3];
            return ret;
        }
    }

    [System.Serializable]
    public class MapData
    {
        public string name;
        public int scene;
    }

    public class Launcher : MonoBehaviourPunCallbacks
    {
        public TMP_InputField usernameField;
        public TMP_InputField roomNameField;
        public Slider maxPlayersSlider;
        public TMP_Text maxPlayersValue;
        public TMP_Text mapValue;
        public static ProfileData myProfile = new ProfileData();

        public GameObject tabMain;
        public GameObject tabRooms;
        public GameObject tabLogin;
        public GameObject tabCreate;

        public GameObject buttonRoom;

        public MapData[] maps;
        int currentMap = 0;

        private List<RoomInfo> roomList;

        public void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            myProfile = Data.LoadProfile();
            usernameField.text = myProfile.username;

            Connect();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
            base.OnConnectedToMaster();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            //Create();

            base.OnJoinRandomFailed(returnCode, message);
        }

        public void Connect()
        {
            PhotonNetwork.GameVersion = "0.0.0";
            PhotonNetwork.ConnectUsingSettings();
        }

        public void Create()
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = (byte)maxPlayersSlider.value;
            options.CustomRoomPropertiesForLobby = new string[] { "map" };

            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add("map", currentMap);
            options.CustomRoomProperties = properties;

            PhotonNetwork.CreateRoom(roomNameField.text, options);
        }

        public void ChangeMap()
        {
            currentMap++;
            if (currentMap >= maps.Length) currentMap = 0;
            {
                mapValue.text = maps[currentMap].name.ToUpper();
            }
        }

        public void ChangeMaxPlayersSlider(float t_value)
        {
            maxPlayersValue.text = Mathf.RoundToInt(t_value).ToString();
        }

        public override void OnJoinedRoom()
        {
            StartGame();

            base.OnJoinedRoom();
        }

        public void Join()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public void StartGame()
        {
            VerifyUsername();

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Data.SaveProfile(myProfile);
                PhotonNetwork.LoadLevel(maps[currentMap].scene);
            }
        }

        public void TabCloseAll()
        {
            tabLogin.SetActive(false);
            tabRooms.SetActive(false);
            tabMain.SetActive(false);
            tabCreate.SetActive(false);
        }

        public void TabOpenCreate()
        {
            TabCloseAll();
            tabCreate.SetActive(true);

            roomNameField.text = "";

            currentMap = 0;
            mapValue.text = maps[currentMap].name.ToUpper();

            maxPlayersSlider.value = maxPlayersSlider.maxValue;
            maxPlayersValue.text = Mathf.RoundToInt(maxPlayersSlider.value).ToString();
        }

        public void TabOpenMenu()
        {
            TabCloseAll();
            tabMain.SetActive(true);
        }

        public void TabOpenLogin()
        {
            TabCloseAll();
            tabLogin.SetActive(true);
        }

        public void TabOpenRooms()
        {
            TabCloseAll();
            tabRooms.SetActive(true);
        }

        private void ClearRoomList()
        {
            Transform content = tabRooms.transform.Find("Scroll View/Viewport/Content");
            foreach (Transform a in content) Destroy(a.gameObject);
        }

        public override void OnRoomListUpdate(List<RoomInfo> p_list)
        {
            roomList = p_list;
            ClearRoomList();

            Debug.Log("LOADED ROOMS @ " + Time.time);
            Transform content = tabRooms.transform.Find("Scroll View/Viewport/Content");

            foreach(RoomInfo a in roomList)
            {
                GameObject newRoomButton = Instantiate(buttonRoom, content) as GameObject;

                newRoomButton.transform.Find("Name").GetComponent<TMP_Text>().text = a.Name;
                newRoomButton.transform.Find("Players").GetComponent<TMP_Text>().text = a.PlayerCount + " / " + a.MaxPlayers;

                if(a.CustomProperties.ContainsKey("map"))
                {
                    newRoomButton.transform.Find("Map").GetComponent<TMP_Text>().text = maps[(int)a.CustomProperties["map"]].name;
                }
                else
                {
                    newRoomButton.transform.Find("Map").GetComponent<TMP_Text>().text = "-----";
                }

                newRoomButton.GetComponent<Button>().onClick.AddListener(delegate { JoinRoom(newRoomButton.transform); });
            }

            base.OnRoomListUpdate(roomList);
        }

        public void JoinRoom(Transform p_button)
        {
            Debug.Log("JOINING ROOM @ " + Time.time);
            string t_roomName = p_button.transform.Find("Name").GetComponent<TMP_Text>().text;

            VerifyUsername();
            PhotonNetwork.JoinRoom(t_roomName);
        }

        void VerifyUsername()
        {
            if(string.IsNullOrEmpty(usernameField.text))
            {
                myProfile.username = "RANDOM_USER_" + Random.Range(100, 10000);
            }
            else
            {
                myProfile.username = usernameField.text;
            }
        }
    }
}

