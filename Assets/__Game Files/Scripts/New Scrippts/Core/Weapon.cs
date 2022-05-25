
using UnityEngine;

namespace Nasser.io.PUN2
{
    public class Weapon : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] Gun[] loadout;
        [SerializeField] Transform weaponParent;
        [SerializeField] GameObject bulletHolePrefab;
        [SerializeField] LayerMask whatIsShoot;
        #endregion

        #region Private Variables
        GameObject currentWeapon;
        int currentWeaponId = -1;

        #endregion

        #region MonoBehaviour Callbacks
        void Update()
        {
            CheckInput();


        }
        #endregion

        #region Methods

        private void CheckInput()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Equip(0);
            }

            Aim(Input.GetMouseButton(1));

        }
        private void Equip(int _id)
        {
            if (currentWeaponId != _id)
            {
                Destroy(currentWeapon);
                currentWeaponId = _id;
                GameObject obj = Instantiate(loadout[_id].objectPrefab, weaponParent.position, weaponParent.rotation, weaponParent) ;
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localEulerAngles = Vector3.zero;
                currentWeapon = obj;
            }
        }

        private void Aim(bool isAiming)
        {
            if (currentWeapon != null)
            {
                Transform anchor = currentWeapon.transform.GetChild(0);
                Transform ads = currentWeapon.transform.GetChild(1).GetChild(1);
                if (isAiming)
                {
                    anchor.position = Vector3.Lerp(anchor.position, ads.position, Time.deltaTime * loadout[currentWeaponId].aimSpeed  );
                }
                else
                {
                    anchor.localPosition = Vector3.Lerp(anchor.localPosition, Vector3.zero, Time.deltaTime * loadout[currentWeaponId].aimSpeed);
                }
            }

        }

        #endregion

    }
}
