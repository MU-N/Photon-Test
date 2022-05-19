using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace Nasser.io.PUN2
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] TMP_InputField roomInputName;
        [SerializeField] GameObject lobbyPanel;
        [SerializeField] GameObject roomPanel;
        [SerializeField] TMP_Text roomName;

        [SerializeField] RoomItem roomItemPrefab;
        [SerializeField] Transform roomItemPrefabHolder;

        List<RoomItem> roomItemList = new List<RoomItem>();

        [SerializeField] float timeBetweenUpdate = 1.5f;
        float nextUpdateTime;

        List<PlayerItem> playerItemsList = new List<PlayerItem>();
        [SerializeField] PlayerItem playerItemPrefab;
        [SerializeField] Transform playerItemPrefabHolder;

        private void Start()
        {
            PhotonNetwork.JoinLobby();
        }

        public void OnClickCreateRoom()
        {
            if (roomInputName.text.Length > 0)
            {
                PhotonNetwork.CreateRoom(roomInputName.text, new RoomOptions() { MaxPlayers = 10 });
            }
        }

        public override void OnJoinedRoom()
        {
            lobbyPanel.SetActive(false);
            roomPanel.SetActive(true);
            roomName.text = "Room Name :" + PhotonNetwork.CurrentRoom.Name;
            UpdatePlayersList();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            if (Time.time >= nextUpdateTime)
            {
                UpdateRoomList(roomList);
                nextUpdateTime = Time.time + timeBetweenUpdate;
            }
        }

        private void UpdateRoomList(List<RoomInfo> _roomList)
        {
            foreach (RoomItem item in roomItemList)
            {
                Destroy(item.gameObject);
            }

            roomItemList.Clear();

            foreach (RoomInfo item in _roomList)
            {
                RoomItem newRoomItem = Instantiate(roomItemPrefab, roomItemPrefabHolder);
                newRoomItem.SetRoomName(item.Name);
                roomItemList.Add(newRoomItem);
            }
        }

        public void JoinRoom(string _roomName)
        {
            PhotonNetwork.JoinRoom(_roomName);
        }

        public void OnClickLeaveRoom()
        {
            PhotonNetwork.LeaveRoom();

        }

        public override void OnLeftRoom()
        {
            roomPanel.SetActive(false);
            lobbyPanel.SetActive(true);
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }


        private void UpdatePlayersList()
        {
            foreach (PlayerItem item in playerItemsList)
            {
                Destroy(item.gameObject);
            }

            playerItemsList.Clear();

            if (PhotonNetwork.CurrentRoom == null) return;

            foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
            {
                PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemPrefabHolder);
                newPlayerItem.SetPlayerInfo(player.Value);

                if (player.Value == PhotonNetwork.LocalPlayer)
                {
                    newPlayerItem.ApplyLocalChanges();
                }
                playerItemsList.Add(newPlayerItem);
            }
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            UpdatePlayersList();
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            UpdatePlayersList();
        }
    }
}
