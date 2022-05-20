using System;
using Audio;
using Helpers;
using Photon.Pun;
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
        if (!PhotonView.Get(other.gameObject).IsMine) return;
        var playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            playerInventory.AddWeapon(_weaponDataSo);

            PlayFX();        
            //AudioManager.Instance.OneShot(Literals.AudioType.PowerUp);


            var destroy = GetComponent<PropDestroyer>();
            if (destroy != null)
            {
                destroy.DestroyProp(gameObject);
            }
        }
    }

    [PunRPC]
    private void PlayFX(bool isRemote = false)
    {
        if (!isRemote)
        {
            PhotonView.Get(gameObject).RPC("PlayFX", RpcTarget.Others, true);
        }

        var fxTransform = _pickUpFX.transform;
        var particles = Instantiate(_pickUpFX, fxTransform.position, fxTransform.rotation);
        Destroy(particles.gameObject, _pickUpFX.main.duration);
    }
}