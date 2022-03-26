using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
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

    //private CameraHandler _cameraHandler;

    // Input Handlers
    public bool b_Input;
    
    
    #region Input Events
    
    public void OnMove(InputAction.CallbackContext context) => MoveHandler(context);
    public void OnLook(InputAction.CallbackContext context) => look = context.ReadValue<Vector2>();
    public void OnRollAndSprint(InputAction.CallbackContext context) => RollAndSprintHandler(context);
    public void OnJump(InputAction.CallbackContext context) => JumpHandler(context);
    public void OnLightAttack(InputAction.CallbackContext context) => LightAttackHandler(context);
    
    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase == InputActionPhase.Started);
    }
    public void OnBlock(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase == InputActionPhase.Started);
    }
    
    #endregion


    #region Event Handlers
    private void MoveHandler(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        moveAmount = Mathf.Clamp01(Mathf.Abs(move.x) + Mathf.Abs(move.y));
    }

    private void RollAndSprintHandler(InputAction.CallbackContext context)
    {
        b_Input = context.phase == InputActionPhase.Performed;
        
        switch (context.phase)
        {
            case InputActionPhase.Disabled:
                break;
            case InputActionPhase.Waiting:
                break;
            case InputActionPhase.Started:
                sprintFlag = true;
                rollInputTimer += Time.time;
                break;
            case InputActionPhase.Performed:
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
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void LightAttackHandler(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Disabled:
                break;
            case InputActionPhase.Waiting:
                break;
            case InputActionPhase.Started:
                break;
            case InputActionPhase.Performed:
                break;
            case InputActionPhase.Canceled:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    

    private void JumpHandler(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Disabled:
                break;
            case InputActionPhase.Waiting:
                break;
            case InputActionPhase.Started:
                jump = true;
                break;
            case InputActionPhase.Performed:
                jump = true;
                break;
            case InputActionPhase.Canceled:
                jump = false;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    #endregion

}