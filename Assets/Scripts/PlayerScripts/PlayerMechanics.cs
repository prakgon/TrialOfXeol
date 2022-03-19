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

        public void Roll(bool newState)
        {
            _animController.ChangeState(PlayerParameters.Roll, newState);
        }

        public void LightAttack(bool newState)
        {
            _animController.ChangeState(PlayerParameters.Attack, newState);
        }

        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _animController = _med.PlayerAnimatorController;
        }
    }
}

