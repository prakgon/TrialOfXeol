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
                    Debug.Log("Skill Combo state 2");
                    break;

                case AttackAnimations.Skill_B:
                    _animController.PlayTargetAnimation(weaponData.Skill_C.ToString(), true);
                    lastAttack = AttackAnimations.Skill_C;
                    Debug.Log("Skill Combo state 3");
                    break;

                case AttackAnimations.Skill_C:
                    _animController.PlayTargetAnimation(weaponData.Skill_D.ToString(), true);
                    lastAttack = AttackAnimations.Skill_D;
                    Debug.Log("Skill Combo state 4");
                    break;

                case AttackAnimations.Skill_D:
                    _animController.PlayTargetAnimation(weaponData.Skill_E.ToString(), true);
                    lastAttack = AttackAnimations.Skill_E;
                    Debug.Log("Skill Combo state 5");
                    break;
                
                case AttackAnimations.Skill_E:
                    _animController.PlayTargetAnimation(weaponData.Skill_F.ToString(), true);
                    lastAttack = AttackAnimations.Skill_F;
                    Debug.Log("Skill Combo state 6");
                    break;
                
                case AttackAnimations.Skill_F:
                    _animController.PlayTargetAnimation(weaponData.Skill_A.ToString(), true);
                    lastAttack = AttackAnimations.Skill_A;
                    Debug.Log("Skill Combo state 1, but reset");
                    break;
                
                default:
                    _animController.PlayTargetAnimation(weaponData.Skill_A.ToString(), true);
                    lastAttack = AttackAnimations.Skill_A;
                    Debug.Log("Skill Combo state 1, but default");

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
                    Debug.Log("Light Combo state 2");
                    break;
                    
                case AttackAnimations.OH_Light_Attack_2:
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_3.ToString(), true);
                    lastAttack = AttackAnimations.OH_Light_Attack_3;
                    Debug.Log("Light Combo state 3");
                    break;
                    
                case AttackAnimations.OH_Light_Attack_3:
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_4.ToString(), true);
                    lastAttack = AttackAnimations.OH_Light_Attack_4;
                    Debug.Log("Light Combo state 4");
                    break;
                    
                case AttackAnimations.OH_Light_Attack_4:
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_5.ToString(), true);
                    lastAttack = AttackAnimations.OH_Light_Attack_5;
                    Debug.Log("Light Combo state 5");
                    break;
                    
                case AttackAnimations.OH_Light_Attack_5:
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_6.ToString(), true);
                    lastAttack = AttackAnimations.OH_Light_Attack_6;
                    Debug.Log("Light Combo state 6");
                    break;
                    
                case AttackAnimations.OH_Light_Attack_6:
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_1.ToString(), true);
                    lastAttack = AttackAnimations.OH_Light_Attack_1;
                    Debug.Log("Light Combo state 1, but reset");
                    break;
                    
                default:
                    lastAttack = AttackAnimations.OH_Light_Attack_1;
                    Debug.Log("Light Combo state 1, but default");
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
                    Debug.Log("Heavy Combo state 2");
                    break;
                    
                case AttackAnimations.OH_Heavy_Attack_2:
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_3.ToString(), true);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_3;
                    Debug.Log("Heavy Combo state 3");
                    break;
                    
                case AttackAnimations.OH_Heavy_Attack_3:
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_4.ToString(), true);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_4;
                    Debug.Log("Heavy Combo state 4");
                    break;
                    
                case AttackAnimations.OH_Heavy_Attack_4:
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_5.ToString(), true);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_5;
                    Debug.Log("Heavy Combo state 5");
                    break;
                    
                case AttackAnimations.OH_Heavy_Attack_5:
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_6.ToString(), true);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_6;
                    Debug.Log("Heavy Combo state 6");
                    break;
                    
                case AttackAnimations.OH_Heavy_Attack_6:
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_7.ToString(), true);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_7;
                    Debug.Log("Heavy Combo state 7");
                    break;
                
                case AttackAnimations.OH_Heavy_Attack_7:
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_1.ToString(), true);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_1;
                    Debug.Log("Heavy Combo state 1, but reset");
                    break;
                    
                default:
                    lastAttack = AttackAnimations.OH_Heavy_Attack_1;
                    Debug.Log("Heavy Combo state 1, but default");
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
            _animController.PlayTargetAnimation(weaponData.Skill_A.ToString(), true);
            lastAttack = weaponData.Skill_A;
        }
    }
}