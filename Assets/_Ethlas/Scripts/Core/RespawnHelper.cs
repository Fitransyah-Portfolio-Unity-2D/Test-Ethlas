using Photon.Pun;
using Shooter.Player;
using System;
using System.Collections;
using UnityEngine;

namespace Shooter.Core
{
    public class RespawnHelper : MonoBehaviourPunCallbacks
    {
        public event Action OnRespawnComplete;

        float delay = 4f;
        GameObject playerToRespawn = null;
        public void RequestRespawn(GameObject player)
        {
            if (player != null) 
            {
                playerToRespawn = player;
                StartCoroutine(RespawnSequence());
            }
        }

        private IEnumerator RespawnSequence() 
        {
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            Transform pickedSpawnedPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].transform;
            playerToRespawn.transform.position = pickedSpawnedPoint.position;


            yield return new WaitForSeconds(delay);

            playerToRespawn.SetActive(true);

            if (OnRespawnComplete != null)
            {
                OnRespawnComplete();
            }

        }
    }
}
