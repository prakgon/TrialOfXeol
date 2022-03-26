using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using PlayerScripts;
using UnityEngine;

namespace WeaponScripts
{
    public class WeaponColliderController : MonoBehaviour, IMediatorUser
    {
        private PlayerMediator _mediator;
        private PlayerAnimatorController _playerAnimatorController;
        private CapsuleCollider _capsuleCollider;
        private bool _canCheckAnimator;
        public WeaponDataSO weaponData;

        public void ConfigureMediator(PlayerMediator med)
        {
            _mediator = med;
        }

        private void Start()
        {
            _playerAnimatorController = _mediator.PlayerAnimatorController;

            _capsuleCollider = GetComponent<CapsuleCollider>();

            EnableCollider(false);

            _canCheckAnimator = true;
        }

        private void Update()
        {
            if (!_canCheckAnimator) return;
            const bool enable = false;
            EnableCollider(enable);
        }

        private void EnableCollider(bool enable) => _capsuleCollider.enabled = enable;

        private void OnTriggerEnter(Collider other) => _canCheckAnimator = false;

        private IEnumerator OnTriggerExit(Collider other)
        {
            EnableCollider(false);

            yield return new WaitForSeconds(1f);

            _canCheckAnimator = true;
        }
    }
}