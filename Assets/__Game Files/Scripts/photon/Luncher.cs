using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Nasser.io.PUN2
{
    public class Luncher : MonoBehaviourPunCallbacks
    {
        [SerializeField] TMP_InputField usernameInput;
        [SerializeField] TMP_Text buttonText;
        [SerializeField] TMP_Text feedbackText;

        [SerializeField] string gameVersion;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            OnClcickConnect();
        }

 
        public void OnClcickConnect()
        {
            /*if(usernameInput.text.Length > 0)
            {
                PhotonNetwork.NickName = usernameInput.text;
                buttonText.text = "Connecting...";
                PhotonNetwork.ConnectUsingSettings();
            }*/
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
            feedbackText.text += "Connecting\n";
        }

        public override void OnConnectedToMaster()
        {
            /*SceneManager.LoadScene("Lobby");*/
        }


        public void OnClickJoin()
        {
            PhotonNetwork.JoinRandomRoom();
            feedbackText.text += "joining\n";
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Create();
        }


        public void Create()
        {
            PhotonNetwork.CreateRoom("Test");
        }
        public override void OnJoinedRoom()
        {
            OnClickStartGame();
        }
        public void OnClickStartGame()
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
            SceneManager.LoadScene("Game");
            feedbackText.text += "joined\n";
        }


    }
}

