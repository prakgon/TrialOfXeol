using Photon.Pun;
using static Helpers.Literals;

namespace PlayerScripts
{
    public class PlayerCansXD : MonoBehaviourPunCallbacks
    {
        protected PlayerAnimatorController _animController;
        
        protected bool _canMove = true;
        protected bool _canRotate = true;
        protected bool _canJump = true;

        private bool _canSprint = true;
        public bool CanSprint { get => _canSprint; set => _canSprint = value; }

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
            if (_animController.CompareAnimState(PlayerStates.Roll.ToString()) || _animController.CompareAnimState(PlayerStates.Attack.ToString()) || _animController.IsTransition())
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

