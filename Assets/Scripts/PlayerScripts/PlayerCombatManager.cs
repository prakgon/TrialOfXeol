using System;
using Photon.Pun.Demo.PunBasics;
using static Helpers.Literals;
using UnityEngine;
using WeaponScripts;

namespace PlayerScripts
{
    public class PlayerCombatManager : MonoBehaviour
    {
        private PlayerAnimatorController _animController;
        private PlayerInputHandler _input;
        private PlayerController _playerController;
        public AttackAnimations lastAttack = AttackAnimations.InitialState;
        private PlayerEffectsManager _playerEffectsManager;

        private void Awake()
        {
            _animController = GetComponent<PlayerAnimatorController>();
            _input = GetComponent<PlayerInputHandler>();
            _playerController = GetComponent<PlayerController>();
            _playerEffectsManager = GetComponent<PlayerEffectsManager>();
        }

        #region Handle Combos
        public void HandleSkillWeaponCombo(WeaponDataSO weaponData)
        {
            //if (!_input.comboFlag) return;
            //_animController.SetParameter(AnimatorParameters.CanDoCombo, false);
            
            CanDoCombo();
            
            switch (lastAttack)
            {
                case AttackAnimations.Skill_A:
                    _animController.EnableCombo();
                    _animController.PlayTargetAnimation(weaponData.Skill_B.ToString(), true, 1);
                    lastAttack = AttackAnimations.Skill_B;
                    Debug.Log("Skill Combo state 2");
                    break;

                case AttackAnimations.Skill_B:
                    _animController.EnableCombo();
                    _animController.PlayTargetAnimation(weaponData.Skill_C.ToString(), true, 1);
                    lastAttack = AttackAnimations.Skill_C;
                    Debug.Log("Skill Combo state 3");
                    break;

                case AttackAnimations.Skill_C:
                    _animController.EnableCombo();
                    _animController.PlayTargetAnimation(weaponData.Skill_D.ToString(), true, 1);
                    lastAttack = AttackAnimations.Skill_D;
                    Debug.Log("Skill Combo state 4");
                    break;

                case AttackAnimations.Skill_D:
                    _animController.EnableCombo();
                    _animController.PlayTargetAnimation(weaponData.Skill_E.ToString(), true, 1);
                    lastAttack = AttackAnimations.Skill_E;
                    Debug.Log("Skill Combo state 5");
                    break;

                case AttackAnimations.Skill_E:
                    _animController.DisableCombo();
                    _animController.PlayTargetAnimation(weaponData.Skill_F.ToString(), true, 1);
                    lastAttack = AttackAnimations.Skill_F;
                    Debug.Log("Skill Combo state 6");
                    break;

                case AttackAnimations.Skill_F:
                    _animController.EnableCombo();
                    _animController.PlayTargetAnimation(weaponData.Skill_A.ToString(), true, 1);
                    lastAttack = AttackAnimations.Skill_A;
                    Debug.Log("Skill Combo state 1, but reset");
                    break;

                default:
                    _animController.EnableCombo();
                    _animController.PlayTargetAnimation(weaponData.Skill_A.ToString(), true, 1);
                    lastAttack = AttackAnimations.Skill_A;
                    Debug.Log("Skill Combo state 1, but default");
                    break;
            }
            
            _playerEffectsManager.PlayWeaponFX(false);
        }

        public void HandleLightWeaponCombo(WeaponDataSO weaponData)
        {
            //if (!_input.comboFlag) return;
            //_animController.SetParameter(AnimatorParameters.CanDoCombo, false);
           
            CanDoCombo();
            
            switch (lastAttack)
            {
                case AttackAnimations.OH_Light_Attack_1:
                    _animController.EnableCombo();
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_2.ToString(), true, 1);
                    lastAttack = AttackAnimations.OH_Light_Attack_2;
                    Debug.Log("Light Combo state 2");
                    break;

                case AttackAnimations.OH_Light_Attack_2:
                    _animController.EnableCombo();
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_3.ToString(), true, 1);
                    lastAttack = AttackAnimations.OH_Light_Attack_3;
                    Debug.Log("Light Combo state 3");
                    break;

                case AttackAnimations.OH_Light_Attack_3:
                    _animController.EnableCombo();
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_4.ToString(), true, 1);
                    lastAttack = AttackAnimations.OH_Light_Attack_4;
                    Debug.Log("Light Combo state 4");
                    break;

                case AttackAnimations.OH_Light_Attack_4:
                    _animController.EnableCombo();
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_5.ToString(), true, 1);
                    lastAttack = AttackAnimations.OH_Light_Attack_5;
                    Debug.Log("Light Combo state 5");
                    break;

                case AttackAnimations.OH_Light_Attack_5:
                    _animController.DisableCombo();
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_6.ToString(), true, 1);
                    lastAttack = AttackAnimations.OH_Light_Attack_6;
                    Debug.Log("Light Combo state 6");
                    break;

                case AttackAnimations.OH_Light_Attack_6:
                    _animController.EnableCombo();
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_1.ToString(), true, 1);
                    lastAttack = AttackAnimations.OH_Light_Attack_1;
                    Debug.Log("Light Combo state 1, but reset");
                    break;

                default:
                    _animController.EnableCombo();
                    _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_1.ToString(), true, 1);
                    lastAttack = AttackAnimations.OH_Light_Attack_1;
                    Debug.Log("Light Combo state 1, but default");
                    break;
            }
            
            _playerEffectsManager.PlayWeaponFX(false);
        }

        public void HandleHeavyWeaponCombo(WeaponDataSO weaponData)
        {
            //if (!_input.comboFlag) return;
           
            CanDoCombo();
            
            switch (lastAttack)
            {
                case AttackAnimations.OH_Heavy_Attack_1:
                    _animController.EnableCombo();
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_2.ToString(), true, 1);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_2;
                    Debug.Log("Heavy Combo state 2");
                    break;

                case AttackAnimations.OH_Heavy_Attack_2:
                    _animController.EnableCombo();
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_3.ToString(), true, 1);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_3;
                    Debug.Log("Heavy Combo state 3");
                    break;

                case AttackAnimations.OH_Heavy_Attack_3:
                    _animController.EnableCombo();
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_4.ToString(), true, 1);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_4;
                    Debug.Log("Heavy Combo state 4");
                    break;

                case AttackAnimations.OH_Heavy_Attack_4:
                    _animController.EnableCombo();
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_5.ToString(), true, 1);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_5;
                    Debug.Log("Heavy Combo state 5");
                    break;

                case AttackAnimations.OH_Heavy_Attack_5:
                    _animController.EnableCombo();
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_6.ToString(), true, 1);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_6;
                    Debug.Log("Heavy Combo state 6");
                    break;

                case AttackAnimations.OH_Heavy_Attack_6:
                    _animController.DisableCombo();
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_7.ToString(), true, 1);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_7;
                    Debug.Log("Heavy Combo state 7");
                    break;

                case AttackAnimations.OH_Heavy_Attack_7:
                    _animController.EnableCombo();
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_1.ToString(), true, 1);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_1;
                    Debug.Log("heavy reset");
                    break;

                default:
                    _animController.EnableCombo();
                    _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_1.ToString(), true, 1);
                    lastAttack = AttackAnimations.OH_Heavy_Attack_1;
                    Debug.Log("Heavy Combo state 1, but default");
                    break;
            }
            
            _playerEffectsManager.PlayWeaponFX(false);
        }

        private void CanDoCombo()
        {
            if (!_animController.GetBool(AnimatorParameters.CanDoCombo))
            {
                lastAttack = AttackAnimations.InitialState;
            }
        }
        #endregion
        
        
        public void HandleLightAttack(WeaponDataSO weaponData)
        {
            _animController.EnableCombo();
            _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_1.ToString(), true, 1);
            lastAttack = weaponData.OH_Light_Attack_1;
        }

        public void HandleHeavyAttack(WeaponDataSO weaponData)
        {
            _animController.EnableCombo();
            _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_1.ToString(), true, 1);
            lastAttack = weaponData.OH_Heavy_Attack_1;
        }

        public void HandleSkillAttack(WeaponDataSO weaponData)
        {
            _animController.EnableCombo();
            _animController.PlayTargetAnimation(weaponData.Skill_A.ToString(), true, 1);
            lastAttack = weaponData.Skill_A;
            Debug.Log(_animController.GetBool(AnimatorParameters.CanDoCombo));
        }
    }
}