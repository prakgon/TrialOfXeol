using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WeaponScripts;

public class PlayerInventory : MonoBehaviour
{
    private WeaponSlotManager _weaponSlotManager;
    
    public WeaponDataSO rightWeapon;
    public WeaponDataSO leftWeapon;


    private void Awake()
    {
        _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    private void Start()
    {
        _weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        _weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
    }
}
