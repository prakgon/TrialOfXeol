using System.Collections;
using Helpers;
using UnityEngine;

namespace PlayerScripts
{
    public class CollisionController : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer playerMeshRenderer;
        [SerializeField] private GameObject playerWeapon;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Literals.Tags.Weapon.ToString()) && other.gameObject != playerWeapon)
            {
                StartCoroutine(DamageFeedback());
            }
        }

        private IEnumerator DamageFeedback()
        {
            foreach (var material in playerMeshRenderer.materials)
            {
                material.color = Color.red;
            }

            yield return new WaitForSeconds(1f);

            foreach (var material in playerMeshRenderer.materials)
            {
                material.color = Color.white;
            }
        }
    }
}