using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Shooter.Player
{
    public class KillCount : MonoBehaviour
    {
        [SerializeField] TMP_Text killCountText;
        public int killCount = 0;

        private void Awake()
        {
            GameObject killCountGO = GameObject.FindGameObjectWithTag("Killcount");
            if (killCountGO != null)
            {
                killCountText = killCountGO.GetComponent<TMP_Text>();
            }
        }

        private void Start()
        {
            killCount = 0;
        }

        public void AddKill()
        {
            GetComponent<PhotonView>().RPC("AddKillRPC", RpcTarget.All);
            killCountText.text = killCount.ToString();
        }

        [PunRPC]
        public void AddKillRPC()
        {
            killCount++;
        }
    }
}
