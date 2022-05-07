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
        private PlayerStats _playerStats;
        private WeaponSlotManager _weaponSlotManager;

        public AttackAnimations LastAttack { get; set; } = AttackAnimations.InitialState;
        private PlayerEffectsManager _playerEffectsManager;

        private void Awake()
        {
            _animController = GetComponent<PlayerAnimatorController>();
            _input = GetComponent<PlayerInputHandler>();
            _playerController = GetComponent<PlayerController>();
            _playerStats = GetComponent<PlayerStats>();
            _weaponSlotManager = GetComponent<WeaponSlotManager>();
            _playerEffectsManager = GetComponent<PlayerEffectsManager>();
        }

        #region Handle Combos

        public void HandleSkillWeaponCombo(WeaponDataSO weaponData)
        {
            if (_playerStats.CurrentStamina <= 0)
                return;

            CanDoCombo();

            _weaponSlotManager.weaponItem = weaponData;

            if (_playerController.isSprinting)
            {
                _animController.EnableCombo(weaponData.OH_Sprint_Heavy_Attack, this, AttackAnimations.OH_Sprint_Heavy_Attack);
                Debug.Log("Sprint light attack");
                _playerEffectsManager.PlayDashFX();

                _playerEffectsManager.PlayDashFX();
            }
            else
            {
                switch (LastAttack)
                {
                    case AttackAnimations.Skill_A:
                        _animController.EnableCombo(weaponData.Skill_B, this, AttackAnimations.Skill_B);
                        Debug.Log("Skill Combo state 2");
                        break;

                    case AttackAnimations.Skill_B:
                        _animController.EnableCombo(weaponData.Skill_C, this, AttackAnimations.Skill_C);
                        Debug.Log("Skill Combo state 3");
                        break;

                    case AttackAnimations.Skill_C:
                        _animController.EnableCombo(weaponData.Skill_D, this, AttackAnimations.Skill_D);
                        Debug.Log("Skill Combo state 4");
                        break;

                    case AttackAnimations.Skill_D:
                        _animController.EnableCombo(weaponData.Skill_E, this, AttackAnimations.Skill_E);
                        Debug.Log("Skill Combo state 5");
                        break;

                    case AttackAnimations.Skill_E:
                        _animController.DisableCombo(weaponData.Skill_F, this, AttackAnimations.Skill_F);
                        Debug.Log("Skill Combo state 6");
                        break;

                    case AttackAnimations.Skill_F:
                        _animController.EnableCombo(weaponData.Skill_A, this, AttackAnimations.Skill_A);
                        Debug.Log("Skill Combo state 1, but reset");
                        break;

                    default:
                        _animController.EnableCombo(weaponData.Skill_A, this, AttackAnimations.Skill_A);
                        Debug.Log("Skill Combo state 1, but default");
                        break;
                }
            }

            _playerEffectsManager.PlayWeaponTrailFX(false);
        }

        public void HandleLightWeaponCombo(WeaponDataSO weaponData)
        {
            if (_playerStats.CurrentStamina <= 0)
                return;

            CanDoCombo();

            _weaponSlotManager.weaponItem = weaponData;

            _weaponSlotManager.DrainStaminaLightAttack();

            if (_playerController.isSprinting)
            {
                _animController.EnableCombo(weaponData.OH_Sprint_Light_Attack, this, AttackAnimations.OH_Sprint_Light_Attack);
                Debug.Log("Sprint light attack");
                _playerEffectsManager.PlayDashFX();
            }
            else
            {
                switch (LastAttack)
                {
                    case AttackAnimations.OH_Light_Attack_1:
                        _animController.EnableCombo(weaponData.OH_Light_Attack_2, this, AttackAnimations.OH_Light_Attack_2);
                        Debug.Log("Light Combo state 2");
                        break;

                    case AttackAnimations.OH_Light_Attack_2:
                        _animController.EnableCombo(weaponData.OH_Light_Attack_3, this, AttackAnimations.OH_Light_Attack_3);
                        Debug.Log("Light Combo state 3");
                        break;

                    case AttackAnimations.OH_Light_Attack_3:
                        _animController.EnableCombo(weaponData.OH_Light_Attack_4, this, AttackAnimations.OH_Light_Attack_4);
                        Debug.Log("Light Combo state 4");
                        break;

                    case AttackAnimations.OH_Light_Attack_4:
                        _animController.EnableCombo(weaponData.OH_Light_Attack_5, this, AttackAnimations.OH_Light_Attack_5);
                        Debug.Log("Light Combo state 5");
                        break;

                    case AttackAnimations.OH_Light_Attack_5:
                        _animController.DisableCombo(weaponData.OH_Light_Attack_6, this, AttackAnimations.OH_Light_Attack_6);
                        Debug.Log("Light Combo state 6");
                        break;

                    case AttackAnimations.OH_Light_Attack_6:
                        _animController.EnableCombo(weaponData.OH_Light_Attack_1, this, AttackAnimations.OH_Light_Attack_1);
                        Debug.Log("Light Combo state 1, but reset");
                        break;

                    default:
                        _animController.EnableCombo(weaponData.OH_Light_Attack_1, this, AttackAnimations.OH_Light_Attack_1);
                        Debug.Log("Light Combo state 1, but default");
                        break;
                }
            }

            _playerEffectsManager.PlayWeaponTrailFX(false);
        }

        public void HandleHeavyWeaponCombo(WeaponDataSO weaponData)
        {
            if (_playerStats.CurrentStamina <= 0)
                return;

            CanDoCombo();

            _weaponSlotManager.weaponItem = weaponData;

            _weaponSlotManager.DrainStaminaHeavyAttack();
            if (_playerController.isSprinting)
            {
                _animController.EnableCombo(weaponData.OH_Sprint_Heavy_Attack, this, AttackAnimations.OH_Sprint_Heavy_Attack);
                Debug.Log("Sprint light attack");
                _playerEffectsManager.PlayDashFX();
            }
            else
            {
                switch (LastAttack)
                {
                    case AttackAnimations.OH_Heavy_Attack_1:
                        _animController.EnableCombo(weaponData.OH_Heavy_Attack_2, this, AttackAnimations.OH_Heavy_Attack_2);
                        Debug.Log("Heavy Combo state 2");
                        break;

                    case AttackAnimations.OH_Heavy_Attack_2:
                        _animController.EnableCombo(weaponData.OH_Heavy_Attack_3, this, AttackAnimations.OH_Heavy_Attack_3);
                        Debug.Log("Heavy Combo state 3");
                        break;

                    case AttackAnimations.OH_Heavy_Attack_3:
                        _animController.EnableCombo(weaponData.OH_Heavy_Attack_4, this, AttackAnimations.OH_Heavy_Attack_4);
                        Debug.Log("Heavy Combo state 4");
                        break;

                    case AttackAnimations.OH_Heavy_Attack_4:
                        _animController.EnableCombo(weaponData.OH_Heavy_Attack_5, this, AttackAnimations.OH_Heavy_Attack_5);
                        Debug.Log("Heavy Combo state 5");
                        break;

                    case AttackAnimations.OH_Heavy_Attack_5:
                        _animController.EnableCombo(weaponData.OH_Heavy_Attack_6, this, AttackAnimations.OH_Heavy_Attack_6);
                        Debug.Log("Heavy Combo state 6");
                        break;

                    case AttackAnimations.OH_Heavy_Attack_6:
                        _animController.DisableCombo(weaponData.OH_Heavy_Attack_7, this, AttackAnimations.OH_Heavy_Attack_7);
                        Debug.Log("Heavy Combo state 7");
                        break;

                    case AttackAnimations.OH_Heavy_Attack_7:
                        _animController.EnableCombo(weaponData.OH_Heavy_Attack_1, this, AttackAnimations.OH_Heavy_Attack_1);
                        Debug.Log("heavy reset");
                        break;

                    default:
                        _animController.EnableCombo(weaponData.OH_Heavy_Attack_1, this, AttackAnimations.OH_Heavy_Attack_1);
                        Debug.Log("Heavy Combo state 1, but default");
                        break;
                }
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