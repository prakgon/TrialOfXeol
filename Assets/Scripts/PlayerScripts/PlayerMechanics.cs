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
        private const float AttackCountResetTime = 4f;

        public void Roll(bool newState)
        {
            if (newState)
            {
                if (!_animController.GetParameterBool(PlayerParameters.Roll))
                {
                    _animController.SetParameter(PlayerParameters.Roll, true);
                }
            }
        }

        public void LightAttack(bool newState)
        {
            if (newState)
            {
                if (!_animController.GetParameterBool(PlayerParameters.Attack))
                {
                    _attackCount++;
                    _animController.SetParameter(PlayerParameters.Attack, true);
                    _animController.SetParameter(PlayerParameters.NormalAttack, true);
                }
                if (!_resettingAttackCount)
                {
                    StartCoroutine(ResetAttackCount());
                }
            }
            _animController.SetParameter(PlayerParameters.AttackCount, _attackCount);
        }

        public void HeavyAttack(bool newState)
        {
            if (newState)
            {
                if (!_animController.GetParameterBool(PlayerParameters.Attack))
                {
                    _attackCount++;
                    _animController.SetParameter(PlayerParameters.Attack, true);
                    _animController.SetParameter(PlayerParameters.HeavyAttack, true);
                }

                if (!_resettingAttackCount)
                {
                    StartCoroutine(ResetAttackCount());
                }
            }

            _animController.SetParameter(PlayerParameters.AttackCount, _attackCount);
        }
        
        // This method is used by the animation events to disable animator parameters
        private void DisableState(PlayerParameters parameter) => _animController.SetParameter(parameter, false);
        
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