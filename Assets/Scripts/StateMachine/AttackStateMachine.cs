using PlayerScripts;
using UnityEngine;
using static Helpers.Literals;

namespace TOX
{
    public class AttackStateMachine : StateMachineBehaviour
    {
        [SerializeField] private GameObject player;
        private PlayerMechanics _playerMechanics;

        private const int MaxAttackCounter = 4;

        private void Awake()
        {
            _playerMechanics = player.GetComponent<PlayerMechanics>();
        }

        // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log(stateInfo.normalizedTime);
            if (!(stateInfo.normalizedTime >= 0.7f)) return;
            if (!animator.GetBool(PlayerParametersStrings.Attack)) return;
            _playerMechanics.AttackCounter++;
            if (_playerMechanics.AttackCounter >= MaxAttackCounter)
            {
                _playerMechanics.AttackCounter = 0;
            }

            animator.SetInteger(PlayerParametersStrings.AttackCount, _playerMechanics.AttackCounter);
            animator.SetBool(PlayerParametersStrings.Attack, false);
        }

        // OnStateExit is called before OnStateExit is called on any state inside this state machine
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        // OnStateMove is called before OnStateMove is called on any state inside this state machine
        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        // OnStateIK is called before OnStateIK is called on any state inside this state machine
        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        //OnStateMachineEnter is called when entering a state machine via its Entry Node
        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
        }

        // OnStateMachineExit is called when exiting a state machine via its Exit Node
        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
        }
    }
}