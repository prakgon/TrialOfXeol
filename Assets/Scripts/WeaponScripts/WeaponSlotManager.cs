using System;
using UnityEngine;

namespace WeaponScripts
{
    public class WeaponSlotManager : MonoBehaviour
    {
        private WeaponHolderSlot _leftHandSlot;
        private WeaponHolderSlot _rightHandSlot;

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
            }
            else
            {
                _rightHandSlot.LoadWeaponModel(weaponData);
            }
        }
    }
}