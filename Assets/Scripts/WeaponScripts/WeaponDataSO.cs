using UnityEngine;

namespace WeaponScripts
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/Weapon/Data", order = 1)]
    public class WeaponDataSO : ScriptableObject
    {
        public Sprite iconSprite;
        public string itemName;
        public float damage;
        public GameObject modelPrefab;
        public bool isUnarmed;
    }
}