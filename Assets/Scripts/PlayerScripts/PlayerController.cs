using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Photon.Pun;
using PlayerScripts;
using TOX;
using static Helpers.Literals;
using UnityEngine;
using Helpers;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour, IMediatorUser
    {
        protected PlayerMediator _med;
        private PlayerAnimatorController _animController;
        private PlayerInputHandler _input;
        private PlayerMovement _playerMovement;
        private PlayerStats _playerStats;

        [Tooltip("Use the PlayerCameraRoot transform")]
        public Transform lockOnTransform;
        
        [Header("Player Flags")]
        public bool isInteracting;
        public bool isSprinting;
        public bool canDoCombo;
        public bool isLocking;
        public bool isInvulnerable;
        public bool isJumping;
        
        
        
        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _animController = med.PlayerAnimatorController;
            _input = med.PlayerInputHandler;
            _playerMovement = med.PlayerMovement;
        }
        
        private void Start()
        {
            _playerStats = GetComponent<PlayerStats>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        private void Update()
        {
            if (_playerMovement.CheckPhotonView()) return;

            _playerMovement.JumpAndGravity();
            _playerMovement.GroundedCheck();
 
            _playerMovement.HandlePlayerLocomotion();
            _playerMovement.HandleRollingAndSprinting();
            
            _playerStats.RegenerateStamina();
        }

        private void LateUpdate()
        {
            _playerMovement.CameraRotation();

            _playerMovement.HandleMoveAnimation();
            
            _playerMovement.HandleMoveEffects();

            _input.rollFlag = false;
            isSprinting = _input.sprintFlag;
            _input.rightTriggerInput = false; // Light attack
            _input.rightTriggerInput = false; // Heavy Attack
            _input.comboFlag = false;
            
            isInteracting = _animController.GetBool(AnimatorParameters.IsInteracting);
            canDoCombo = _animController.GetBool(AnimatorParameters.CanDoCombo);
            isInvulnerable = _animController.GetBool(AnimatorParameters.IsInvulnerable);

            isJumping = _animController.GetBool(AnimatorParameters.Jump);

        }

        //protected virtual bool CheckPhotonView(){ return true; }
        //protected virtual void JumpAndGravity(){}
        //protected virtual void GroundedCheck(){}
        //protected virtual void HandlePlayerLocomotion(){}
        //protected virtual void HandleRollingAndSprinting(){}
        //protected virtual void CameraRotation(){}
    }
}


