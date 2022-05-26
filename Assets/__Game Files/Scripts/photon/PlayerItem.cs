
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System;

namespace Nasser.io.PUN2
{
    public class PlayerItem : MonoBehaviourPunCallbacks
    {
        [SerializeField] TMP_Text playerName;
        [SerializeField] Color HightlightColor;
        [SerializeField] GameObject rightArrow;
        [SerializeField] GameObject LeftArrow;

        [SerializeField] Image playerBGImage;
        [SerializeField] Image playerCharcterAvatar;

        [SerializeField] Sprite[] playerAvatars;

        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

        Player player;
        public void SetPlayerInfo(Player _player)
        {
            playerName.text = _player.NickName;
            player = _player;
            UpdatePlayerItem(player);
        }

        public void ApplyLocalChanges()
        {
            playerBGImage.color = HightlightColor;
            if (player == PhotonNetwork.LocalPlayer)
            {
                rightArrow.SetActive(true);
                LeftArrow.SetActive(true);
            }
            else
            {
                rightArrow.SetActive(true);
                LeftArrow.SetActive(true);
            }

        }

        public void OnclickLeftArrow()
        {
            if ((int)playerProperties["playerAvatar"] == 0)
                playerProperties["playerAvatar"] = playerAvatars.Length - 1;
            else
                playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] - 1;

            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        }

        public void OnclickRightArrow()
        {
            if ((int)playerProperties["playerAvatar"] == playerAvatars.Length - 1)
                playerProperties["playerAvatar"] = 0;
            else
                playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] + 1;

            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        }


        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if(player == targetPlayer)
            {
                UpdatePlayerItem(targetPlayer);
            }
        }

        private void UpdatePlayerItem(Player _player)
        {
            if (_player.CustomProperties.ContainsKey("playerAvatar"))
            {
                playerCharcterAvatar.sprite = playerAvatars[(int)_player.CustomProperties["playerAvatar"]];
                playerProperties["playerAvatar"] = (int)_player.CustomProperties["playerAvatar"];
            }
            else
            {
                playerProperties["playerAvatar"] = 0;
            }
        }
    }
}
