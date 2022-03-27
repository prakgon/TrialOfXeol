using System;
using System.Collections;
using System.Collections.Generic;
using DummyScripts;
using Helpers;
using ParrelSync.NonCore;
using PlayerScripts;
using UnityEngine;
using static Helpers.Literals;

namespace WeaponScripts
{
    public class DamageCollider : MonoBehaviour, IMediatorUser
    {
        private PlayerMediator _mediator;
        private PlayerAnimatorController _playerAnimatorController;
        private Animator _animator;
        private Collider _damageCollider;
        private bool _canCheckAnimator;
        public WeaponDataSO weaponData;

        public void ConfigureMediator(PlayerMediator med)
        {
            _mediator = med;
        }

        private void Awake()
        {
            _damageCollider = GetComponent<Collider>();
            _damageCollider.gameObject.SetActive(true);
            _damageCollider.isTrigger = true;
            _damageCollider.enabled = false;
        }

        public void EnableDamageCollider() => _damageCollider.enabled = true;  
        

        public void DisableDamageCollider() => _damageCollider.enabled = false;
        
        private void OnTriggerEnter(Collider other)
        {
            
            if (other.CompareTag(Tags.Player.ToString()))
            {
                PlayerStats playerStats = other.GetComponent<PlayerStats>();

                if (playerStats != null)
                {
                    playerStats.TakeDamage(weaponData.damage);
                }
            }


            if (other.CompareTag(Tags.Dummy.ToString()))
            {
                DummyStats playerStats = other.GetComponent<DummyStats>();

                if (playerStats != null)
                {
                    playerStats.TakeDamage(weaponData.damage);
                }
            }

            if (other.CompareTag(Tags.Enemy.ToString()))
            {
                //EnemyStats enemyStats;
            }
            
        }
    }
}