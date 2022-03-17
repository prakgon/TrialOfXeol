using Helpers;
using StarterAssets;
using UnityEngine;
using static Helpers.Literals;

namespace PlayerScripts
{
    public class PlayerMechanics : MonoBehaviour, IMediatorUser
    {
        [SerializeField] private float _rollImpulseFactor;
        private PlayerMediator _med;
        private PlayerAnimatorController _animController;
        private PlayerMovement _playerMovement;
        private const byte FixedRollSpeed = 4;
        public void Roll(bool newRollState)
        {
            _animController.ChangeState(PlayerParameters.Roll, newRollState);
            var rollSpeed = _playerMovement.Speed < FixedRollSpeed ? FixedRollSpeed : _playerMovement.Speed;
            _playerMovement.ControllerMoveForward(rollSpeed * _rollImpulseFactor);
        }

        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _animController = _med.PlayerAnimatorController;
            _playerMovement = _med.PlayerMovement;
        }
    }
}

