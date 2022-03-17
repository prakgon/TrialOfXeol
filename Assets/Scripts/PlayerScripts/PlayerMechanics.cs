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
        public void Roll(bool newState)
        {
            float rollSpeed;
            if (_med.StarterAssetsInputs.move != Vector2.zero)
            {
                rollSpeed = _playerMovement.Speed < FixedRollSpeed ? FixedRollSpeed : _playerMovement.Speed;
            }

            else
            {
                rollSpeed = 12;
            }
            _animController.ChangeState(PlayerParameters.Roll, newState);
            _playerMovement.ControllerMoveForward(rollSpeed * _rollImpulseFactor);

        }

        public void LightAttack(bool newState)
        {
            _animController.ChangeState(PlayerParameters.Attack, newState);
        }

        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _animController = _med.PlayerAnimatorController;
            _playerMovement = _med.PlayerMovement;
        }
    }
}

