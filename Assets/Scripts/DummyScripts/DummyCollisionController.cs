using System.Collections;
using Helpers;
using TMPro;
using UnityEngine;
using WeaponScripts;

namespace DummyScripts
{
    public class DummyCollisionController : MonoBehaviour
    {
        [SerializeField] private DummyDataSO dummyData;
        [SerializeField] private TMP_Text dummyTMPText;
        private float _currentHealth;

        private void Start() => InitializeDummy();

        private void InitializeDummy()
        {
            _currentHealth = dummyData.maximumHealth;

            UpdateUI();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(Literals.Tags.Weapon.ToString())) return;

            var damage = other.gameObject.GetComponent<WeaponColliderController>().weaponData.damage;

            StartCoroutine(TakeDamage(damage));
        }

        private IEnumerator TakeDamage(float damageTaken)
        {
            DecreaseHealth(damageTaken);

            UpdateUI();

            GetComponent<SkinnedMeshRenderer>().material.color = Color.red;

            yield return new WaitForSeconds(1f);

            GetComponent<SkinnedMeshRenderer>().material.color = Color.white;
        }

        private void UpdateUI() =>
            SetText(_currentHealth > 0 ? $"Current dummy health: {_currentHealth}" : "Death");

        private void SetText(string message) => dummyTMPText.text = message;

        private void DecreaseHealth(float decrement) => _currentHealth -= decrement;
    }
}