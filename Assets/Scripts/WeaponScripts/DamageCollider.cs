using DummyScripts;
using Helpers;
using Photon.Pun;
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
        private GameObject _player; // This gameObject handles player reference collision with his own weapon

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

        private void Start()
        {
            FindPlayerReference();
        }

        private void FindPlayerReference()
        {
            // Refactor this
            _player = gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.gameObject;
        }

        public void EnableDamageCollider() => _damageCollider.enabled = true;  
        

        public void DisableDamageCollider() => _damageCollider.enabled = false;
        
        [PunRPC]
        private void OnTriggerEnter(Collider other)
        {
            
            if (other.CompareTag(Tags.Player.ToString()) && other.gameObject != _player)
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

            /*if (other.CompareTag(Tags.Enemy.ToString()))
            {
                //EnemyStats enemyStats;
            }*/
            
        }
    }
}