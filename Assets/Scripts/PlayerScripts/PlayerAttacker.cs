using System;
using static Helpers.Literals;
using UnityEngine;
using WeaponScripts;

namespace PlayerScripts
{
    public class PlayerAttacker : MonoBehaviour
    {
        private PlayerAnimatorController _animController;
        private PlayerInputHandler _input;
        public AttackAnimations lastAttack;

        private void Awake()
        {
            _animController = GetComponent<PlayerAnimatorController>();
            _input = GetComponent<PlayerInputHandler>();
        }

        public void HandleWeaponCombo(WeaponDataSO weaponData)
        {
            if (!_input.comboFlag) return;
            _animController.SetParameter(AnimatorParameters.CanDoCombo, false);
            switch (lastAttack)
            {
                case AttackAnimations.OH_Light_Attack_1:
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_2.ToString(), true);
                    break;
                case AttackAnimations.OH_Light_Attack_2:
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_3.ToString(), true);
                    break;
                case AttackAnimations.OH_Light_Attack_3:
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_1.ToString(), true);
                    break;
                case AttackAnimations.OH_Heavy_Attack_1:
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_2.ToString(), true);
                    break;
                case AttackAnimations.OH_Heavy_Attack_2:
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_3.ToString(), true);
                    break;
                case AttackAnimations.OH_Heavy_Attack_3:
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_1.ToString(), true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void HandleLightAttack(WeaponDataSO weaponData)
        {
            _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_1.ToString(), true);
            lastAttack = weaponData.OH_Light_Attack_1;
        }
        
        public void HandleHeavyAttack(WeaponDataSO weaponData)
        {
            _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_1.ToString(), true);
            lastAttack = weaponData.OH_Heavy_Attack_1;
        }
    }
}