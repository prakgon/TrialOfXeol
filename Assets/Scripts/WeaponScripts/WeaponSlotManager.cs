using System;
using PlayerScripts;
using UnityEngine;

namespace WeaponScripts
{
    public class WeaponSlotManager : MonoBehaviour
    {
        public WeaponDataSO weaponItem;
        
        private WeaponHolderSlot _leftHandSlot;
        private WeaponHolderSlot _rightHandSlot;

        private DamageCollider _leftHandDamageCollider;
        private DamageCollider _rightHandDamageCollider;

        private PlayerEffectsManager _playerEffectsManager; 
        
        private PlayerStats _playerStats;
        
        private void Awake()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            _playerStats = GetComponent<PlayerStats>();
            
            foreach (var weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    _leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    _rightHandSlot = weaponSlot;
                }
            }

            _playerEffectsManager = GetComponent<PlayerEffectsManager>();
        }

        public void LoadWeaponOnSlot(WeaponDataSO weaponData, bool isLeft)
        {
            if (isLeft)
            {
                _leftHandSlot.LoadWeaponModel(weaponData);
                LoadLeftWeaponDamageCollider();
            }
            else
            {
                _rightHandSlot.LoadWeaponModel(weaponData);
                LoadRightWeaponDamageCollider();
            }
        }
        
        #region Handles Weapon's Damage Collider

        private void LoadLeftWeaponDamageCollider()
        {
            try
            {
                _leftHandDamageCollider = _leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                _playerEffectsManager.leftWeaponFX =
                    _leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
            }
            catch
            {
                Debug.Log("No weapon or damage collider on left hand slot.");
            }
        }

        private void LoadRightWeaponDamageCollider()
        {
            try
            {
                _rightHandDamageCollider = _rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                _playerEffectsManager.rightWeaponFX =
                    _rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
            }
            catch
            {
                Debug.Log("No weapon or damage collider on right hand slot.");
            }
        }
        
        public void OpenLeftDamageCollider() => _leftHandDamageCollider.EnableDamageCollider();
        public void OpenRightDamageCollider() => _rightHandDamageCollider.EnableDamageCollider();
        public void CloseLeftDamageCollider() => _leftHandDamageCollider.DisableDamageCollider();
        public void CloseRightDamageCollider() => _rightHandDamageCollider.DisableDamageCollider();
        #endregion

        #region Handles Weapon's Stamina Drain
        public void DrainStaminaLightAttack()
        {
            _playerStats.DrainStamina(Mathf.RoundToInt(weaponItem.baseStamina * weaponItem.lightStaminaMultiplier));
        }
        
        public void DrainStaminaHeavyAttack()
        {
            _playerStats.DrainStamina(Mathf.RoundToInt(weaponItem.baseStamina * weaponItem.heavyStaminaMultiplier));
        }

        #endregion
    }
}