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
        public AttackAnimations OH_Light_Attack_4;
        public AttackAnimations OH_Light_Attack_5;
        public AttackAnimations OH_Light_Attack_6;
        public AttackAnimations OH_Heavy_Attack_1;
        public AttackAnimations OH_Heavy_Attack_2;
        public AttackAnimations OH_Heavy_Attack_3;
        public AttackAnimations OH_Heavy_Attack_4;
        public AttackAnimations OH_Heavy_Attack_5;
        public AttackAnimations OH_Heavy_Attack_6;
        public AttackAnimations Skill_A;
        public AttackAnimations Skill_B;
        public AttackAnimations Skill_C;
        public AttackAnimations Skill_D;
        public AttackAnimations Skill_E;
        public AttackAnimations Skill_F;
        
        [Header("Stats")]
        public float damage;
    }
}