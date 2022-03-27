
using Helpers;
using UnityEngine;

namespace AnimatorStateMachine
{

    public class IntroAnimatorState : ResetAnimatorBoolOnEnter
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            animator.SetBool(Literals.AnimatorParameters.IsInteracting.ToString(), true);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(Literals.AnimatorParameters.IsInteracting.ToString(), false);
        }
    }
}