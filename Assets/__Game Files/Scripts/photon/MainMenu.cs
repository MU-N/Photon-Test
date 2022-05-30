using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nasser.io.PUN2
{
    public class MainMenu : MonoBehaviour
    {
        #region Inspector Variables

        #endregion

        #region Private Variables

        private Luncher luncher;
        #endregion

        #region MonoBehaviour Callbacks
        void Start()
        {
            luncher = GetComponent<Luncher>();
        }

        #endregion

        #region Public Methods

        public void JoinMatch()
        {
            luncher.OnClickJoin();
        }

        public void CreateMatch()
        {
            luncher.Create();
        }

        public void QuitGame()
        {
            Application.Quit();
        }


        #endregion
    }
}
