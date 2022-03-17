using Helpers;
using StarterAssets;
using UnityEngine;
using static Helpers.Literals;

namespace PlayerScripts
{
    public class PlayerMechanics : MonoBehaviour, IMediatorUser
    {
        private PlayerMediator _med;
        private PlayerAnimatorController _animController;
        private PlayerMovement _playerMovement;
        public void Roll(bool newRollState)
        {
            _animController.ChangeState(PlayerParameters.Roll, newRollState);
            const int fixedRollSpeed = 4;
            var rollSpeed = _playerMovement.Speed < fixedRollSpeed ? fixedRollSpeed : _playerMovement.Speed;
            _playerMovement.ControllerMoveForward(rollSpeed);
        }

        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _animController = _med.PlayerAnimatorController;
            _playerMovement = _med.PlayerMovement;
        }
    }
}

