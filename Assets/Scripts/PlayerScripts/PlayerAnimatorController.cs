using Helpers;
using UnityEngine;
using static Helpers.Literals;

namespace PlayerScripts
{
    public class PlayerAnimatorController : MonoBehaviour, IMediatorUser
    {
        [SerializeField] private PlayerStates _currentPlayerState;
        [SerializeField] private PlayerParameters _lastActiveParameter;
        private PlayerMediator _med;
        private Animator _animator;
        public PlayerStates CurrentPlayerState { get => _currentPlayerState; set => _currentPlayerState = value; }
        public PlayerParameters LastActiveParameter { get => _lastActiveParameter; set => _lastActiveParameter = value; }

        public bool CompareAnimState(string stateToCompare, byte layer = 0)
        {
            return _animator.GetCurrentAnimatorStateInfo(layer).IsName(stateToCompare);
        }

        public bool IsTransition(byte layer = 0)
        {
            return _animator.IsInTransition(layer);
        }

        public void ChangeState(PlayerParameters newState, bool state)
        {
            if (state)
            {
                LastActiveParameter = newState;
            }
            _animator.SetBool(newState.ToString(), state);
        }

        public void ChangeState(PlayerParameters newState, float state)
        {
            _animator.SetFloat(newState.ToString(), state);
        }

        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _animator = _med.An;
        }
    }
}
