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


        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        public void OnClcickConnect()
        {
            if(usernameInput.text.Length > 0)
            {
                PhotonNetwork.NickName = usernameInput.text;
                buttonText.text = "Connecting...";
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            SceneManager.LoadScene("Lobby");
        }



    }
}

