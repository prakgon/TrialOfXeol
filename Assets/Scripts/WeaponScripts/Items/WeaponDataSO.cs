using UnityEngine;
using UnityEngine.Serialization;
using static Helpers.Literals;

namespace WeaponScripts
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/Weapon/Data", order = 1)]
    public class WeaponDataSO : ItemDataSO
    {
        public bool isUnarmed;

        [Header("Idle Animations")] 
        public WeaponIdleAnimations Right_Hand_Idle;
        public WeaponIdleAnimations Left_Hand_Idle;
        
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
        public AttackAnimations OH_Heavy_Attack_7;
        public AttackAnimations Skill_A;
        public AttackAnimations Skill_B;
        public AttackAnimations Skill_C;
        public AttackAnimations Skill_D;
        public AttackAnimations Skill_E;
        public AttackAnimations Skill_F;
        public AttackAnimations OH_Sprint_Light_Attack;
        public AttackAnimations OH_Sprint_Heavy_Attack;
        
        [Header("Stamina Costs")]
        public int baseStamina;
        public float lightStaminaMultiplier;
        public float heavyStaminaMultiplier;
        
        [Header("Damage Stats")]
        public int baseDamage;
        public float lightDamageMultiplier;
        public float heavyDamageMultiplier;
    }
}