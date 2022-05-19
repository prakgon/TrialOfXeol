using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PointerController : MonoBehaviour {
    
    /*DefaultInputActions _defaultInputActions;
    
    Vector2 _currentPointerPosition;
    
    Vector2 _lastPointerPosition;
    
    
    
    void Start(){
        _defaultInputActions = new DefaultInputActions();
        _defaultInputActions.UI.Point.performed += action => _currentPointerPosition = action.ReadValue<Vector2>();
        
        _defaultInputActions.Enable();
    }
 
    void Update () {
     
        Vector2 pointerDelta = _currentPointerPosition - _lastPointerPosition;
 
 
        //If the magnitude of the pointerDelta is larger than zero, the pointer must have been moved
        if (pointerDelta.magnitude > 0f) { 
            Debug.Log(_defaultInputActions.controlSchemes.Count);
            Debug.Log(_defaultInputActions.devices);
            Debug.Log(_defaultInputActions.bindingMask);
            Debug.Log(_defaultInputActions.bindings);
            CursorController.instance.SetVisibility(true);
        } else {
            Debug.Log(pointerDelta.magnitude > 0f);
            CursorController.instance.SetVisibility(false);
        }
        
        _lastPointerPosition = _currentPointerPosition;
        
    }*/
        
    [SerializeField] private InputAction pointerPosition;
    [SerializeField] private InputAction gamepadInput;
    
    [SerializeField] Vector2 _currentPointerPosition;
    [SerializeField] Vector2 _currentGamepadPosition;
    
    //Vector2 _lastPointerPosition;

    private void Awake()
    {
    }

    void Start(){
        pointerPosition.AddBinding("<Mouse>/delta");
        gamepadInput.AddBinding("<Gamepad>/leftStick");
        gamepadInput.AddBinding("<Gamepad>/rightStick");
        gamepadInput.AddBinding("<Gamepad>/dpad");
        
        // pointerPosition.performed += action => _currentPointerPosition = action.ReadValue<Vector2>();
        // gamepadInput.performed += action => _currentGamepadPosition = action.ReadValue<Vector2>();
        pointerPosition.performed += PointerPerfomed;
        gamepadInput.performed += GamepadPerfomed;
        
        pointerPosition.Enable();
        gamepadInput.Enable();
    }
    
    void PointerPerfomed(InputAction.CallbackContext context){
        Debug.Log("PointerPerfomed");
        Debug.Log(context.ReadValue<Vector2>());
        Cursor.visible = true;
    }
    
    void GamepadPerfomed(InputAction.CallbackContext context){
        Debug.Log("GamepadPerfomed");
        Debug.Log(context.ReadValue<Vector2>());
        Cursor.visible = false;
    }
 
}
