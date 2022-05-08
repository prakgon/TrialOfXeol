using System;
using UnityEngine;
using WeaponScripts;


public class PickUpPlayer : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private WeaponDataSO _weaponDataSo;

    [SerializeField] private ParticleSystem _pickUpFX;

    private void Awake()
    {
        _pickUpFX.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            playerInventory.AddWeapon(_weaponDataSo);
            _pickUpFX.Play();
            //Destroy(gameObject);
        }
    }
}