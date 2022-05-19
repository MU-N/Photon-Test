
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

namespace Nasser.io.PUN2
{
    public class PlayerItem : MonoBehaviour
    {
        [SerializeField] TMP_Text playerName;
        [SerializeField] Color HightlightColor;
        [SerializeField] GameObject rightArrow;
        [SerializeField] GameObject LeftArrow;

        Image playerBGImage;

        private void Start()
        {
            playerBGImage = GetComponent<Image>();
        }
        public void SetPlayerInfo(Player _player)
        {
            playerName.text = _player.NickName;
        }

        public void ApplyLocalChanges()
        {
            playerBGImage.color = HightlightColor;
            rightArrow.SetActive(false);
            LeftArrow.SetActive(true);

        }
    }
}
