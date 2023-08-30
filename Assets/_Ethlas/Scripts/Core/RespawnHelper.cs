using Photon.Pun;
using Shooter.Player;
using System.Collections;
using UnityEngine;

namespace Shooter.Core
{
    public class RespawnHelper : MonoBehaviourPunCallbacks
    {
        [SerializeField] GameObject respawnUI;

        float delay = 4f;
        bool isRespawning = false;
        GameObject playerToRespawn = null;
        public void RequestRespawn(GameObject player)
        {
            if (!isRespawning) 
            {
                playerToRespawn = player;
                isRespawning = true;
                
                GetComponent<PhotonView>().RPC("StartRespawnSequence", RpcTarget.All);
            }
        }

        [PunRPC]
        private void StartRespawnSequence()
        {
            if (playerToRespawn == null)
            {
                Debug.Log("No player to respawn assigned");
                return;
            }
            StartCoroutine(RespawnSequence());
        }

        private IEnumerator RespawnSequence() 
        {
            playerToRespawn.SetActive(false);
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            Transform pickedSpawnedPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
            playerToRespawn.transform.position = pickedSpawnedPoint.position;
            respawnUI.SetActive(true);

            yield return new WaitForSeconds(delay);

            playerToRespawn.SetActive(true);
            playerToRespawn.GetComponent<Health>().ResetHealth();

            isRespawning = false;
            respawnUI.SetActive(false);
        }
    }
}
