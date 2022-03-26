using UnityEngine;

namespace WeaponScripts
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOverride;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;

        public GameObject currentWeaponModel;

        public void UnloadWeapon()
        {
            if (currentWeaponModel != null)
            {
                currentWeaponModel.SetActive(false);
            }
        }

        public void UnloadWeaponAndDestroy()
        {
            if (currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }
        
        public void LoadWeaponModel(WeaponDataSO weaponData)
        {
            UnloadWeaponAndDestroy();
            
            if (weaponData == null)
            {
                return;
            }

            GameObject model = Instantiate(weaponData.modelPrefab);

            if (model != null)
            {
                model.transform.parent = parentOverride != null ? parentOverride : transform;

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            currentWeaponModel = model;
        }
    }
}