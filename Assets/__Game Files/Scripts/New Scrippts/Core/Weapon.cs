
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
        private GameObject currentWeapon;
        private int currentWeaponId = -1;

        private Transform normalCameraTransform;


        private float cooldown;
        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            normalCameraTransform = transform.parent;
        }
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
                GameObject obj = Instantiate(loadout[_id].objectPrefab, weaponParent.position, weaponParent.rotation, weaponParent);
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
                    anchor.position = Vector3.Lerp(anchor.position, ads.position, Time.deltaTime * loadout[currentWeaponId].aimSpeed);
                }
                else
                {
                    anchor.localPosition = Vector3.Lerp(anchor.localPosition, Vector3.zero, Time.deltaTime * loadout[currentWeaponId].aimSpeed);
                }
                CheckShootInput();
                RestWaponPosition();
            }

        }

        private void RestWaponPosition()
        {
            currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
        }
        private void CheckShootInput()
        {
            if (Input.GetMouseButtonDown(0) && cooldown <= 0)
            {
                Shoot();
            }
            if (cooldown > 0)
                cooldown -= Time.deltaTime;
        }

        private void Shoot()
        {
            RaycastHit hit;
            GameObject tempBullectEffect;

            //bloom
            Vector3 bloomVar = normalCameraTransform.position + normalCameraTransform.forward * 1000f;
            bloomVar += Random.Range(-loadout[currentWeaponId].bloom, loadout[currentWeaponId].bloom) * normalCameraTransform.up;
            bloomVar += Random.Range(-loadout[currentWeaponId].bloom, loadout[currentWeaponId].bloom) * normalCameraTransform.right;
            bloomVar -= normalCameraTransform.position;
            bloomVar.Normalize();

            if (Physics.Raycast(normalCameraTransform.position, bloomVar, out hit, whatIsShoot))
            {
                tempBullectEffect = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.identity);
                tempBullectEffect.transform.LookAt(hit.point + hit.normal);
                Destroy(tempBullectEffect, 5f);
            }

            // fx
            currentWeapon.transform.Rotate(-loadout[currentWeaponId].recoil, 0f, 0f);
            currentWeapon.transform.position -= currentWeapon.transform.forward * loadout[currentWeaponId].kickback;

            cooldown = loadout[currentWeaponId].fireRate;
        }

        #endregion

    }
}
