using Photon.Pun;
using Shooter.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Shooter.Player
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float maxHealthPoints = 120f;
        [SerializeField] float healthPoints;
        [SerializeField] UnityEvent<float> takeDamage;
        [SerializeField] UnityEvent dead;

        GameObject lastInstigator = null;

        private void Start()
        {
            healthPoints = maxHealthPoints;
        }

       

        public void TakeDamage(GameObject instigator, float damage)
        {

            lastInstigator = instigator;
            GetComponent<PhotonView>().RPC("TakeDamageRPC", RpcTarget.All, damage);

            

        }

        [PunRPC]
        private void TakeDamageRPC(float damage)
        {
            // Update health UI
            // Play player hurt sound


            
            healthPoints -= damage;
            if (healthPoints <= 0f)
            {
                Die();
            }

            takeDamage.Invoke(damage);
        }

        void Die()
        {
            //SpawnPlayer playerSpawner = GameObject.FindGameObjectWithTag("PlayerSpawner").GetComponent<SpawnPlayer>();
            //playerSpawner.DestroySpecificPlayer(gameObject);

            Destroy(gameObject);
            // play dead sound
            // loading screen, exit room
            dead.Invoke();
        }

        public float GetHealthPoints()
        {
            return healthPoints;
        }

        public float GetPercentage()
        {
            return 100 * (healthPoints / maxHealthPoints);
        }

        public float GetFraction()
        {
            return healthPoints / maxHealthPoints;
        }
    }
}
