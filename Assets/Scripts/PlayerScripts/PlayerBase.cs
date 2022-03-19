using System;
using Photon.Pun;
using static Helpers.Literals;

namespace PlayerScripts
{
    public class PlayerBase : MonoBehaviourPunCallbacks
    {
        protected PlayerAnimatorController _animController;
        
        protected bool _canMove = true;
        protected bool _canRotate = true;
        protected bool _canJump = true;
        private bool _canSprint = true;
        
        public bool CanSprint { get => _canSprint; set => _canSprint = value; }

        protected void AnimationStateCheck()
        {
            if (_animController.CompareAnimState(PlayerStates.IdleWalkRunBlend.ToString()))
            {
                //Debug.Log("Idle Walk Run");
                _animController.CurrentPlayerAnimatorState = PlayerStates.IdleWalkRunBlend;
            }

            else if (_animController.CompareAnimState(PlayerStates.Roll.ToString()))
            {
                //Debug.Log("Roll");
                _animController.CurrentPlayerAnimatorState = PlayerStates.Roll;
            }

            else if (_animController.CompareAnimState(PlayerStates.Attack.ToString()))
            {
                //Debug.Log("Attack");
                _animController.CurrentPlayerAnimatorState = PlayerStates.Attack;
            }

            else if (_animController.CompareAnimState(PlayerStates.JumpStart.ToString()))
            {
                _animController.CurrentPlayerAnimatorState = PlayerStates.JumpStart;

                //Debug.Log("JumpStart");
            }

            else if (_animController.CompareAnimState(PlayerStates.InAir.ToString()))
            {
                _animController.CurrentPlayerAnimatorState = PlayerStates.InAir;

                //Debug.Log("InAir");
            }

            else if (_animController.CompareAnimState(PlayerStates.JumpLand.ToString()))
            {
                _animController.CurrentPlayerAnimatorState = PlayerStates.JumpLand;

                //Debug.Log("JumpLand");
            }
        }
        protected void CanJumpCheck()
        {
            if (_animController.CompareAnimState(PlayerStates.IdleWalkRunBlend.ToString()))
            {
                _canJump = true;
            }

            else
            {
                _canJump = false;
            }
        }

        protected void CanRotateCheck()
        {
            if (_animController.CompareAnimState(PlayerStates.Roll.ToString()) || _animController.CompareAnimState(PlayerStates.Attack.ToString()) || _animController.IsInTransition())
            {
                _canRotate = false;
            }

            else
            {
                _canRotate = true;
            }
        }
        protected void CanMoveCheck()
        {
            if (_animController.CompareAnimState(PlayerStates.Attack.ToString()) /*|| _animController.CompareAnimState(PlayerStates.Roll.ToString())*/)
            {
                _canMove = false;
            }

            else
            {
                _canMove = true;
            }
        }
    }
}

