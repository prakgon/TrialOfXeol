using System;
using System.Collections;
using System.Linq;
using Helpers;
using static Helpers.Literals;
using TMPro;
using UnityEngine;
using WeaponScripts;
using Photon.Pun;
using UIScripts;
using UnityEngine.UI;

namespace PlayerScripts
{
    public class PlayerStats : MonoBehaviour, IMediatorUser, IPunObservable
    {
        private PlayerDataSO _playerData;
        private GameObject _playerWeapon;
        private TMP_Text _playerTMPText;
        private float _currentHealth;
        private float _currentStamina;
        private PlayerAnimatorController _animatorController;
        private float _maximumHealth;
        private float _maximumStamina;
        private SliderBar _healthBar;
        [SerializeField] private SliderBar _staminaBar;
        private PlayerEffectsManager _effectsManager;
        private PlayerMediator _med;

        private void Start() => InitializePlayer();

        private void InitializePlayer()
        {
            SliderBar onScreenHealthBar = GetComponentsInChildren<SliderBar>().FirstOrDefault(r => r.CompareTag(Tags.OnScreenHealthBar.ToString())); 
            SliderBar inWorldHealthBar = GetComponentsInChildren<SliderBar>().FirstOrDefault(r => r.CompareTag(Tags.InWorldHealthBar.ToString())); 

            if (PhotonView.Get(gameObject).IsMine)
            {
                _healthBar = onScreenHealthBar;
                if (inWorldHealthBar is not null) inWorldHealthBar.gameObject.SetActive(false);
            }
            else
            {
                _healthBar = inWorldHealthBar;
                if (onScreenHealthBar is not null) onScreenHealthBar.gameObject.SetActive(false);
            }

            _maximumHealth = _playerData.maximumHealth;
            _currentHealth = _maximumHealth;
            _maximumStamina = _playerData.maximumStamina;
            _currentStamina = _maximumStamina;
            InitializeHealthBar();
            //InitializeStaminaBar();

            
            //Debug
            UpdateDebugUI();

            _effectsManager = GetComponent<PlayerEffectsManager>();
        }

        [PunRPC]
        public void TakeDamage(float damage)
        {
            DecreaseHealth(damage);
            UpdateHealthBar();
            //Debug
            UpdateDebugUI();
            
            _animatorController.PlayTargetAnimation(DamageAnimations.Damage_01.ToString(), true,1);

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                _animatorController.PlayTargetAnimation(DamageAnimations.Damage_Die.ToString(), true, 1);
                _effectsManager.PlayDeathFX();
                // Handle player death
            }
        }
        
        [PunRPC]
        private void DecreaseHealth(float decrement) => _currentHealth -= decrement;
        private void InitializeHealthBar() => _healthBar.SetMaxValue(_currentHealth);
        private void UpdateHealthBar() => _healthBar.SetValue(_currentHealth);
        private void IncreaseStamina(float increment) => _currentStamina += increment;
        private void DecreaseStamina(float decrement) => _currentStamina -= decrement;
        private void InitializeStaminaBar() => _staminaBar.SetMaxValue(_currentStamina);
        private void UpdateStaminaBar() => _staminaBar.SetValue(_currentStamina);
        private void SetDebugText(string message) => _playerTMPText.text = message;

        private void UpdateDebugUI() =>
            SetDebugText(_currentHealth > 0 ? $"Current {gameObject.name} health: {_currentHealth}" : "Death");

        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _playerData = _med.PlayerData;
            _playerWeapon = _med.PlayerWeapon;
            _playerTMPText = _med.PlayerTMPText;
            _animatorController = _med.PlayerAnimatorController;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_currentHealth);
                stream.SendNext(_currentStamina);
            }
            else
            {
                _currentHealth = (float) stream.ReceiveNext();
                _currentStamina = (float) stream.ReceiveNext();
                if (_currentHealth < _maximumHealth || _currentStamina < _maximumStamina)
                {
                    UpdateDebugUI();
                    UpdateHealthBar();
                    //UpdateStaminaBar();
                }
            }
        }
    }
}