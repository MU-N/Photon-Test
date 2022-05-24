
using UnityEngine;

namespace Nasser.io.PUN2
{
    [CreateAssetMenu(fileName ="New Gun", menuName ="Gun")]
    public class Gun : ScriptableObject
    {

        public string Name;
        public float FireRate;
        public GameObject ObjectPrefab;
    }
}
