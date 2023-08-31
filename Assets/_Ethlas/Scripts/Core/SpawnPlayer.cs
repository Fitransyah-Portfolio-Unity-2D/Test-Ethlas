using Cinemachine;
using Photon.Pun;
using UnityEngine;


namespace Shooter.Core
{
    public class SpawnPlayer : MonoBehaviourPunCallbacks
    {
        [SerializeField] GameObject[] playerPrefabs = null;
        [SerializeField] Vector2 spawnPosition = Vector2.zero;

        private void Start()
        {
            SpawnIncomingPlayer();
        }

        private void SpawnIncomingPlayer()
        {
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            spawnPosition = (Vector2)spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            GameObject player = PhotonNetwork.Instantiate(playerPrefabs[PhotonNetwork.CurrentRoom.PlayerCount - 1].name, spawnPosition, Quaternion.identity);
            Transform playerTransform = player.transform;

            

            //GameObject virtualCameraGO = new GameObject("VirtualCamera");
            GameObject virtualCameraGO = new GameObject("VirtualCamera_" + PhotonNetwork.LocalPlayer.ActorNumber); // Unique camera name
            CinemachineVirtualCamera virtualCamera = virtualCameraGO.AddComponent<CinemachineVirtualCamera>();

            virtualCamera.m_Follow = playerTransform;
            virtualCamera.m_LookAt = playerTransform;
            virtualCamera.m_Lens.Orthographic = true;
            virtualCamera.m_Lens.OrthographicSize = 6.22f;
            virtualCamera.Priority = 1;
            virtualCamera.transform.SetParent(playerTransform, false);

            virtualCamera.gameObject.layer = 11;

            CinemachineTransposer transposer = virtualCamera.AddCinemachineComponent<CinemachineTransposer>();
            if (transposer != null)
            {
                transposer.m_FollowOffset.y = 1.5f;
            }

            CinemachineComposer composer = virtualCamera.AddCinemachineComponent<CinemachineComposer>();
            if (composer != null)
            {
                composer.m_DeadZoneHeight = 0.2f;
                composer.m_DeadZoneWidth = 0.75f;
            }   
        }
    }
}

