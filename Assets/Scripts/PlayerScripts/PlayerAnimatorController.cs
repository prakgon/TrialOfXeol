using UnityEngine;
using static Helpers.Literals;

namespace PlayerScripts
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        private Animator _an;
        [SerializeField] private PlayerStates _currentPlayerState;
        [SerializeField] private PlayerParameters _lastActiveParameter;

        public PlayerStates CurrentPlayerState { get => _currentPlayerState; set => _currentPlayerState = value; }
        public Animator An { get => _an; set => _an = value; }
        public PlayerParameters LastActiveParameter { get => _lastActiveParameter; set => _lastActiveParameter = value; }

        private void Start()
        {
            An = GetComponent<Animator>();
        }
        public bool CompareAnimState(string stateToCompare, byte layer = 0)
        {
            if (An.GetCurrentAnimatorStateInfo(layer).IsName(stateToCompare)) return true;
            return false;
        }

        public bool IsTransition(byte layer = 0)
        {
            return An.IsInTransition(layer);
        }

        public void ChangeState(PlayerParameters newState, bool state)
        {
            if (state)
            {
                LastActiveParameter = newState;
            }

            An.SetBool(newState.ToString(), state);
        }

        public void ChangeState(PlayerParameters newState, float state)
        {
            An.SetFloat(newState.ToString(), state);
        }
    }
}
