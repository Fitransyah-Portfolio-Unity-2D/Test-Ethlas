using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Shooter.Player
{
    public class KillCount : MonoBehaviour
    {
        public int killCount = 0;

        private void Start()
        {
            killCount = 0;
        }

        public void AddKill()
        {
            GetComponent<PhotonView>().RPC("AddKillRPC", RpcTarget.All);
        }

        [PunRPC]
        public void AddKillRPC()
        {
            killCount++;
        }
    }
}
