using System.Collections;
using Helpers;
using UnityEngine;
using static Helpers.Literals;

namespace PlayerScripts
{
    public class PlayerMechanics : MonoBehaviour, IMediatorUser
    {
        private PlayerMediator _med;
        private PlayerAnimatorController _animController;
        private bool _resettingAttackCount;
        private const float AttackCountResetTime = 4f;

        public int AttackCounter { get; set; }

        public void Roll(bool newState)
        {
            float delta = Time.time;
            Debug.Log(delta);
            if (!newState) return;
            if (_animController.GetBool(PlayerParameters.isInteracting)) return;
            _animController.PlayTargetAnimation(PlayerStatesStrings.Roll, true);
        }

        public void LightAttack(bool newState)
        {
            /*if (!newState) return;
            if (_animController.GetParameterBool(PlayerParameters.isInteracting)) return;
            _animController.SetParameter(PlayerParameters.NormalAttack, true);
            _animController.PlayTargetAnimation("Attack", true);*/
            
            
            
            /*switch (AttackCounter)
            {
                case 0:
                    _animController.PlayTargetAnimation(PlayerStatesStrings.FirstAttack, true);
                    break;
                case 1:
                    _animController.PlayTargetAnimation(PlayerStatesStrings.FirstAttack, true);
                    break;
                case 2:
                    _animController.PlayTargetAnimation(PlayerStatesStrings.FirstAttack, true);
                    break;
                case 3:
                    _animController.PlayTargetAnimation(PlayerStatesStrings.FirstAttack, true);
                    break;
            }*/

            //_animController.SetParameter(PlayerParameters.NormalAttack, true);
            /*if (!_animController.GetParameterBool(PlayerParameters.Attack))
                {
            _animController.SetParameter(PlayerParameters.AttackCount, _attackCount);
                }*/

            /*if (!_resettingAttackCount)
            {
                StartCoroutine(ResetAttackCount(AttackCountResetTime));
            }*/
        }

        public void HeavyAttack(bool newState)
        {
            if (!newState)
            {
                return;
            }

            //_attackCount++;
            _animController.SetParameter(PlayerParameters.Attack, true);
            _animController.SetParameter(PlayerParameters.HeavyAttack, true);
            //_animController.SetParameter(PlayerParameters.AttackCount, _attackCount);
            /*if (!_animController.GetParameterBool(PlayerParameters.Attack))
                {
                }*/

            if (!_resettingAttackCount)
            {
                StartCoroutine(ResetAttackCount(7f));
            }
        }


        private IEnumerator ResetAttackCount(float time)
        {
            _resettingAttackCount = true;
            yield return new WaitForSeconds(time);
            //_attackCount = 0;
            _resettingAttackCount = false;
        }


        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _animController = _med.PlayerAnimatorController;
        }
    }
}