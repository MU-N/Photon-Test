using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Nasser.io.PUN2
{
    public class PlayerSpwaner : MonoBehaviour
    {
        [SerializeField] GameObject[] playerPrfabs;
        [SerializeField] Transform[] spwanPoints;


        int randomNumber;
        Transform spwanLocation;
        GameObject playerToSpwan;
        private void Start()
        {
            randomNumber = Random.Range(0,spwanPoints.Length);
            spwanLocation = spwanPoints[randomNumber];
            playerToSpwan = playerPrfabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];

            PhotonNetwork.Instantiate(playerToSpwan.name, spwanLocation.position, spwanLocation.rotation);
        }
    }
}
