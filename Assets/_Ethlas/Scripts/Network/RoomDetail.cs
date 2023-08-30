using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Shooter.Network
{
    public class RoomDetail : MonoBehaviour
    {
        [SerializeField] TMP_Text roomNameText;

        public void UpdateRoomDetail(RoomInfo roomInfo )
        {
            roomNameText.text = roomInfo.Name;
        }
    }
}

