using System;
using Helpers;
using PlayerScripts;
using TOX;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour, IMediatorUser
{
    // Basic inputs
    [Header("Basic player inputs")] 
    public Vector2 move;
    public float moveAmount;
    public Vector2 look;
    public bool jump;


    // Combat mechanics inputs
    [Header("Combat input flags")] 
    public bool rollFlag;
    public bool sprintFlag;
    public bool lockOnFlag = false;
    public bool comboFlag;
    public float rollInputTimer;

    // Controller convection keys Input Handlers
    [Header("Controller convection inputs")] 
    [Tooltip("This button handles roll and sprint inputs")]
    public bool eastButtonInput;

    [Tooltip("This button handles right handed light attack inputs")]
    public bool rightButtonInput;

    [Tooltip("This button handles right handed heavy attack inputs")]
    public bool rightTriggerInput;

    [Tooltip("This button handles special attack inputs")]
    public bool leftTriggerInput;
    
    [Tooltip("This input handles camera lock on target")]
    public bool lockOnInput;

    [Tooltip("This input changes camera lock on to right target")]
    public bool lockOnRightInput;
    
    [Tooltip("This input changes camera lock on to left target")]
    public bool lockOnLeftInput;

    [Tooltip("This button handles menu inputs")]
    public bool startInput;

    [SerializeField] private int _startCounter;

    private PlayerCombatManager _playerCombatManager;
    private PlayerInventory _playerInventory;
    private PlayerController _playerController;
    private PlayerMovement _playerMovement;
    
    
    // Create UI Controller
    [Header("Player Controls UI")]
    [SerializeField] private GameObject _gamepadControlsSchema;
    [SerializeField] private GameObject _gamepadControlsText;
    [SerializeField] private GameObject _keyboardControlsText;
    

    #region Input Events

    public void OnMove(InputAction.CallbackContext context) => MoveHandler(context);
    public void OnLook(InputAction.CallbackContext context) => look = context.ReadValue<Vector2>();
    public void OnRollAndSprint(InputAction.CallbackContext context) => RollAndSprintHandler(context);
    public void OnJump(InputAction.CallbackContext context) => JumpHandler(context);
    public void OnLightAttack(InputAction.CallbackContext context) => LightAttackHandler(context);
    public void OnHeavyAttack(InputAction.CallbackContext context) => HeavyAttackHandler(context);
    public void OnSpecialAttack(InputAction.CallbackContext context) => SpecialAttackHandler(context);
    public void OnTargetLock(InputAction.CallbackContext context) => LockTarget(context);
    public void OnBlock(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase == InputActionPhase.Started);
    }
    public void OnShowControls(InputAction.CallbackContext context) => ShowControlsHandler(context);
    public void OnChangeWeapon(InputAction.CallbackContext context) => ChangeWeaponHandler(context);
    public void OnLockOnTargetLeft(InputAction.CallbackContext context) => LockOnTargetLeftHandler(context);
    public void OnLockOnTargetRight(InputAction.CallbackContext context) => LockOnTargetRightHandler(context);

    #endregion


    #region Event Handlers

    private void MoveHandler(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        moveAmount = Mathf.Clamp01(Mathf.Abs(move.x) + Mathf.Abs(move.y));
    }

    private void JumpHandler(InputAction.CallbackContext context)
    {
        jump = context.phase switch
        {
            InputActionPhase.Disabled => false,
            InputActionPhase.Waiting => false,
            InputActionPhase.Started => true,
            InputActionPhase.Performed => true,
            InputActionPhase.Canceled => false,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void RollAndSprintHandler(InputAction.CallbackContext context)
    {
        eastButtonInput = context.phase == InputActionPhase.Performed;

        switch (context.phase)
        {
            case InputActionPhase.Disabled:
                rollFlag = false;
                sprintFlag = false;
                break;
            case InputActionPhase.Waiting:
                rollFlag = false;
                sprintFlag = false;
                break;
            case InputActionPhase.Started:
                if (moveAmount > 0)
                {
                    sprintFlag = true;
                    rollInputTimer = Time.time;
                }
                else
                {
                    rollFlag = true;
                }

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
        rightButtonInput = context.phase is InputActionPhase.Started;

        if (!rightButtonInput || _playerController.isInteracting) return;
        comboFlag = true;
        _playerCombatManager.HandleLightWeaponCombo(_playerInventory.rightWeapon);
        comboFlag = false;
        /*
        if (_playerController.canDoCombo)
        */
        /*{
        }
        else
        {
            _playerAttacker.HandleLightAttack(_playerInventory.rightWeapon);
        }*/
    }

    private void HeavyAttackHandler(InputAction.CallbackContext context)
    {
        rightTriggerInput = context.phase is InputActionPhase.Started;

        if (!rightTriggerInput || _playerController.isInteracting) return;
        comboFlag = true;
        _playerCombatManager.HandleHeavyWeaponCombo(_playerInventory.rightWeapon);
        comboFlag = false;
        /*if (_playerController.canDoCombo)
        {
        }
        else
        {
            _playerAttacker.HandleHeavyAttack(_playerInventory.rightWeapon);
        }*/
    }

    private void SpecialAttackHandler(InputAction.CallbackContext context)
    {
        leftTriggerInput = context.phase is InputActionPhase.Started;

        if (!leftTriggerInput || _playerController.isInteracting) return;
        comboFlag = true;
        _playerCombatManager.HandleSkillWeaponCombo(_playerInventory.rightWeapon);
        comboFlag = false;
        /*if (_playerController.canDoCombo)
        {
        }
        else
        {
            _playerAttacker.HandleSkillAttack(_playerInventory.rightWeapon);
        }*/
    }

    private void LockTarget(InputAction.CallbackContext context)
    {
        lockOnInput = context.phase == InputActionPhase.Performed;

        if (lockOnInput && lockOnFlag == false)
        {
            lockOnInput = false;
            _playerMovement.HandleLockOn();
            if (_playerMovement.nearestLockOnTarget != null)
            {
                _playerMovement.currentLockOnTarget = _playerMovement.nearestLockOnTarget;
                lockOnFlag = true;
            }
        }
        else if (lockOnInput && lockOnFlag)
        {
            lockOnFlag = false;
            lockOnInput = false;
                
            _playerMovement.ClearLockOnTargets();
        }
        
        _playerMovement.StrafeTransition(lockOnFlag);
        
        _playerController.isLocking = lockOnFlag;
    } 

    private void ChangeWeaponHandler(InputAction.CallbackContext context)
    {
        if (context.phase is InputActionPhase.Started)
        {
            _playerInventory.ChangeWeapon((int)Math.Round(context.ReadValue<float>()));
        }
    }

    private void LockOnTargetLeftHandler(InputAction.CallbackContext context)
    {
        lockOnLeftInput = context.phase == InputActionPhase.Performed;

        if (lockOnFlag && lockOnLeftInput)
        {
            lockOnRightInput = false;
            _playerMovement.HandleLockOn();
            if (_playerMovement.leftLockTarget != null)
            {
                _playerMovement.currentLockOnTarget = _playerMovement.leftLockTarget;
            }
        }
    } 
    
    private void LockOnTargetRightHandler(InputAction.CallbackContext context)
    {
        lockOnRightInput = context.phase == InputActionPhase.Performed;
        
        if (lockOnFlag && lockOnRightInput)
        {
            lockOnLeftInput = false;
            _playerMovement.HandleLockOn();
            if (_playerMovement.rightLockTarget != null)
            {
                _playerMovement.currentLockOnTarget = _playerMovement.rightLockTarget;
            }
        }
    }
    

    private void ShowControlsHandler(InputAction.CallbackContext context)
    {
        startInput = context.phase == InputActionPhase.Performed;
        
        _startCounter += startInput ? 1 : 0;
        _startCounter = _startCounter >= 3 ? 0 : _startCounter;
        
        if (!startInput) return;
        
        
        
        switch (_startCounter)
        {
            case 0:
                _gamepadControlsSchema.SetActive(false);
                _gamepadControlsText.SetActive(false);
                _keyboardControlsText.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case 1:
                _gamepadControlsSchema.SetActive(true);
                _gamepadControlsText.SetActive(false);
                _keyboardControlsText.SetActive(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
            case 2:
                _gamepadControlsSchema.SetActive(false);
                _gamepadControlsText.SetActive(true);
                _keyboardControlsText.SetActive(true);
                break;
        }
    }

    #endregion

    public void ConfigureMediator(PlayerMediator med)
    {
        _playerCombatManager = med.PlayerCombatManager;
        _playerInventory = med.PlayerInventory;
        _playerController = med.PlayerController;
        _playerMovement = med.PlayerMovement;
    }
}