using Audio;
using Helpers;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using static Helpers.Literals;
using static Helpers.LiteralToStringParse;

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
        PhotonView _photonView;
        public bool HasAnimator { get; private set; }

        public AnimatorStates CurrentAnimatorState => currentAnimatorState;

        public AnimatorParameters LastActiveAnimatorState
        {
            set => lastActiveAnimatorState = value;
        }

        private void Start()
        {
            HasAnimator = TryGetComponent(out _animator);
            _photonView = PhotonView.Get(gameObject);
        }

        public bool CompareAnimState(string stateToCompare, byte layer = 0)
        {
            return _animator.GetCurrentAnimatorStateInfo(layer).IsName(stateToCompare);
        }

        public bool IsInTransition(byte layer = 0)
        {
            return _animator.IsInTransition(layer);
        }

        [PunRPC]
        public void PlayTargetAnimation(string targetAnimation, bool isInteracting, int indexLayer,
            bool isRemote = false)
        {
            if (!isRemote)
            {
                _photonView.RPC(LiteralToStringParse.PlayTargetAnimation, RpcTarget.Others, targetAnimation,
                    isInteracting, indexLayer, true);
            }

            _animator.applyRootMotion = isInteracting;
            SetParameter(AnimatorParameters.IsInteracting, isInteracting);
            _animator.CrossFade(targetAnimation, 0.2f, indexLayer);
        }

        [PunRPC]
        public void PlayTargetAnimation(AnimatorStates targetAnimation, bool isInteracting, int indexLayer,
            bool isRemote = false)
        {
            if (!isRemote)
            {
                _photonView.RPC(LiteralToStringParse.PlayTargetAnimation, RpcTarget.Others, targetAnimation,
                    isInteracting, indexLayer, true);
            }

            _animator.applyRootMotion = isInteracting;
            SetParameter(AnimatorParameters.IsInteracting, isInteracting);
            _animator.CrossFade(targetAnimation.ToString(), 0.2f, indexLayer);
        }

        public void EnableCombo(AttackAnimations targetAnimation, PlayerCombatManager playerCombatManager,
            AttackAnimations comboPhase, bool isRemote = false)
        {
            //if (!isRemote)
            //{
            //    _photonView.RPC(LiteralToStringParse.EnableCombo, RpcTarget.Others, true);
            //}

            SetParameter(AnimatorParameters.CanDoCombo, true);

            PlayTargetAnimation(targetAnimation.ToString(), true, 1);
            playerCombatManager.LastAttack = comboPhase;
            
            AudioManager.Instance.AtPoint(Literals.AudioType.AirSlash, transform.position);
        }

        public void DisableCombo(AttackAnimations targetAnimation, PlayerCombatManager playerCombatManager,
            AttackAnimations comboPhase)
        {
            SetParameter(AnimatorParameters.CanDoCombo, false);

            PlayTargetAnimation(targetAnimation.ToString(), true, 1);
            playerCombatManager.LastAttack = comboPhase;
        }

        public void EnableIsInvulnerable() => SetParameter(AnimatorParameters.IsInvulnerable, true);
        public void DisableIsInvulnerable() => SetParameter(AnimatorParameters.IsInvulnerable, false);


        public void SetParameter(AnimatorParameters state, bool value)
        {
            if (value)
            {
                LastActiveAnimatorState = state;
            }

            _animator.SetBool(state.ToString(), value);
        }

        public void SetParameter(AnimatorParameters state, float value) => _animator.SetFloat(state.ToString(), value);
        public void SetParameter(AnimatorParameters state, int value) => _animator.SetInteger(state.ToString(), value);

        public bool GetBool(AnimatorParameters state) => _animator.GetBool(state.ToString());
        public float GetFloat(AnimatorParameters state) => _animator.GetFloat(state.ToString());
        public float GetInteger(AnimatorParameters state) => _animator.GetInteger(state.ToString());

        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _animator = _med.An;
        }
    }
}