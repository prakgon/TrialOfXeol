using System;
using UnityEditor.Animations;
using UnityEngine;
using WeaponScripts;

namespace PlayerScripts
{
    public class PlayerAttacker : MonoBehaviour
    {
        private PlayerAnimatorController _animController;

        private void Awake()
        {
            _animController = GetComponent<PlayerAnimatorController>();
        }

        public void HandleLightAttack(WeaponDataSO weaponData)
        {
            _animController.PlayTargetAnimation(weaponData.OH_Light_Attack_1.ToString(), true);
        }
        
        public void HandleHeavyAttack(WeaponDataSO weaponData)
        {
            _animController.PlayTargetAnimation(weaponData.OH_Heavy_Attack_1.ToString(), true);
        }
    }
}