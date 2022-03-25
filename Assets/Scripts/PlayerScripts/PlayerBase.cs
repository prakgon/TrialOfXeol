using System;
using Photon.Pun;
using static Helpers.Literals;

namespace PlayerScripts
{
    public class PlayerBase : MonoBehaviourPunCallbacks
    {
        protected PlayerAnimatorController _animController;

        public bool isInteracting;
        private bool _isSprinting = true;
        
        public bool isSprinting { get => _isSprinting; set => _isSprinting = value; }

        protected void AnimationStateCheck()
        {
            if (_animController.CompareAnimState(PlayerStates.IdleWalkRunBlend.ToString()))
            {
                _animController.CurrentPlayerAnimatorState = PlayerStates.IdleWalkRunBlend;
            }
            else if (_animController.CompareAnimState(PlayerStates.Roll.ToString()))
            {
                _animController.CurrentPlayerAnimatorState = PlayerStates.Roll;
            }
            else if (_animController.CompareAnimState(PlayerStates.FirstAttack.ToString()))
            {
                _animController.CurrentPlayerAnimatorState = PlayerStates.FirstAttack;
            }
            else if (_animController.CompareAnimState(PlayerStates.SecondAttack.ToString()))
            {
                _animController.CurrentPlayerAnimatorState = PlayerStates.SecondAttack;
            }
            else if (_animController.CompareAnimState(PlayerStates.ThirdAttack.ToString()))
            {
                _animController.CurrentPlayerAnimatorState = PlayerStates.ThirdAttack;
            }
            else if (_animController.CompareAnimState(PlayerStates.FourthAttack.ToString()))
            {
                _animController.CurrentPlayerAnimatorState = PlayerStates.FourthAttack;
            }
            else if (_animController.CompareAnimState(PlayerStates.JumpStart.ToString()))
            {
                _animController.CurrentPlayerAnimatorState = PlayerStates.JumpStart;
            }
            else if (_animController.CompareAnimState(PlayerStates.InAir.ToString()))
            {
                _animController.CurrentPlayerAnimatorState = PlayerStates.InAir;
            }
            else if (_animController.CompareAnimState(PlayerStates.JumpLand.ToString()))
            {
                _animController.CurrentPlayerAnimatorState = PlayerStates.JumpLand;
            }
        }

        protected void HandleInteractions()
        {
            isInteracting = _animController.GetBool(PlayerParameters.isInteracting);
        }
        

    }
}

