using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Helpers;
using Photon.Pun;
using UnityEditor;
using UnityEngine;
using VisualFX;
using WeaponScripts;

public class PlayerInventory : MonoBehaviour
{
    private WeaponSlotManager _weaponSlotManager;

    [Header("Current Weapons")] public WeaponDataSO rightWeapon;
    public WeaponDataSO leftWeapon;

    [Header("Weapon Inventory Slots")] [SerializeField]
    private List<WeaponDataSO> weaponInventory;

    [Header("Weapon Repository")] [SerializeField]
    public WeaponRepositorySO weaponRepository;

    [SerializeField] private int index;

    [SerializeField] private PlayerEffectsManager playerEffectsManager;
    private PhotonView _photonView;

    private void Awake()
    {
        _weaponSlotManager = GetComponent<WeaponSlotManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
    }

    private void Start()
    {
        _photonView = PhotonView.Get(gameObject);
        _weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        _weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);

        index = 0;
        weaponInventory.Add(rightWeapon);
    }

    public void ChangeWeapon(int newValue)
    {
        index += newValue;
        index = index > weaponInventory.Count - 1 ? 0 : index < 0 ? weaponInventory.Count - 1 : index;
        SetCurrentWeapon(weaponInventory[index]);
        if (weaponInventory.Count > 1)
        {
            AudioManager.Instance.OneShot(Literals.AudioType.ChangeWeapon);
        }
    }

    public void AddWeapon(WeaponDataSO weaponDataSo)
    {
        for (var i = 0; i < weaponInventory.Count; i++)
        {
            if (weaponInventory[i].Equals(weaponDataSo))
            {
                SetCurrentWeapon(weaponInventory[i]);
                index = i;
                return;
            }
        }

        SetCurrentWeapon(weaponDataSo);
        weaponInventory.Add(weaponDataSo);
        index = weaponInventory.Count - 1;
        
        AudioManager.Instance.OneShot(Literals.AudioType.PowerUp);
    }

    [PunRPC]
    private void SetCurrentWeapon(int repoWeaponindex, bool isRemote = false)
    {
        SetCurrentWeapon(weaponRepository.GetWeapon(repoWeaponindex), isRemote);
    }

    private void SetCurrentWeapon(WeaponDataSO weaponDataSo, bool isRemote = false)
    {
        if (!_photonView.IsMine && !isRemote) return;
        if (!isRemote)
        {
            _photonView.RPC("SetCurrentWeapon", RpcTarget.Others, weaponRepository.GetWeaponIndex(weaponDataSo), true);
        }

        _weaponSlotManager.LoadWeaponOnSlot(weaponDataSo, false);
        rightWeapon = weaponDataSo;
        playerEffectsManager.SetCurrentDashFX(weaponDataSo.colorFX);
    }
}