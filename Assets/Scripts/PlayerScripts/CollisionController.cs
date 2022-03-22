using System.Collections;
using Helpers;
using TMPro;
using UnityEngine;
using WeaponScripts;
using Photon.Pun;
using UIScripts;

namespace PlayerScripts
{
    public class CollisionController : MonoBehaviour, IMediatorUser, IPunObservable
    {
        private PlayerDataSO _playerData;
        private GameObject _playerWeapon;
        private SkinnedMeshRenderer _playerMeshRenderer;
        private TMP_Text _playerTMPText;
        private float _currentHealth;
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
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Literals.Tags.Weapon.ToString()) && other.gameObject != _playerWeapon)
            {
                var damage = other.gameObject.GetComponent<WeaponColliderController>().weaponData.damage;
                StartCoroutine(TakeDamage(damage));
            }
        }

        private IEnumerator TakeDamage(float damage)
        {
            DecreaseHealth(damage);
            UpdateHealthBar();
            //Debug
            UpdateDebugUI();
            DebugMaterialColor(Color.red);
            yield return new WaitForSeconds(1f);
            DebugMaterialColor(Color.white);
        }
        
        private void DecreaseHealth(float decrement) => _currentHealth -= decrement;
        private void InitializeHealthBar() => _healthBar.SetMaxValue(_currentHealth);
        private void UpdateHealthBar() => _healthBar.SetValue(_currentHealth);
        private void SetDebugText(string message) => _playerTMPText.text = message;

        private void UpdateDebugUI() =>
            SetDebugText(_currentHealth > 0 ? $"Current {gameObject.name} health: {_currentHealth}" : "Death");

        private void DebugMaterialColor(Color color)
        {
            foreach (var material in _playerMeshRenderer.materials)
            {
                material.color = color;
            }
        }

        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _playerData = _med.PlayerData;
            _playerWeapon = _med.PlayerWeapon;
            _playerMeshRenderer = _med.PlayerMeshRenderer;
            _playerTMPText = _med.PlayerTMPText;
            _healthBar = _med.HealthBar;
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