using Photon.Pun;
using Shooter.Combat;
using Shooter.Player;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Shooter.Core
{
    public class PlayTime : MonoBehaviourPunCallbacks
    {
        public TMP_Text countdownText;
        public GameObject exitUI;

        private bool isExiting = false;
        private float countdownStartTime;
        [SerializeField] float timeLimit = 300.0f; // 5 minutes in seconds
        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GetComponent<PhotonView>().RPC("BroadcastTimeLimit", RpcTarget.All, timeLimit);
            }
        }

        private void Update()
        {
            if (isExiting)
            {
                return;
            }

            float elapsed = Time.time - countdownStartTime;
            float remainingTime = Mathf.Max(0, timeLimit - elapsed);

            if (remainingTime <= 0)
            {
                isExiting = true;
                ShowExitUI();
            }
            else
            {
                countdownText.text = FormatTime(remainingTime);
            }
        }

        [PunRPC]
        private void BroadcastTimeLimit(float limit)
        {
            timeLimit = limit;
            countdownStartTime = Time.time;

            StartCoroutine(StartCountdown());
        }

        private IEnumerator StartCountdown()
        {
            while (true)
            {
                float elapsed = Time.time - countdownStartTime;
                float remainingTime = Mathf.Max(0, timeLimit - elapsed);

                if (remainingTime <= 0)
                {
                    isExiting = true;
                    ShowExitUI();
                    yield break;
                }
                else
                {
                    countdownText.text = FormatTime(remainingTime);
                }

                yield return null;
            }
        }

        private string FormatTime(float timeInSeconds)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60);
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        private void ShowExitUI()
        {
            countdownText.gameObject.SetActive(false);
            exitUI.SetActive(true);
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players) 
            {
                player.GetComponent<Gunner>().enabled = false;
                player.GetComponent<Mover>().enabled = false;
                player.GetComponent<Health>().enabled = false;
                
            }
        }

        public void ExitButtonClicked()
        {
            ExitRoomAndLoadMainMenu();
        }

        private void ExitRoomAndLoadMainMenu()
        {
            if (GetComponent<PhotonView>().IsMine)
            {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.LoadLevel("MainMenu");
            }
        }
    }
}
