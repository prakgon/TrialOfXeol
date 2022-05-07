using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WeaponScripts;

public class PlayerInventory : MonoBehaviour
{
    public enum WeaponSlots
    {
        Primary,
        Secondary,
    }

    private WeaponSlotManager _weaponSlotManager;

    [Header("Current Weapons")]
    public WeaponDataSO rightWeapon;
    public WeaponDataSO leftWeapon;

    [Header("Weapon Inventory Slots")]
    public WeaponDataSO firstWeapon;
    public WeaponDataSO secondaryWeapon;

    public WeaponSlots nextWeaponSlot;
    
    [SerializeField] private PlayerEffectsManager playerEffectsManager;
    
    [Header("Consumable Inventory")]
    public ItemDataSO[] consumableInventory;

    private void Awake()
    {
        _weaponSlotManager = GetComponent<WeaponSlotManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
    }

    private void Start()
    {
        _weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        _weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
    }

    public void ChangeWeapon()
    {
        switch (nextWeaponSlot)
        {
            case WeaponSlots.Primary:
                _weaponSlotManager.LoadWeaponOnSlot(firstWeapon, false);
                rightWeapon = firstWeapon;
                playerEffectsManager.SetCurrentDashFX(firstWeapon.colorFX);
                nextWeaponSlot = WeaponSlots.Secondary;
                break;
            case WeaponSlots.Secondary:
                _weaponSlotManager.LoadWeaponOnSlot(secondaryWeapon, false);
                rightWeapon = secondaryWeapon;
                playerEffectsManager.SetCurrentDashFX(secondaryWeapon.colorFX);
                nextWeaponSlot = WeaponSlots.Primary;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void AddWeapon(WeaponDataSO weaponDataSo)
    {
        _weaponSlotManager.LoadWeaponOnSlot(weaponDataSo, false);
        rightWeapon = weaponDataSo;
        playerEffectsManager.SetCurrentDashFX(weaponDataSo.colorFX);
    }
}