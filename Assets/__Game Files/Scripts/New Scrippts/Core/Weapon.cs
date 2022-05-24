
using UnityEngine;

namespace Nasser.io.PUN2
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] Gun[] loadout;
        [SerializeField] Transform weaponParent;

        GameObject currentWeapon;
        int currentWeaponId =-1;
        void Start()
        {

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Equip(0);
            }
        }

        private void Equip(int _id)
        {
            if (currentWeaponId != _id )
            {
                Destroy(currentWeapon);
                currentWeaponId = _id;
                GameObject obj = Instantiate(loadout[_id].ObjectPrefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localEulerAngles = Vector3.zero;
                currentWeapon = obj;
            }
        }
    }
}
