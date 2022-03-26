using UnityEngine;
using static Helpers.Literals;

namespace TOX
{
    public class RollStateMachine : StateMachineBehaviour
    {
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!(stateInfo.normalizedTime >= 0.85f)) return;
            if (!animator.GetBool(AnimatorParametersStrings.Roll)) return;
            animator.SetBool(AnimatorParametersStrings.Roll, false);
        }
    }
}