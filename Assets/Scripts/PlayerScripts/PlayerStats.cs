using System;
using System.Collections;
using System.Linq;
using Audio;
using Helpers;
using static Helpers.Literals;
using TMPro;
using UnityEngine;
using WeaponScripts;
using Photon.Pun;
using UIScripts;
using UnityEngine.UI;
using VisualFX;

namespace PlayerScripts
{
    public class PlayerStats : MonoBehaviour, IMediatorUser, IPunObservable
    {
        private PlayerDataSO _playerData;

        [SerializeField] private int healthLevel = 10;
        private int _maximumHealth;
        private int _currentHealth;

        [SerializeField] private int staminaLevel = 10;
        private int _maximumStamina;
        private float _currentStamina;
        [SerializeField] private float staminaRegenerationAmount = 20f;
        [SerializeField] private float staminaRegenTimer = 0f;
        [SerializeField] private float timeToStartStaminaRegen = 1f;

        [SerializeField] private int manaLevel = 10;
        private int _maximumMana;
        private int _currentMana;

        [SerializeField] private GameObject playerCanvas;
        private SliderBar _healthBar;
        private SliderBar _staminaBar;
        private SpriteSwapper _headSpriteSwapper;

        private PlayerAnimatorController _animatorController;
        private PlayerController _playerController;

        private TMP_Text _playerTMPText;
        private PlayerEffectsManager _effectsManager;
        private BloodEffects _bloodEffects;

        private GameObject _playerWeapon;

        private PlayerMediator _med;

        public float CurrentStamina => _currentStamina;
        public float CurrentHealth => _currentHealth;


        private void Start() => InitializePlayer();

        private void InitializePlayer()
        {
            /*var t = gameObject.transform.root;
            foreach(Transform tr in t)
            {
                if (!tr.CompareTag(Tags.PlayerCanvas.ToString())) continue;
                var o = tr.gameObject;
                Debug.Log(o, o);
            }*/
            _headSpriteSwapper = GetComponentInChildren<SpriteSwapper>();

            SliderBar onScreenHealthBar = GetComponentsInChildren<SliderBar>()
                .FirstOrDefault(r => r.CompareTag(Tags.OnScreenHealthBar.ToString()));
            SliderBar inWorldHealthBar = GetComponentsInChildren<SliderBar>()
                .FirstOrDefault(r => r.CompareTag(Tags.InWorldHealthBar.ToString()));

            SliderBar onScreenStaminaBar = GetComponentsInChildren<SliderBar>()
                .FirstOrDefault(r => r.CompareTag(Tags.OnScreenStaminaBar.ToString()));

            if (PhotonView.Get(gameObject).IsMine)
            {
                _healthBar = onScreenHealthBar;
                _staminaBar = onScreenStaminaBar;
                if (inWorldHealthBar is not null) inWorldHealthBar.gameObject.SetActive(false);
            }
            else
            {
                playerCanvas.SetActive(false);
                _healthBar = inWorldHealthBar;
                if (onScreenHealthBar is not null) onScreenHealthBar.gameObject.SetActive(false);
                if (onScreenStaminaBar is not null) onScreenStaminaBar.gameObject.SetActive(false);
            }


            _currentHealth = SetMaxHealthFormHealthLevel();
            _healthBar.SetMaxValue(_currentHealth);

            _currentStamina = SetMaxStaminaFromStaminaLevel();
            _staminaBar.SetMaxValue(_currentStamina);

            //UpdateDebugUI();

            _playerController = GetComponent<PlayerController>();
            _effectsManager = GetComponent<PlayerEffectsManager>();
            _bloodEffects = GetComponent<BloodEffects>();
        }

        [PunRPC]
        public void TakeDamage(int damage)
        {
            if (_playerController.isInvulnerable) return;
            if (_currentHealth <= 0) return;

            DecreaseHealth(damage);

            UpdateHealthBar();
            UpdateHead();

            //Debug
            //UpdateDebugUI();

            _animatorController.PlayTargetAnimation(DamageAnimations.Damage_01.ToString(), true, 1);

            _animatorController.PlayTargetAnimation(DamageAnimations.Damage_01.ToString(), true, 1);

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                _animatorController.PlayTargetAnimation(DamageAnimations.Damage_Die.ToString(), true, 1);
                _effectsManager.PlayDeathFX();
                GetComponent<MatchManager>().PlayerDied();
            }
        }

        public void DrainStamina(float drain)
        {
            _currentStamina -= drain;
            UpdateStaminaBar();
        }
        
        public void IncreaseStamina()
        {
            _currentStamina = _maximumStamina;
            UpdateStaminaBar();
        }

        public void RegenerateStamina()
        {
            if (_playerController.isInteracting || _playerController.isSprinting || _playerController.isJumping)
            {
                staminaRegenTimer = 0f;
            }
            else
            {
                staminaRegenTimer += Time.deltaTime;

                if (_currentStamina < _maximumStamina && staminaRegenTimer > timeToStartStaminaRegen)
                {
                    _currentStamina += staminaRegenerationAmount * Time.deltaTime;
                    _staminaBar.SetValue(Mathf.RoundToInt(_currentStamina));
                }
            }
        }

        [PunRPC]
        public bool HealPlayer(int heal)
        {
            if (!PhotonView.Get(gameObject).IsMine) return false;
            if (heal + _currentHealth <= _maximumHealth)
            {
                _currentHealth += heal;
                _effectsManager.PlayHealFX();
                UpdateHealthBar();
                UpdateHead();
                return true;
            }

            if (_currentHealth >= _maximumHealth) return false;
            
            _currentHealth = _maximumHealth;
            _effectsManager.PlayHealFX();
            UpdateHealthBar();
            UpdateHead();
            return true;
        }
        
        [PunRPC]
        public bool RestoreStamina(int stamina)
        {
            if (stamina + _currentStamina <= _maximumStamina)
            {
                _currentStamina += stamina;
                UpdateStaminaBar();
                return true;
            }
            
            if (_currentStamina >= _maximumStamina) return false;

            _currentStamina = _maximumStamina;
            UpdateStaminaBar();
            return true;
        }

        [PunRPC]
        private void DecreaseHealth(int decrement) => _currentHealth -= decrement;

        private int SetMaxHealthFormHealthLevel()
        {
            _maximumHealth = _playerData.increasedHealthByLevel * healthLevel;
            return _maximumHealth;
        }

        private void UpdateHealthBar() => _healthBar.SetValue(_currentHealth);
        private void UpdateHead() => _headSpriteSwapper.SetValue(_currentHealth);

        private int SetMaxStaminaFromStaminaLevel()
        {
            _maximumStamina = _playerData.increasedStaminaByLevel * staminaLevel;
            return _maximumStamina;
        }

        private void UpdateStaminaBar() => _staminaBar.SetValue(_currentStamina);
        private void SetDebugText(string message) => _playerTMPText.text = message;

        private void UpdateDebugUI() =>
            SetDebugText(_currentHealth > 0 ? $"Current {gameObject.name} health: {_currentHealth}" : "Death");


        public void PlayBloodVFX(Collider other)
        {
            /*var myPosition = transform.position;
            var otherPosition = other.transform.position;
            var differencePosition = otherPosition - myPosition;
            differencePosition = differencePosition.normalized;

            var collisionPoint = other.ClosestPoint(myPosition);

            Ray ray = new Ray(collisionPoint, differencePosition);
            Debug.DrawRay(ray.origin, ray.direction, Color.green, 5f);

            _bloodEffects.InstantiateBloodEffect(collisionPoint.x, collisionPoint.y, collisionPoint.z,
                differencePosition.x, differencePosition.y, differencePosition.z);*/

            var transformPosition = transform.position;
            var collisionPoint = other.ClosestPoint(transformPosition);
            var collisionNormal = collisionPoint - transformPosition;
            _bloodEffects.InstantiateBloodEffect(collisionPoint.x, collisionPoint.y, collisionPoint.z,
                collisionNormal.x, collisionNormal.y, collisionNormal.z);
        }


        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _playerData = _med.PlayerData;
            _playerWeapon = _med.PlayerWeapon;
            //_playerTMPText = _med.PlayerTMPText;
            _animatorController = _med.PlayerAnimatorController;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_currentHealth);
            }
            else
            {
                _currentHealth = (int)stream.ReceiveNext();
                if (_currentHealth < _maximumHealth)
                {
                    //UpdateDebugUI();
                    UpdateHealthBar();
                    //UpdateStaminaBar();
                }
            }
        }
    }
}