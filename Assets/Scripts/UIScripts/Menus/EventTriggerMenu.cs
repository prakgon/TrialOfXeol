using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EventTriggerMenu : EventTrigger
{
    public override void OnBeginDrag(PointerEventData data)
    {
        //Debug.Log("OnBeginDrag called.");
    }

    public override void OnCancel(BaseEventData data)
    {
        //Debug.Log("OnCancel called.");
    }

    public override void OnDeselect(BaseEventData data)
    {
        //Debug.Log("OnDeselect called.");
        GetComponent<MenuButton>().cursors.SetActive(false);
    }

    public override void OnDrag(PointerEventData data)
    {
        //Debug.Log("OnDrag called.");
    }

    public override void OnDrop(PointerEventData data)
    {
        //Debug.Log("OnDrop called.");
    }

    public override void OnEndDrag(PointerEventData data)
    {
        //Debug.Log("OnEndDrag called.");
    }

    public override void OnInitializePotentialDrag(PointerEventData data)
    {
        //Debug.Log("OnInitializePotentialDrag called.");
    }

    public override void OnMove(AxisEventData data)
    {
        //Debug.Log("OnMove called.");
        //EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public override void OnPointerClick(PointerEventData data)
    {
        //Debug.Log("OnPointerClick called.");
        AudioMenuManager.Instance.PlayOneShot(gameObject.GetComponent<MenuButton>().clickSound);
    }

    public override void OnPointerDown(PointerEventData data)
    {
        //Debug.Log("OnPointerDown called.");
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        //Debug.Log("OnPointerEnter called.");
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public override void OnPointerExit(PointerEventData data)
    {
        //Debug.Log("OnPointerExit called.");
    }

    public override void OnPointerUp(PointerEventData data)
    {
        //Debug.Log("OnPointerUp called.");
    }

    public override void OnScroll(PointerEventData data)
    {
        //Debug.Log("OnScroll called.");
    }

    public override void OnSelect(BaseEventData data)
    {
        //Debug.Log("OnSelect called.");
        AudioMenuManager.Instance.PlayOneShot(gameObject.GetComponent<MenuButton>().hoverSound);
        GetComponent<MenuButton>().cursors.SetActive(true);
    }

    public override void OnSubmit(BaseEventData data)
    {
        //Debug.Log("OnSubmit called.");
        AudioMenuManager.Instance.PlayOneShot(gameObject.GetComponent<MenuButton>().clickSound);
    }

    public override void OnUpdateSelected(BaseEventData data)
    {
        //Debug.Log("OnUpdateSelected called." + data.selectedObject.name);
    }
}