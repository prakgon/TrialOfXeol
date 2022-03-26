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
        private AnimatorStates currentAnimatorState;

        [FormerlySerializedAs("_lastActiveParameter")] [SerializeField]
        private AnimatorParameters lastActiveAnimatorState;

        private PlayerMediator _med;
        private Animator _animator;
        public bool HasAnimator { get; private set; }

        public AnimatorStates CurrentAnimatorState
        {
            get => currentAnimatorState;
            set => currentAnimatorState = value;
        }

        public AnimatorParameters LastActiveAnimatorState
        {
            get => lastActiveAnimatorState;
            set => lastActiveAnimatorState = value;
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
        
        public void PlayTargetAnimation(string targetAnimation, bool isInteracting, int layer = 0)
        {
            _animator.applyRootMotion = isInteracting;
            _animator.SetBool(AnimatorParametersStrings.isInteracting, isInteracting);
            _animator.CrossFade(targetAnimation, 0.2f, layer);
        }

        public void PlayTargetAnimation(AnimatorStates targetAnimation, bool isInteracting, int layer = 0)
        {
            _animator.applyRootMotion = isInteracting;
            _animator.SetBool( AnimatorParametersStrings.isInteracting, isInteracting);
            _animator.CrossFade(targetAnimation.ToString(), 0.2f, layer);
        }


        // This method returns the lenght time of the current animation
        public float GetCurrentAnimationTime(byte layer = 0)
        {
            return _animator.GetCurrentAnimatorStateInfo(layer).length;
        }

        public void SetParameter(AnimatorParameters state, bool value)
        {
            if (value)
            {
                LastActiveAnimatorState = state;
            }

            _animator.SetBool(state.ToString(), value);
        }

        public void SetParameter(AnimatorParameters state, float value)
        {
            _animator.SetFloat(state.ToString(), value);
        }

        public void SetParameter(AnimatorParameters state, int value)
        {
            _animator.SetInteger(state.ToString(), value);
        }

        public bool GetBool(AnimatorParameters state)
        {
            return _animator.GetBool(state.ToString());
        }

        public float GetFloat(AnimatorParameters state)
        {
            return _animator.GetFloat(state.ToString());
        }

        public int GetInteger(AnimatorParameters state)
        {
            return _animator.GetInteger(state.ToString());
        }

        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _animator = _med.An;
        }
    }
}