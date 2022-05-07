using System;
using UnityEngine;
using WeaponScripts;


    public class PickUpPlayer : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private WeaponDataSO _weaponDataSo;


        private void OnTriggerEnter(Collider other)
        {
            var playerInventory = other.GetComponent<PlayerInventory>();

            if (playerInventory != null)
            {
                playerInventory.AddWeapon(_weaponDataSo);
                // playerStats.PlayBloodVFX(other);
            }
        }
    }
