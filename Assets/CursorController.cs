using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public static CursorController instance;

    public Texture2D cursorTexture;
    // public Texture2D loadingTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetDefaultCursor();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void SetDefaultCursor() => Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    
    public void SetVisibility(bool visible) => Cursor.visible = visible;
    
    public void SetLockState(CursorLockMode lockState) => Cursor.lockState = lockState;
    // public void SetLoadingCursor() => Cursor.SetCursor(loadingTexture, hotSpot, cursorMode);
    
}