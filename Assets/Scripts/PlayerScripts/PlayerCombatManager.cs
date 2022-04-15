using System;
using Photon.Pun.Demo.PunBasics;
using static Helpers.Literals;
using UnityEngine;
using UnityEngine.Serialization;
using WeaponScripts;

namespace PlayerScripts
{
    public class PlayerCombatManager : MonoBehaviour
    {
        private PlayerAnimatorController _animController;
        private PlayerInputHandler _input;
        private PlayerController _playerController;
        private WeaponSlotManager _weaponSlotManager;
        
        
        public AttackAnimations LastAttack { get; set; } = AttackAnimations.InitialState;
        private PlayerEffectsManager _playerEffectsManager;

        private void Awake()
        {
            _animController = GetComponent<PlayerAnimatorController>();
            _input = GetComponent<PlayerInputHandler>();
            _playerController = GetComponent<PlayerController>();
            _weaponSlotManager = GetComponent<WeaponSlotManager>();
            _playerEffectsManager = GetComponent<PlayerEffectsManager>();
        }

        #region Handle Combos
        public void HandleSkillWeaponCombo(WeaponDataSO weaponData)
        {
            CanDoCombo();
            
            _weaponSlotManager.weaponItem = weaponData;

            switch (LastAttack)
            {
                case AttackAnimations.Skill_A:
                    _animController.EnableCombo(weaponData.Skill_B, this);
                    Debug.Log("Skill Combo state 2");
                    break;

                case AttackAnimations.Skill_B:
                    _animController.EnableCombo(weaponData.Skill_C, this);
                    Debug.Log("Skill Combo state 3");
                    break;

                case AttackAnimations.Skill_C:
                    _animController.EnableCombo(weaponData.Skill_D, this);
                    Debug.Log("Skill Combo state 4");
                    break;

                case AttackAnimations.Skill_D:
                    _animController.EnableCombo(weaponData.Skill_E, this);
                    Debug.Log("Skill Combo state 5");
                    break;

                case AttackAnimations.Skill_E:
                    _animController.DisableCombo(weaponData.Skill_F, this);
                    Debug.Log("Skill Combo state 6");
                    break;

                case AttackAnimations.Skill_F:
                    _animController.EnableCombo(weaponData.Skill_A, this);
                    Debug.Log("Skill Combo state 1, but reset");
                    break;

                default:
                    _animController.EnableCombo(weaponData.Skill_A, this);
                    Debug.Log("Skill Combo state 1, but default");
                    break;
            }
            
            _playerEffectsManager.PlayWeaponTrailFX(false);
        }

        public void HandleLightWeaponCombo(WeaponDataSO weaponData)
        {
            CanDoCombo();
            
            _weaponSlotManager.weaponItem = weaponData;
            
            _weaponSlotManager.DrainStaminaLightAttack();

            switch (LastAttack)
            {
                case AttackAnimations.OH_Light_Attack_1:
                    _animController.EnableCombo(weaponData.OH_Light_Attack_2, this);
                    Debug.Log("Light Combo state 2");
                    break;

                case AttackAnimations.OH_Light_Attack_2:
                    _animController.EnableCombo(weaponData.OH_Light_Attack_3, this);
                    Debug.Log("Light Combo state 3");
                    break;

                case AttackAnimations.OH_Light_Attack_3:
                    _animController.EnableCombo(weaponData.OH_Light_Attack_4, this);
                    Debug.Log("Light Combo state 4");
                    break;

                case AttackAnimations.OH_Light_Attack_4:
                    _animController.EnableCombo(weaponData.OH_Light_Attack_5, this);
                    Debug.Log("Light Combo state 5");
                    break;

                case AttackAnimations.OH_Light_Attack_5:
                    _animController.DisableCombo(weaponData.OH_Light_Attack_6, this);
                    Debug.Log("Light Combo state 6");
                    break;

                case AttackAnimations.OH_Light_Attack_6:
                    _animController.EnableCombo(weaponData.OH_Light_Attack_1, this);
                    Debug.Log("Light Combo state 1, but reset");
                    break;

                default:
                    _animController.EnableCombo(weaponData.OH_Light_Attack_1, this);
                    Debug.Log("Light Combo state 1, but default");
                    break;
            }
            
            _playerEffectsManager.PlayWeaponTrailFX(false);
        }

        public void HandleHeavyWeaponCombo(WeaponDataSO weaponData)
        {
            CanDoCombo();
            
            _weaponSlotManager.weaponItem = weaponData;
            
            _weaponSlotManager.DrainStaminaHeavyAttack();
            
            switch (LastAttack)
            {
                case AttackAnimations.OH_Heavy_Attack_1:
                    _animController.EnableCombo(weaponData.OH_Heavy_Attack_2, this);
                    Debug.Log("Heavy Combo state 2");
                    break;

                case AttackAnimations.OH_Heavy_Attack_2:
                    _animController.EnableCombo(weaponData.OH_Heavy_Attack_3, this);
                    Debug.Log("Heavy Combo state 3");
                    break;

                case AttackAnimations.OH_Heavy_Attack_3:
                    _animController.EnableCombo(weaponData.OH_Heavy_Attack_4, this);
                    Debug.Log("Heavy Combo state 4");
                    break;

                case AttackAnimations.OH_Heavy_Attack_4:
                    _animController.EnableCombo(weaponData.OH_Heavy_Attack_5, this);
                    Debug.Log("Heavy Combo state 5");
                    break;

                case AttackAnimations.OH_Heavy_Attack_5:
                    _animController.EnableCombo(weaponData.OH_Heavy_Attack_6, this);
                    Debug.Log("Heavy Combo state 6");
                    break;

                case AttackAnimations.OH_Heavy_Attack_6:
                    _animController.DisableCombo(weaponData.OH_Heavy_Attack_7, this);
                    Debug.Log("Heavy Combo state 7");
                    break;

                case AttackAnimations.OH_Heavy_Attack_7:
                    _animController.EnableCombo(weaponData.OH_Heavy_Attack_1, this);
                    Debug.Log("heavy reset");
                    break;

                default:
                    _animController.EnableCombo(weaponData.OH_Heavy_Attack_1, this);
                    Debug.Log("Heavy Combo state 1, but default");
                    break;
            }
            
            _playerEffectsManager.PlayWeaponTrailFX(false);
        }

        private void CanDoCombo()
        {
            if (!_animController.GetBool(AnimatorParameters.CanDoCombo))
            {
                LastAttack = AttackAnimations.InitialState;
            }
        }
        #endregion
    }
}