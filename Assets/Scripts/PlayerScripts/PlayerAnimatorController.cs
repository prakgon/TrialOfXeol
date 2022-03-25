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

        public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
        {
            _animator.applyRootMotion = isInteracting;
            _animator.SetBool("isInteracting", isInteracting);
            _animator.CrossFade(targetAnimation, 0.2f);
        }
        public void PlayTargetAnimation(string targetAnimation, bool isInteracting, int layer)
        {
            _animator.applyRootMotion = isInteracting;
            _animator.SetBool("isInteracting", isInteracting);
            _animator.CrossFade(targetAnimation, 0.2f, layer);
        }

        // This method returns the lenght time of the current animation
        public float GetCurrentAnimationTime(byte layer = 0)
        {
            return _animator.GetCurrentAnimatorStateInfo(layer).length;
        }

        public void SetParameter(PlayerParameters parameter, bool value)
        {
            if (value)
            {
                LastActiveAnimatorParameter = parameter;
            }

            _animator.SetBool(parameter.ToString(), value);
        }

        public void SetParameter(PlayerParameters parameter, float value)
        {
            _animator.SetFloat(parameter.ToString(), value);
        }

        public void SetParameter(PlayerParameters parameter, int value)
        {
            _animator.SetInteger(parameter.ToString(), value);
        }

        public bool GetParameterBool(PlayerParameters parameter)
        {
            return _animator.GetBool(parameter.ToString());
        }

        public float GetParameterFloat(PlayerParameters parameter)
        {
            return _animator.GetFloat(parameter.ToString());
        }

        public int GetParameterInteger(PlayerParameters parameter)
        {
            return _animator.GetInteger(parameter.ToString());
        }

        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _animator = _med.An;
        }
    }
}