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
        private int _attackCount;
        private bool _resettingAttackCount;
        private const float AttackCountResetTime = 5f;

        public void Roll(bool newState)
        {
            _animController.ChangeState(PlayerParameters.Roll, newState);
        }

        public void LightAttack(bool newState)
        {
            Debug.Log(_animController.CurrentPlayerAnimatorState);
            _animController.ChangeState(PlayerParameters.Attack, newState);
            if (newState)
            {
                _animController.ChangeState(PlayerParameters.AttackCount, _attackCount);
                _attackCount++;
            }
            else
            {
                if (!_resettingAttackCount)
                {
                    StartCoroutine(ResetAttackCount());
                }
            }

           
        }

        private IEnumerator ResetAttackCount()
        {
            _resettingAttackCount = true;
            yield return new WaitForSeconds(AttackCountResetTime);
            _attackCount = 0;
            _resettingAttackCount = false;
        }

        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _animController = _med.PlayerAnimatorController;
        }
    }
}