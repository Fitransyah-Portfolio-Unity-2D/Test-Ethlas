using Photon.Pun;
using Photon.Realtime;

namespace Shooter.Network
{
    public class ConnectToServer : MonoBehaviourPunCallbacks
    {

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {

            TypedLobby typedLobby = new TypedLobby("TestLobby", LobbyType.Default);
            PhotonNetwork.JoinLobby(typedLobby);
            PhotonNetwork.LoadLevel("MainMenu");
        }

    }
}


