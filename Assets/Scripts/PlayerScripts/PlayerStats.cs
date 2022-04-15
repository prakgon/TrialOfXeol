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

        [SerializeField] private int healthLevel = 10;
        private int _maximumHealth;
        private int _currentHealth;

        [SerializeField] private int staminaLevel = 10;
        private int _maximumStamina;
        private int _currentStamina;
        
        [SerializeField] private int manaLevel = 10;
        private int _maximumMana;
        private int _currentMana;
        
        private SliderBar _healthBar;
        private SliderBar _staminaBar;
        private PlayerAnimatorController _animatorController;
        
        private TMP_Text _playerTMPText;
        private PlayerEffectsManager _effectsManager;
        
        private GameObject _playerWeapon;
        
        private PlayerMediator _med;


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

            SliderBar onScreenHealthBar = GetComponentsInChildren<SliderBar>().FirstOrDefault(r => r.CompareTag(Tags.OnScreenHealthBar.ToString())); 
            SliderBar inWorldHealthBar = GetComponentsInChildren<SliderBar>().FirstOrDefault(r => r.CompareTag(Tags.InWorldHealthBar.ToString())); 

            SliderBar onScreenStaminaBar = GetComponentsInChildren<SliderBar>().FirstOrDefault(r => r.CompareTag(Tags.OnScreenStaminaBar.ToString()));
            
            if (PhotonView.Get(gameObject).IsMine)
            {
                _healthBar = onScreenHealthBar;
                _staminaBar = onScreenStaminaBar;
                if (inWorldHealthBar is not null) inWorldHealthBar.gameObject.SetActive(false);
            }
            else
            {
                _healthBar = inWorldHealthBar;
                if (onScreenHealthBar is not null) onScreenHealthBar.gameObject.SetActive(false);
                if (onScreenStaminaBar is not null) onScreenStaminaBar.gameObject.SetActive(false);
            }
            
            _currentHealth = SetMaxHealthFormHealthLevel();
            _healthBar.SetMaxValue(_currentHealth);
            
            _currentStamina = SetMaxStaminaFromStaminaLevel();
            _staminaBar.SetMaxValue(_currentStamina);
            
            UpdateDebugUI();

            _effectsManager = GetComponent<PlayerEffectsManager>();
        }

        [PunRPC]
        public void TakeDamage(int damage)
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

        public void DrainStamina(int drain)
        {
            DecreaseStamina(drain);
            UpdateStaminaBar();
        }
        
        [PunRPC]
        private void DecreaseHealth(int decrement) => _currentHealth -= decrement;
        private int SetMaxHealthFormHealthLevel()
        {
            _maximumHealth = _playerData.increasedHealthByLevel * healthLevel;
            return _maximumHealth;
        }
        private void UpdateHealthBar() => _healthBar.SetValue(_currentHealth);
        private void IncreaseStamina(int increment) => _currentStamina += increment;
        private void DecreaseStamina(int decrement) => _currentStamina -= decrement;
        private int SetMaxStaminaFromStaminaLevel()
        {
            _maximumStamina = _playerData.increasedStaminaByLevel * staminaLevel;
            return _maximumStamina;
        }
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
                _currentHealth = (int) stream.ReceiveNext();
                _currentStamina = (int) stream.ReceiveNext();
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