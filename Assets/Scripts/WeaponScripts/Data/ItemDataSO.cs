using Helpers;
using UnityEngine;

namespace WeaponScripts
{
    public abstract class ItemDataSO : ScriptableObject
    {
        [Header("Game logic related")]
        public GameObject modelPrefab;
        public Literals.ItemNames itemName;
        public string itemDescription;
        public Sprite itemIcon;
    }
}