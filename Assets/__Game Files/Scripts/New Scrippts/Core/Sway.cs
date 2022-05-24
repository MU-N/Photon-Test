using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nasser.io.PUN2
{
    public class Sway : MonoBehaviour
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
        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            originQuaternion = transform.localRotation;
        }
        void Update()
        {
            UpdateSway();
        }
        #endregion

        #region Methods

        private void UpdateSway()
        {

            xInput = Input.GetAxis(mouseXString);
            yInput = Input.GetAxis(mouseYString);

            adjustQuaternionX = Quaternion.AngleAxis(-intensity * xInput, Vector3.up);
            adjustQuaternionY = Quaternion.AngleAxis(intensity * yInput, Vector3.right);
            targetQuaternion = adjustQuaternionX * adjustQuaternionY * originQuaternion;

            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetQuaternion, Time.deltaTime * smooth);
        }
        #endregion
    }
}
