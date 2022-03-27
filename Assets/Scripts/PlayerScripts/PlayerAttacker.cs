using System;
using Photon.Pun.Demo.PunBasics;
using static Helpers.Literals;
using UnityEngine;
using WeaponScripts;

namespace PlayerScripts
{
    public class PlayerAttacker : MonoBehaviour
    {
        private PlayerAnimatorController _animController;
        private PlayerInputHandler _input;
        private PlayerController _playerController;
        public AttackAnimations lastAttack;

        private void Awake()
        {
            _animController = GetComponent<PlayerAnimatorController>();
            _input = GetComponent<PlayerInputHandler>();
            _playerController = GetComponent<PlayerController>();
        }

        public void HandleSkillWeaponCombo(WeaponDataSO weaponData)
        {
            if (!_input.comboFlag) return;
            _animController.SetParameter(AnimatorParameters.CanDoCombo, false);
            switch (lastAttack)
            {
                case AttackAnimations.Skill_A:
                    _animController.PlayTargetAnimation(weaponData.Skill_B.ToString(), true);
                    lastAttack = AttackAnimations.Skill_B;
                    break;
                case AttackAnimations.Skill_B:
                    _animController.PlayTargetAnimation(weaponData.Skill_C.ToString(), true);
                    lastAttack = AttackAnimations.Skill_C;
                    break;
                case AttackAnimations.Skill_C:
                    _animController.PlayTargetAnimation(weaponData.Skill_D.ToString(), true);
                    break;
                case AttackAnimations.Skill_D:
                    _animController.PlayTargetAnimation(weaponData.Skill_E.ToString(), true);
                    lastAttack = AttackAnimations.Skill_E;
                    break;
                case AttackAnimations.Skill_E:
                    _animController.PlayTargetAnimation(weaponData.Skill_F.ToString(), true);
                    break;
                case AttackAnimations.Skill_F:
                    _animController.PlayTargetAnimation(weaponData.Skill_A.ToString(), true);
                    lastAttack = AttackAnimations.Skill_A;
                    break;
                default:
                    lastAttack = AttackAnimations.Skill_A;
                    break;
            }
        }

        public void HandleLightWeaponCombo(WeaponDataSO weaponData)
        {
            if (!_input.comboFlag) return;
            _animController.SetParameter(AnimatorParameters.CanDoCombo, false);
            switch (lastAttack)
            {
                case AttackAnimations.OH_Light_Attack_1:
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_2.ToString(), true);
                    lastAttack = AttackAnimations.OH_Light_Attack_2;
                    break;
                case AttackAnimations.OH_Light_Attack_2:
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_3.ToString(), true);
                    lastAttack = AttackAnimations.OH_Light_Attack_3;
                    break;
                case AttackAnimations.OH_Light_Attack_3:
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_4.ToString(), true);
                    lastAttack = AttackAnimations.OH_Light_Attack_4;
                    break;
                case AttackAnimations.OH_Light_Attack_4:
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_5.ToString(), true);
                    lastAttack = AttackAnimations.OH_Light_Attack_5;
                    break;
                case AttackAnimations.OH_Light_Attack_5:
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_6.ToString(), true);
                    lastAttack = AttackAnimations.OH_Light_Attack_6;
                    break;
                case AttackAnimations.OH_Light_Attack_6:
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_1.ToString(), true);
                    lastAttack = AttackAnimations.OH_Light_Attack_1;
                    break;
                default:
                    lastAttack = AttackAnimations.OH_Light_Attack_1;
                    break;
            }
        }

        public void HandleHeavyWeaponCombo(WeaponDataSO weaponData)
        {
            if (!_input.comboFlag) return;
            _animController.SetParameter(AnimatorParameters.CanDoCombo, false);
            switch (lastAttack)
            {
                case AttackAnimations.OH_Heavy_Attack_1:
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_2.ToString(), true);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_2;
                    break;
                case AttackAnimations.OH_Heavy_Attack_2:
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_3.ToString(), true);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_3;
                    break;
                case AttackAnimations.OH_Heavy_Attack_3:
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_4.ToString(), true);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_4;
                    break;
                case AttackAnimations.OH_Heavy_Attack_4:
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_5.ToString(), true);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_5;
                    break;
                case AttackAnimations.OH_Heavy_Attack_5:
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_6.ToString(), true);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_6;
                    break;
                case AttackAnimations.OH_Heavy_Attack_6:
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_1.ToString(), true);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_1;
                    break;
                default:
                    lastAttack = AttackAnimations.OH_Heavy_Attack_1;
                    break;
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

        public void HandleSkillAttack(WeaponDataSO weaponData)
        {
            _animController.PlayTargetAnimation(weaponData.Skill_A.ToString(),true);
            lastAttack = weaponData.Skill_A;
        }
    }
}