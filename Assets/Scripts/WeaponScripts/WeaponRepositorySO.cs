using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WeaponScripts
{
    [CreateAssetMenu(fileName = "WeaponRepository", menuName = "ScriptableObjects/Weapon/Repository", order = 1)]
    public class WeaponRepositorySO : ScriptableObject
    {
        public List<WeaponDataSO> weapons;

        private void OnEnable()
        {
            weapons = Resources.LoadAll<WeaponDataSO>("ScriptableObjects/Weapons").ToList();
        }

        public WeaponDataSO GetWeapon(int index)
        {
            return weapons[index];
        }

        public int GetWeaponIndex(WeaponDataSO weapon)
        {
            return weapons.FindIndex(w => w.name == weapon.name);
        }
    }
}