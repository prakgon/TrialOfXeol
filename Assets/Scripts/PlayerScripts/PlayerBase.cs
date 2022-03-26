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
            if (_animController.CompareAnimState(AnimatorStates.IdleWalkRunBlend.ToString()))
            {
                _animController.CurrentAnimatorState = AnimatorStates.IdleWalkRunBlend;
            }
            else if (_animController.CompareAnimState(AnimatorStates.Roll.ToString()))
            {
                _animController.CurrentAnimatorState = AnimatorStates.Roll;
            }
            else if (_animController.CompareAnimState(AnimatorStates.FirstAttack.ToString()))
            {
                _animController.CurrentAnimatorState = AnimatorStates.FirstAttack;
            }
            else if (_animController.CompareAnimState(AnimatorStates.SecondAttack.ToString()))
            {
                _animController.CurrentAnimatorState = AnimatorStates.SecondAttack;
            }
            else if (_animController.CompareAnimState(AnimatorStates.ThirdAttack.ToString()))
            {
                _animController.CurrentAnimatorState = AnimatorStates.ThirdAttack;
            }
            else if (_animController.CompareAnimState(AnimatorStates.FourthAttack.ToString()))
            {
                _animController.CurrentAnimatorState = AnimatorStates.FourthAttack;
            }
            else if (_animController.CompareAnimState(AnimatorStates.JumpStart.ToString()))
            {
                _animController.CurrentAnimatorState = AnimatorStates.JumpStart;
            }
            else if (_animController.CompareAnimState(AnimatorStates.InAir.ToString()))
            {
                _animController.CurrentAnimatorState = AnimatorStates.InAir;
            }
            else if (_animController.CompareAnimState(AnimatorStates.JumpLand.ToString()))
            {
                _animController.CurrentAnimatorState = AnimatorStates.JumpLand;
            }
        }

        protected void HandleInteractions()
        {
            isInteracting = _animController.GetBool(AnimatorParameters.isInteracting);
        }
        

    }
}

