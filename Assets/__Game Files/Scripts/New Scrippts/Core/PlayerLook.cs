
using UnityEngine;

namespace Nasser.io.PUN2
{
    public class PlayerLook : MonoBehaviour
    {
        public static bool isCurserLocked = true;
        [SerializeField] Transform player;
        [SerializeField] Transform cam;

        [SerializeField] float xSensitivity;
        [SerializeField] float ySensitivity;
        [SerializeField] float maxYAnagle;

        private float yInput;
        private float XInput;

        private const string mouseYString = "Mouse Y";
        private const string mouseXString = "Mouse X";

        Quaternion yQuaternion;
        Quaternion yQuaternionDelta;
        Quaternion camCentre;

        Quaternion XQuaternion;
        Quaternion XQuaternionDelta;
        void Start()
        {
            camCentre = cam.localRotation;
        }

        void Update()
        {
            SetY();
            SetX();
            UpdateLockCurser();
        }

        void SetY()
        {
            yInput = Input.GetAxis(mouseYString) * ySensitivity * Time.deltaTime;
            yQuaternion = Quaternion.AngleAxis(yInput, -Vector3.right);
            yQuaternionDelta = cam.localRotation * yQuaternion;
            if (Quaternion.Angle(camCentre, yQuaternionDelta) < maxYAnagle)
            {
                cam.localRotation = yQuaternionDelta;
            }
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
    }
}
