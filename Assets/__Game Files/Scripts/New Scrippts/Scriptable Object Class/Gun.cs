
using UnityEngine;

namespace Nasser.io.PUN2
{
    [CreateAssetMenu(fileName ="New Gun", menuName ="Gun")]
    public class Gun : ScriptableObject
    {

        public string gunName;
        public float fireRate;
        public float aimSpeed;
        public GameObject objectPrefab;
    }
}
