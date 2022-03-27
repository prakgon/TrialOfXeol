using System.Collections;
using Helpers;
using TMPro;
using UnityEngine;
using WeaponScripts;
using Photon.Pun;
using UIScripts;

namespace PlayerScripts
{
    public class PlayerStats : MonoBehaviour, IMediatorUser, IPunObservable
    {
        private PlayerDataSO _playerData;
        private GameObject _playerWeapon;
        private TMP_Text _playerTMPText;
        private float _currentHealth;
        private PlayerAnimatorController _animatorController;
        private float _maximumHealth;
        private SliderBar _healthBar;
        private PlayerMediator _med;

        private void Start() => InitializePlayer();

        private void InitializePlayer()
        {
            _maximumHealth = _playerData.maximumHealth;
            _currentHealth = _maximumHealth;
            InitializeHealthBar();
            //Debug
            UpdateDebugUI();
        }

        [PunRPC]
        public void TakeDamage(float damage)
        {
            DecreaseHealth(damage);
            UpdateHealthBar();
            //Debug
            UpdateDebugUI();
            
            _animatorController.PlayTargetAnimation(Literals.DamageAnimations.Damage_01.ToString(), true);

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                _animatorController.PlayTargetAnimation(Literals.DamageAnimations.Damage_Die.ToString(), true);
                // Handle player death
            }
        }
        
        [PunRPC]
        private void DecreaseHealth(float decrement) => _currentHealth -= decrement;
        private void InitializeHealthBar() => _healthBar.SetMaxValue(_currentHealth);
        private void UpdateHealthBar() => _healthBar.SetValue(_currentHealth);
        private void SetDebugText(string message) => _playerTMPText.text = message;

        private void UpdateDebugUI() =>
            SetDebugText(_currentHealth > 0 ? $"Current {gameObject.name} health: {_currentHealth}" : "Death");

        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _playerData = _med.PlayerData;
            _playerWeapon = _med.PlayerWeapon;
            _playerTMPText = _med.PlayerTMPText;
            _healthBar = _med.HealthBar;
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
                this._currentHealth = (float) stream.ReceiveNext();
                if (_currentHealth < _maximumHealth)
                {
                    UpdateDebugUI();
                    UpdateHealthBar();
                }
            }
        }
    }
}