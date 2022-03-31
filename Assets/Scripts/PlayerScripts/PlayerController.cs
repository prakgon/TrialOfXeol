using System;
using System.Collections;
using System.Collections.Generic;
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
        private PlayerMovement _playerMovement;
        private PlayerInputHandler _input;
        private PlayerAnimatorController _animController;

        [Header("Player Flags")]
        public bool isInteracting;
        public bool isSprinting;
        public bool canDoCombo;
        public bool isLocking;

        private void Update()
        {
            if (_playerMovement.CheckPhotonView()) return;

            _playerMovement.JumpAndGravity();
            _playerMovement.GroundedCheck();

            _playerMovement.HandlePlayerLocomotion();
            _playerMovement. HandleRollingAndSprinting();
        }

        private void LateUpdate()
        {
            _playerMovement.CameraRotation();

            _input.rollFlag = false;
            isSprinting = _input.sprintFlag;
            _input.rightTriggerInput = false; // Light attack
            _input.rightTriggerInput = false; // Heavy Attack
            isInteracting = _animController.GetBool(AnimatorParameters.IsInteracting);
            _input.comboFlag = false;
            canDoCombo = _animController.GetBool(AnimatorParameters.CanDoCombo);
        }
        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _animController = med.PlayerAnimatorController;
            _input = med.PlayerInputHandler;
            _playerMovement = med.PlayerMovement;
        }

        //protected virtual bool CheckPhotonView(){ return true; }
        //protected virtual void JumpAndGravity(){}
        //protected virtual void GroundedCheck(){}
        //protected virtual void HandlePlayerLocomotion(){}
        //protected virtual void HandleRollingAndSprinting(){}
        //protected virtual void CameraRotation(){}
    }
}


