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

        public void Roll(bool newState)
        {
            _animController.ChangeState(PlayerParameters.Roll, newState);
        }

        public void LightAttack(bool newState)
        {
            _animController.ChangeState(PlayerParameters.Attack, newState);
            if (_animController.CurrentPlayerAnimatorState != PlayerStates.Attack)
            {
                _playerMovement.AttackCount += 1;
            }
            _animController.ChangeState(PlayerParameters.AttackCount, _playerMovement.AttackCount);

        }

        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _animController = _med.PlayerAnimatorController;
            _playerMovement = _med.PlayerMovement;
        }
    }
}

