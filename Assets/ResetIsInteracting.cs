using UnityEngine;
using static Helpers.Literals;

public class ResetIsInteracting : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(AnimatorParametersStrings.isInteracting, false);
        animator.applyRootMotion = false;
    }
}