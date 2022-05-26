
using UnityEngine;
using Photon.Pun;

namespace Nasser.io.PUN2
{
    public class Weapon : MonoBehaviourPunCallbacks
    {
        #region Inspector Variables
        [SerializeField] Gun[] loadout;
        [SerializeField] Transform weaponParent;
        [SerializeField] GameObject bulletHolePrefab;
        [SerializeField] LayerMask whatIsShoot;
        [SerializeField] int playersLayer;
        #endregion

        #region Private Variables
        private GameObject currentWeapon;
        private int currentWeaponId = -1;

        private Transform normalCameraTransform;


        private float cooldown;

        PhotonView view;
        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            view = GetComponentInParent<PhotonView>();
            normalCameraTransform = transform.GetChild(1).GetChild(0);

        }
        void Update()
        {
            if (!view.IsMine) return;

            CheckInput();
        }
        #endregion

        #region Methods

        private void CheckInput()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                view.RPC("Equip", RpcTarget.All, 0);
            }

            Aim(Input.GetMouseButton(1));

        }
        [PunRPC]
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
                Transform hips = currentWeapon.transform.GetChild(1).GetChild(0);
                if (isAiming)
                {
                    anchor.position = Vector3.Lerp(anchor.position, ads.position, Time.deltaTime * loadout[currentWeaponId].aimSpeed);
                }
                else
                {
                    anchor.localPosition = Vector3.Lerp(anchor.localPosition, hips.localPosition, Time.deltaTime * loadout[currentWeaponId].aimSpeed);
                }
                CheckShootInput();
                 view.RPC("RestWaponPosition", RpcTarget.All);
                //RestWaponPosition();
            }

        }
        [PunRPC]
        private void RestWaponPosition()
        {
            currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
        }
        private void CheckShootInput()
        {
            if (Input.GetMouseButtonDown(0) && cooldown <= 0)
            {
                view.RPC("Shoot", RpcTarget.All);
            }
            if (cooldown > 0)
                cooldown -= Time.deltaTime;
        }

        [PunRPC]
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

                if (view.IsMine)
                {
                    // shootingPlayers
                    if(hit.collider.gameObject.layer == playersLayer)
                    {
                        hit.collider.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[currentWeaponId].damage);
                    }
                }
            }

            // fx
            currentWeapon.transform.Rotate(-loadout[currentWeaponId].recoil, 0f, 0f);
            currentWeapon.transform.position -= currentWeapon.transform.forward * loadout[currentWeaponId].kickback;

            cooldown = loadout[currentWeaponId].fireRate;
        }

        [PunRPC]
        private void TakeDamage(int _amount)
        {
            GetComponent<PlayerMotion>().TakeDamageR(_amount);
        }
        #endregion

    }
}
