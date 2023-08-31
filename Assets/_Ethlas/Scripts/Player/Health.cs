using Photon.Pun;
using Shooter.Core;
using System;
using UnityEngine;


namespace Shooter.Player
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float maxHealthPoints = 120f;
        [SerializeField] float healthPoints;
        [SerializeField] GameObject respawnUI;
        private GameObject lastInstigator = null;

        private PhotonView view = null;
        private RespawnHelper respawnHelper;

        private void Awake()
        {
            respawnHelper = GameObject.FindGameObjectWithTag("RespawnHelper").GetComponent<RespawnHelper>();
            if (respawnHelper == null)
            {
                Debug.LogError("No repsawn helper available");
            }
            view = GetComponent<PhotonView>();
        }

        private void Start()
        { 
            ResetHealth();

            if (view != null)
            {
                view.RPC("SetUniqueName", RpcTarget.All);
            }
        }

        [PunRPC]
        void SetUniqueName()
        {
            gameObject.name = gameObject.name + view.ViewID;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            lastInstigator = instigator;
            view.RPC("ApplyDamage", RpcTarget.All, damage);
        }

        [PunRPC]
        private void ApplyDamage(float damage)
        {
            healthPoints -= damage;
            CheckDie();
        }

        private void CheckDie()
        {
            if (healthPoints <= 0f && view.IsMine)
            {
                healthPoints = 0f;

                if (respawnUI != null)
                {
                    respawnUI.SetActive(true);
                }


                view.RPC("Die", RpcTarget.All);
            }
        }

        [PunRPC]
        private void Die()
        {

            gameObject.SetActive(false);

            if (lastInstigator != null)
            {
                lastInstigator.GetComponent<KillCount>().AddKill();
            }

            if (respawnHelper != null)
            {
                respawnHelper.RequestRespawn(gameObject);
                respawnHelper.OnRespawnComplete += OnRespawnCompleteAction;
            }

        }

        private void OnRespawnCompleteAction()
        {
            ResetHealth();
            
            if (respawnUI != null)
            {
                respawnUI.SetActive(false);
            }

            respawnHelper.OnRespawnComplete -= OnRespawnCompleteAction;
        }

        public void ResetHealth()
        {
            healthPoints = maxHealthPoints;
        }

        public float GetHealthPoints()
        {
            return healthPoints;
        }
    }
}