
using Photon.Pun;
using UnityEngine;

namespace Nasser.io.PUN2
{
    public class PlayerLook : MonoBehaviourPunCallbacks
    {
        #region Inspector Variables

        public static bool isCurserLocked = true;
        [SerializeField] Transform player;
        [SerializeField] Transform cam;
        [SerializeField] Transform weaponParent;

        [SerializeField] float xSensitivity;
        [SerializeField] float ySensitivity;
        [SerializeField] float maxYAnagle;

        #endregion

        #region Private Variables
        private float yInput;
        private float XInput;

        private const string mouseYString = "Mouse Y";
        private const string mouseXString = "Mouse X";

        Quaternion yQuaternion;
        Quaternion yQuaternionDelta;
        Quaternion camCentre;

        Quaternion XQuaternion;
        Quaternion XQuaternionDelta;

        PhotonView view;

        #endregion

        #region MonoBehaviour Callbacks
        void Start()
        {
            view = GetComponent<PhotonView>();
            camCentre = cam.localRotation;
        }

        void Update()
        {
            if (!view.IsMine) return;
            SetY();
            SetX();
            UpdateLockCurser();
        }
        #endregion

        #region Methods

        void SetY()
        {
            yInput = Input.GetAxis(mouseYString) * ySensitivity * Time.deltaTime;
            yQuaternion = Quaternion.AngleAxis(yInput, -Vector3.right);
            yQuaternionDelta = cam.localRotation * yQuaternion;
            if (Quaternion.Angle(camCentre, yQuaternionDelta) < maxYAnagle)
            {
                cam.localRotation = yQuaternionDelta;
            }
            weaponParent.rotation = cam.rotation;
        }

        void SetX()
        {
            XInput = Input.GetAxis(mouseXString) * xSensitivity * Time.deltaTime;
            XQuaternion = Quaternion.AngleAxis(XInput, Vector3.up);
            XQuaternionDelta = player.localRotation * XQuaternion;
            player.localRotation = XQuaternionDelta;

        }

        private void UpdateLockCurser()
        {
            
            if (isCurserLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if (Input.GetKeyDown(KeyCode.Escape))
                    isCurserLocked = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                if (Input.GetKeyDown(KeyCode.Escape))
                    isCurserLocked = true;
            }
        }
        #endregion
       
    }
}
