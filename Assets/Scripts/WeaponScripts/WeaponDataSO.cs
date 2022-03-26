using UnityEngine;
using static Helpers.Literals;

namespace WeaponScripts
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/Weapon/Data", order = 1)]
    public class WeaponDataSO : ScriptableObject
    {
        [Header("Game logic related")]
        public GameObject modelPrefab;
        public bool isUnarmed;
        public Sprite iconSprite;
        public ItemNames itemName;

        [Header("One Handed Attack Animations")]
        public AttackAnimations OH_Light_Attack_1;
        public AttackAnimations OH_Light_Attack_2;
        public AttackAnimations OH_Light_Attack_3;
        public AttackAnimations OH_Heavy_Attack_1;
        public AttackAnimations OH_Heavy_Attack_2;
        public AttackAnimations OH_Heavy_Attack_3;
        
        [Header("Stats")]
        public float damage;
    }
}