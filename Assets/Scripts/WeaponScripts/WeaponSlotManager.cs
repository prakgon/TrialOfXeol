using System;
using UnityEngine;

namespace WeaponScripts
{
    public class WeaponSlotManager : MonoBehaviour
    {
        private WeaponHolderSlot _leftHandSlot;
        private WeaponHolderSlot _rightHandSlot;

        private DamageCollider _leftHandDamageCollider;
        private DamageCollider _rightHandDamageCollider;

        private void Awake()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
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

    }
}