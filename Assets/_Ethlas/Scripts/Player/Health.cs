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
            ResetHealth();
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
            if (lastInstigator != null)
            {
                lastInstigator.GetComponent<KillCount>().AddKill();
            }

            if (GetComponent<PhotonView>().IsMine)
            {
                RespawnHelper respawnHelper = GameObject.FindGameObjectWithTag("RespawnHelper").GetComponent<RespawnHelper>();
                if (respawnHelper != null && PhotonNetwork.IsMasterClient)
                {
                    respawnHelper.RequestRespawn(gameObject);
                }
            }

            // play dead sound
            // loading screen, exit room
            dead.Invoke();

        }
        
        public void ResetHealth()
        {
            healthPoints = maxHealthPoints;
        }
    }
}
