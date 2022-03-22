using System.Collections;
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
        private bool _resettingAttackCount;

        public void Roll(bool newState)
        {
            _animController.ChangeState(PlayerParameters.Roll, newState);
        }

        public void LightAttack(bool newState)
        {
            switch (_playerMovement.AttackCount)
            {
                case 0:
                    _animController.ChangeState(PlayerParameters.AttackCount, 0);
                    break;
                case 1:
                    _animController.ChangeState(PlayerParameters.AttackCount, 1);
                    break;
                case 2:
                    _animController.ChangeState(PlayerParameters.AttackCount, 2);
                    break;
                case 3:
                    _animController.ChangeState(PlayerParameters.AttackCount, 3);
                    break;
                case 4:
                    _animController.ChangeState(PlayerParameters.AttackCount, 4);
                    break;
                default:
                    _animController.ChangeState(PlayerParameters.AttackCount, 0);
                    break;
            }

            _playerMovement.AttackCount++;
            _animController.ChangeState(PlayerParameters.Attack, newState);

            if (!_resettingAttackCount)
            {
                StartCoroutine(ResetAttackCount());
            }
        }

        private IEnumerator ResetAttackCount()
        {
            _resettingAttackCount = true;
            yield return new WaitForSeconds(4f);
            _playerMovement.AttackCount = 0;
            _resettingAttackCount = false;
        }

        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _animController = _med.PlayerAnimatorController;
            _playerMovement = _med.PlayerMovement;
        }
    }
}