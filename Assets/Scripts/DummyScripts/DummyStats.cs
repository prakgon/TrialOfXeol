using System.Collections;
using Helpers;
using TMPro;
using UnityEngine;
using WeaponScripts;

namespace DummyScripts
{
    public class DummyStats : MonoBehaviour
    {
        [SerializeField] private DummyDataSO dummyData;
        [SerializeField] private TMP_Text dummyTMPText;
        private float _currentHealth;
        private BloodEffects _bloodEffects;

        private void Start() => InitializeDummy();

        private void InitializeDummy()
        {
            _currentHealth = dummyData.maximumHealth;

            UpdateUI();
            
            _bloodEffects = GetComponent<BloodEffects>();
        }

        /*private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(Literals.Tags.Weapon.ToString())) return;

            var damage = other.gameObject.GetComponent<DamageCollider>().weaponData.damage;

            StartCoroutine(TakeDamage(damage));
        }*/
        

        public void TakeDamage(float damage, Collider other)
        {
            DecreaseHealth(damage);
            UpdateUI();
        }


        private void UpdateUI() =>
            SetText(_currentHealth > 0 ? $"Current dummy health: {_currentHealth}" : "Death");

        private void SetText(string message) => dummyTMPText.text = message;

        private void DecreaseHealth(float decrement) => _currentHealth -= decrement;
    }
}