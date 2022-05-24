using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Nasser.io.PUN2
{
    public class ConnectToServer : MonoBehaviourPunCallbacks
    {
        [SerializeField] TMP_InputField usernameInput;
        [SerializeField] TMP_Text buttonText;

        public void OnClcickConnect()
        {
            if(usernameInput.text.Length > 0)
            {
                PhotonNetwork.NickName = usernameInput.text;
                buttonText.text = "Connecting...";
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.AutomaticallySyncScene = true;
            }
        }

        public override void OnConnectedToMaster()
        {
            SceneManager.LoadScene("Lobby");
        }



    }
}

