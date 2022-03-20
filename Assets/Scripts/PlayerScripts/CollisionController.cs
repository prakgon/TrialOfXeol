using System.Collections;
using Helpers;
using TMPro;
using UnityEngine;
using WeaponScripts;

namespace PlayerScripts
{
    public class CollisionController : MonoBehaviour
    {
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private GameObject playerWeapon;
        [SerializeField] private SkinnedMeshRenderer playerMeshRenderer;
        [SerializeField] private TMP_Text playerTMPText;
        private float _currentHealth;
        
        private void Start() => InitializePlayer();

        private void InitializePlayer()
        {
            _currentHealth = playerData.maximumHealth;

            UpdateDebugUI();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Literals.Tags.Weapon.ToString()) && other.gameObject != playerWeapon)
            {
                var damage = other.gameObject.GetComponent<WeaponColliderController>().weaponData.damage;

                StartCoroutine(TakeDamage(damage));
            }
        }

        private IEnumerator TakeDamage(float damage)
        {
            DecreaseHealth(damage);
            
            UpdateDebugUI();
            
            DebugMaterialColor(Color.red);

            yield return new WaitForSeconds(1f);

            DebugMaterialColor(Color.white);
        }
        
        private void DecreaseHealth(float decrement) => _currentHealth -= decrement;

        private void UpdateDebugUI() =>
            SetDebugText(_currentHealth > 0 ? $"Current {gameObject.name} health: {_currentHealth}" : "Death");

        private void SetDebugText(string message) => playerTMPText.text = message;
        
        private void DebugMaterialColor(Color color)
        {
            foreach (var material in playerMeshRenderer.materials)
            {
                material.color = color;
            }
        }
    }
}