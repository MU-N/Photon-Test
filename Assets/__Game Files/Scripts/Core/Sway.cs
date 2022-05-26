
using UnityEngine;
using Photon.Pun;
using System;

namespace Nasser.io.PUN2
{
    public class Sway : MonoBehaviourPunCallbacks
    {
        #region Inspector Variables
        [SerializeField] float intensity;
        [SerializeField] float smooth;
        #endregion

        #region Private Variables

        private float yInput;
        private float xInput;

        private const string mouseYString = "Mouse Y";
        private const string mouseXString = "Mouse X";

        Quaternion originQuaternion;
        Quaternion targetQuaternion;
        Quaternion adjustQuaternionX;
        Quaternion adjustQuaternionY;

        PhotonView view;
        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            view = GetComponentInParent<PhotonView>();
            originQuaternion = transform.localRotation;
        }
        void Update()
        {
            if (view.IsMine)
            GetInput();
            UpdateSway();
        }


        #endregion

        #region Methods
        private void GetInput()
        {
            xInput = Input.GetAxis(mouseXString);
            yInput = Input.GetAxis(mouseYString);
        }
        private void UpdateSway()
        {

            

            adjustQuaternionX = Quaternion.AngleAxis(-intensity * xInput, Vector3.up);
            adjustQuaternionY = Quaternion.AngleAxis(intensity * yInput, Vector3.right);
            targetQuaternion = adjustQuaternionX * adjustQuaternionY * originQuaternion;

            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetQuaternion, Time.deltaTime * smooth);
        }
        #endregion
    }
}
