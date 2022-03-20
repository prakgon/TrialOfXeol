using UnityEngine;

namespace WeaponScripts
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/Weapon/Data", order = 1)]
    public class WeaponDataSO : ScriptableObject
    {
        public float damage;
    }
}