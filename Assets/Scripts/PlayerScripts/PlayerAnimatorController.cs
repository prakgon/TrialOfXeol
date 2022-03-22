using System;
using Helpers;
using UnityEngine;
using UnityEngine.Serialization;
using static Helpers.Literals;

namespace PlayerScripts
{
    public class PlayerAnimatorController : MonoBehaviour, IMediatorUser
    {
        [FormerlySerializedAs("_currentPlayerState")] [SerializeField]
        private PlayerStates currentPlayerAnimatorState;

        [FormerlySerializedAs("_lastActiveParameter")] [SerializeField]
        private PlayerParameters lastActiveAnimatorParameter;

        private PlayerMediator _med;
        private Animator _animator;
        public bool HasAnimator { get; private set; }

        public PlayerStates CurrentPlayerAnimatorState
        {
            get => currentPlayerAnimatorState;
            set => currentPlayerAnimatorState = value;
        }

        public PlayerParameters LastActiveAnimatorParameter
        {
            get => lastActiveAnimatorParameter;
            set => lastActiveAnimatorParameter = value;
        }

        private void Start()
        {
            HasAnimator = TryGetComponent(out _animator);
        }

        public bool CompareAnimState(string stateToCompare, byte layer = 0)
        {
            return _animator.GetCurrentAnimatorStateInfo(layer).IsName(stateToCompare);
        }

        public bool IsInTransition(byte layer = 0)
        {
            return _animator.IsInTransition(layer);
        }

        public void ChangeState(PlayerParameters newState, bool state)
        {
            if (state)
            {
                LastActiveAnimatorParameter = newState;
            }

            _animator.SetBool(newState.ToString(), state);
        }

        public void ChangeState(PlayerParameters newState, float state)
        {
            _animator.SetFloat(newState.ToString(), state);
        }

        public void ChangeState(PlayerParameters newState, int parameter)
        {
            _animator.SetInteger(newState.ToString(), parameter);
        }

        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _animator = _med.An;
        }
    }
}