using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    // Basic inputs
    public Vector2 move;
    public float moveAmount;
    public Vector2 look;

    // Combat mechanics inputs
    public bool jump;
    public bool rollFlag;
    public bool sprintFlag;
    public float rollInputTimer;
    public bool isInteracting;

    private InputActionSystem _inputActions;
    //private CameraHandler _cameraHandler;

    // Input Handlers
    private Vector2 _movementInput;
    private Vector2 _cameraInput;

    public bool b_Input;

    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
        moveAmount = Mathf.Clamp01(Mathf.Abs(_movementInput.x) + Mathf.Abs(_movementInput.y));
        move = _movementInput;
    }

    public void OnLook(InputAction.CallbackContext context)
    {

        _cameraInput = context.ReadValue<Vector2>();
        look = _cameraInput;
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        b_Input = context.phase == InputActionPhase.Started;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                sprintFlag = true;
                rollInputTimer += Time.time;
                break;
            case InputActionPhase.Canceled:
                rollInputTimer = Time.time - rollInputTimer;
                if (rollInputTimer is > 0 and < 0.5f)
                {
                    rollFlag = true;
                }
                sprintFlag = false;
                rollInputTimer = 0;
                break;
            
            case InputActionPhase.Disabled:
                break;
            case InputActionPhase.Waiting:
                break;
            case InputActionPhase.Performed:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jump = context.phase == InputActionPhase.Started;
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase == InputActionPhase.Started);
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase == InputActionPhase.Started);
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase == InputActionPhase.Started);
    }
}
