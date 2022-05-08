using System;
using Unity.Mathematics;
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
            /*_pickUpFX.Play();*/
            PlayFX();
            var destroy = GetComponent<PropDestroyer>();
            if (destroy != null)
            {
                destroy.DestroyProp(gameObject);
            }
        }
    }

    private void PlayFX()
    {
        var fxTransform = _pickUpFX.transform;
        var particles = Instantiate(_pickUpFX, fxTransform.position, fxTransform.rotation);
        Destroy(particles.gameObject, _pickUpFX.main.duration);
    }
}