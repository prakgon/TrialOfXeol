using System;
using Helpers;
using PlayerScripts;
using TOX;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    // Basic inputs
    [Header("Basic player inputs")] public Vector2 move;
    public float moveAmount;
    public Vector2 look;
    public bool jump;
    public bool targetLock = false;
    // Combat mechanics inputs
    [Header("Combat input flags")] public bool rollFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public float rollInputTimer;

    // Controller convection keys Input Handlers
    [Header("Controller convection inputs")] [Tooltip("This button handles roll and sprint inputs")]
    public bool eastButtonInput;

    [Tooltip("This button handles right handed light attack inputs")]
    public bool rightButtonInput;

    [Tooltip("This button handles right handed heavy attack inputs")]
    public bool rightTriggerInput;

    [Tooltip("This button handles special attack inputs")]
    public bool leftTriggerInput;

    private PlayerCombatManager _playerCombatManager;
    private PlayerInventory _playerInventory;
    private PlayerController _playerController;
    private PlayerMovement _playerMovement;

    private void Start()
    {
        _playerCombatManager = GetComponent<PlayerCombatManager>();
        _playerInventory = GetComponent<PlayerInventory>();
        _playerController = GetComponent<PlayerController>();
        _playerMovement = GetComponent<PlayerMovement>();

    }

    #region Input Events

    public void OnMove(InputAction.CallbackContext context) => MoveHandler(context);
    public void OnLook(InputAction.CallbackContext context) => look = context.ReadValue<Vector2>();
    public void OnRollAndSprint(InputAction.CallbackContext context) => RollAndSprintHandler(context);
    public void OnJump(InputAction.CallbackContext context) => JumpHandler(context);
    public void OnLightAttack(InputAction.CallbackContext context) => LightAttackHandler(context);
    public void OnHeavyAttack(InputAction.CallbackContext context) => HeavyAttackHandler(context);
    public void OnSpecialAttack(InputAction.CallbackContext context) => SpecialAttackHandler(context);
    public void OnTargetLock(InputAction.CallbackContext context) => _playerMovement.ToggleTargetLock();
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

    #endregion
}