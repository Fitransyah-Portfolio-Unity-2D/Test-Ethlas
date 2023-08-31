using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Shooter.Network
{
    public class MainMenuManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] int maxPlayers;
        [SerializeField] TMP_InputField createRoomInputText;
        [SerializeField] TMP_InputField joinRoomInputText;
        [SerializeField] TMP_Text infoText;
        [SerializeField] GameObject ScrollViewContent;
        [SerializeField] RoomDetail roomItem;
        [SerializeField] GameObject waitingListUI;
        [SerializeField] TMP_Text currentPlayerText;

        static string roomName = "Deadmatch";
        List<RoomInfo> existingRoomList = new List<RoomInfo>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RefreshRoomList();
            }
        }

        public void QuickMatch()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinedRoom()
        {
            if (!CheckRoomFullness())
            {
                waitingListUI.SetActive(true); 
                CheckMinimumPlayersAndStart();
                //PhotonNetwork.LoadLevel("Arena");

            }
        }

        public override void OnLeftRoom()
        {
            waitingListUI.SetActive(false);
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            CheckMinimumPlayersAndStart();
        }

        public void CancelMatch()
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
        }

        public void CheckMinimumPlayersAndStart()
        {
            currentPlayerText.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
            if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayers)
            {
                PhotonNetwork.LoadLevel("Arena"); 
            }
        }


        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            infoText.text = "Sorry room you're trying to join is full, please create a room or join with quickmatch";
            ShowInfoText();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            CreateRoomForQuickmatch();
        }

        public void CreateRoom()
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayers;
            TypedLobby typedLobby = new TypedLobby("TestLobby", LobbyType.Default);
          
            PhotonNetwork.CreateRoom(createRoomInputText.text , roomOptions, typedLobby);
        }

        private void CreateRoomForQuickmatch()
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayers;
            TypedLobby typedLobby = new TypedLobby("TestLobby", LobbyType.Default);

            PhotonNetwork.CreateRoom($"{roomName} {UnityEngine.Random.Range(0, 99)}", roomOptions, typedLobby);
        }

        public void JoinRoom()
        {
            PhotonNetwork.JoinRoom(joinRoomInputText.text);
        }
       
        private bool CheckRoomFullness()
        {
            if (PhotonNetwork.CurrentRoom != null)
            {
                int currentPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
                int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;

                return currentPlayers > maxPlayers;
            }
            else
            {
                return false;
            }
        }

        public void RefreshRoomList()
        {
            ClearRoomList();

            if (existingRoomList.Count > 0)
            {
                foreach (var room in existingRoomList)
                {
                    RoomDetail roomDetail = Instantiate(roomItem, ScrollViewContent.transform);
                    roomDetail.UpdateRoomDetail(room);
                }
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            existingRoomList = roomList;
            RefreshRoomList();
        }

        private void ClearRoomList()
        {
            foreach (Transform child in ScrollViewContent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void ShowInfoText()
        {
            infoText.gameObject.SetActive(true);
            Invoke("HideInfoText", 2f);
        }
        private void HideInfoText()
        {
            infoText.gameObject.SetActive(false);
        }



    }
}
