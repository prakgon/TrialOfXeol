using System;
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
        [SerializeField] private GameObject sparksFX;

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
            _player = gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent
                .parent.parent.gameObject;
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
                    playerStats.TakeDamage(weaponData.baseDamage);
                    playerStats.PlayBloodVFX(other);
                    AudioManager.Instance.PlayAtPoint(Literals.AudioType.HitSlash, transform.position);
                }
            }


            if (other.CompareTag(Tags.Dummy.ToString()))
            {
                DummyStats playerStats = other.GetComponent<DummyStats>();

                if (playerStats != null)
                {
                    playerStats.TakeDamage(weaponData.baseDamage, other);
                    AudioManager.Instance.PlayAtPoint(Literals.AudioType.HitSlash, transform.position);
                }
            }


            if (other.CompareTag(Tags.Ground.ToString()))
            {
                SparksFXOnGround(other);
                AudioManager.Instance.PlayAtPoint(Literals.AudioType.SteelSlash, transform.position);
            }

            if (other.CompareTag(Tags.Wall.ToString()))
            {
                SparksFXOnWall(other);
                AudioManager.Instance.PlayAtPoint(Literals.AudioType.SteelSlash, transform.position);
            }
        }


        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Tags.Player.ToString()) && other.gameObject != _player)
            {
                PlayerStats playerStats = other.GetComponent<PlayerStats>();

                if (playerStats != null)
                {
                    playerStats.PlayBloodVFX(other);
                }
            }
            
            if (other.CompareTag(Tags.Ground.ToString()))
            {
                SparksFXOnGround(other);
            }

            if (other.CompareTag(Tags.Wall.ToString()))
            {
                SparksFXOnWall(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.Player.ToString()) && other.gameObject != _player)
            {
                PlayerStats playerStats = other.GetComponent<PlayerStats>();

                if (playerStats != null)
                {
                    playerStats.PlayBloodVFX(other);
                }
            }
        }

        private void SparksFXOnGround(Collider other)
        {
            var collisionPoint = other.ClosestPointOnBounds(transform.position);
            var collisionNormal = transform.position - collisionPoint;
            Ray hit = new Ray(collisionPoint, collisionNormal);
            Debug.DrawRay(hit.origin, hit.direction, Color.red, 10);
            var tmpRotation = Quaternion.LookRotation(collisionNormal);
            var particles = Instantiate(sparksFX, collisionPoint,
                tmpRotation);
            Destroy(particles.gameObject, sparksFX.GetComponent<ParticleSystem>().main.startLifetime.constantMax);
        }
        
        private void SparksFXOnWall(Collider other)
        {
            var collisionPoint = other.ClosestPoint(transform.position);
            //var collisionNormal = collisionPoint - transform.position; to swap the direction
            var collisionNormal = transform.position - collisionPoint;
            Ray hit = new Ray(collisionPoint, collisionNormal);
            Debug.DrawRay(hit.origin, hit.direction, Color.red, 10);
            var tmpRotation = Quaternion.LookRotation(collisionNormal);
            var particles = Instantiate(sparksFX, collisionPoint,
                tmpRotation);
            Destroy(particles.gameObject, sparksFX.GetComponent<ParticleSystem>().main.startLifetime.constantMax);
        }
    }
}