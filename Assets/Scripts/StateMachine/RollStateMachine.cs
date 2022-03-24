using UnityEngine;
using static Helpers.Literals;

namespace TOX
{
    public class RollStateMachine : StateMachineBehaviour
    {
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!(stateInfo.normalizedTime >= 0.85f)) return;
            if (!animator.GetBool(PlayerParametersStrings.Roll)) return;
            animator.SetBool(PlayerParametersStrings.Roll, false);
        }
    }
}