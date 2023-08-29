using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter.Core
{
    public class SpawnPickups : MonoBehaviour
    {
        
        [SerializeField] GameObject[] pickupPrefabs = null;

        List<Vector2> pickupPositions = new List<Vector2>();

        

        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameObject[] pickupSpawnPoints = GameObject.FindGameObjectsWithTag("PickupSpawnPoint");

                if (pickupSpawnPoints.Length == 0) return;

                foreach (GameObject pickupPosition in pickupSpawnPoints)
                {
                    pickupPositions.Add((Vector2)pickupPosition.transform.position);
                }

                foreach (Vector2 position in pickupPositions)
                {
                    int randomPrefabIndex = Random.Range(0, pickupPrefabs.Length);
                    PhotonNetwork.Instantiate(pickupPrefabs[randomPrefabIndex].name, position, Quaternion.identity);
                }
            }
        }
    }
}
