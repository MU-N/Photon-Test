
using UnityEngine;
using TMPro;

namespace Nasser.io.PUN2
{
    public class RoomItem : MonoBehaviour
    {
        [SerializeField] TMP_Text roomName;
        LobbyManager manager;
        private void Start()
        {
            manager = FindObjectOfType<LobbyManager>();
        }
        public void SetRoomName(string _roomName)
        {
            roomName.text = _roomName;
        }

        public void OnClickItem()
        {
            manager.JoinRoom(roomName.text);
        }
    }
}
