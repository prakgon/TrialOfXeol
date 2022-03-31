using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIScripts.Menus
{
    public class ButtonSelected : MonoBehaviour, ISelectHandler // required interface for OnSelect
    {
        [SerializeField] private GameObject _cursorImages;
        public void OnSelect(BaseEventData eventData)
        {
            _cursorImages.SetActive(true);
        }
    }
}
