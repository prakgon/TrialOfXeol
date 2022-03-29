using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PlayerScripts;
using TOX;
using static Helpers.Literals;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputHandler _input;
    private PlayerAnimatorController _animatorController;
    private PlayerMovement _playerMovement;
    
    [Header("Player Flags")]
    public bool isInteracting;
    public bool isSprinting;
    public bool canDoCombo;
    
    // Start is called before the first frame update
    private void Start()
    {
        _input = GetComponent<PlayerInputHandler>();
        _animatorController = GetComponent<PlayerAnimatorController>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_playerMovement.CheckPhotonView()) return;
        
        float delta = Time.deltaTime;
        
        //_playerMovement.AnimationStateCheck();
        
        _playerMovement.JumpAndGravity();
        _playerMovement.GroundedCheck();

        _playerMovement.HandlePlayerLocomotion();
        _playerMovement.HandleRollingAndSprinting();
        
    }

    private void LateUpdate()
    {
        _playerMovement.CameraRotation();

        _input.rollFlag = false;
        isSprinting = _input.sprintFlag;
        _input.rightTriggerInput = false; // Light attack
        _input.rightTriggerInput = false; // Heavy Attack
        isInteracting = _animatorController.GetBool(AnimatorParameters.IsInteracting);
        _input.comboFlag = false;
        canDoCombo = _animatorController.GetBool(AnimatorParameters.CanDoCombo);
    }
}
