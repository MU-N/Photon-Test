
using UnityEngine;
using Photon.Pun;

namespace Nasser.io.PUN2
{
    public class PlayerSpwaner : MonoBehaviour
    {
        [SerializeField] GameObject playerPrfabs;
        [SerializeField] Transform[] spwanPoints;

        int spwanLocationCount;

        int randomNumber;
        Transform spwanLocation;
        GameObject playerToSpwan;
        private void Start()
        {
            spwanLocationCount = transform.childCount;
            spwanPoints = new Transform[spwanLocationCount];
            for (int i = 0; i < spwanLocationCount; i++)
            {
                spwanPoints[i] = transform.GetChild(i);
            }

            Spwan();
        }

        public void Spwan()
        {
            randomNumber = Random.Range(0, spwanPoints.Length);
            spwanLocation = spwanPoints[randomNumber];
            playerToSpwan = playerPrfabs;

            PhotonNetwork.Instantiate(playerToSpwan.name, spwanLocation.position, spwanLocation.rotation);
        }
    }
}
