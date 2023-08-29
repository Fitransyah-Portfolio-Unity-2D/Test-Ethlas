using Photon.Pun;



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
            
            PhotonNetwork.LoadLevel("MainMenu");
        }

    }
}


