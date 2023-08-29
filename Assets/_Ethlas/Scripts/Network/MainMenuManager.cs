using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Shooter.Network
{
    public class MainMenuManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] int maxPlayers;
        [SerializeField] TMP_InputField createRoomInputText;
        [SerializeField] TMP_InputField joinRoomInputText;

        static string roomName = "QuickMatch";
       
        public void QuickMatch()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Arena");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            CreateRoomForQuickmatch();
        }

        public void CreateRoom()
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayers;
            PhotonNetwork.CreateRoom(createRoomInputText.text, roomOptions);
        }

        public void JoinRoom()
        {
            PhotonNetwork.JoinRoom(joinRoomInputText.text);
        }
        private void CreateRoomForQuickmatch()
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayers;
            PhotonNetwork.CreateRoom($"{roomName} {UnityEngine.Random.Range(0, 99)}", roomOptions);
        }



    }
}
