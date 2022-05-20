using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PointerController : MonoBehaviour
{
    public static PointerController Instance;
    
    public InputAction pointerPosition;
    [SerializeField] private InputAction gamepadInput;

    // [SerializeField] Vector2 _currentPointerPosition;
    // [SerializeField] Vector2 _currentGamepadPosition;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        pointerPosition.AddBinding("<Mouse>/delta");
        gamepadInput.AddBinding("<Gamepad>/leftStick");
        gamepadInput.AddBinding("<Gamepad>/dpad");

        // pointerPosition.performed += action => _currentPointerPosition = action.ReadValue<Vector2>();
        // gamepadInput.performed += action => _currentGamepadPosition = action.ReadValue<Vector2>();
        // pointerPosition.performed += PointerPerformed;
        pointerPosition.performed += _ => CursorController.Instance.SetVisibility(false);
        gamepadInput.performed += GamepadPerformed;

        pointerPosition.Enable();
        gamepadInput.Enable();
    }

    public void PointerPerformed(InputAction.CallbackContext context)
    {
        // Debug.Log(context.ReadValue<Vector2>());
        Cursor.visible = true;
    }

    private void GamepadPerformed(InputAction.CallbackContext context)
    {
        // Debug.Log(context.ReadValue<Vector2>());
        Cursor.visible = false;
    }

    public void DisableAll()
    {
        pointerPosition.Disable();
        gamepadInput.Disable();
    }
    
    public void DisablePointer()
    {
        pointerPosition.Disable();
    }
}