using UnityEngine;
using static Helpers.Literals;

public class ResetAnimatorBool : StateMachineBehaviour
{
    public AnimatorParameters targetBool;
    public bool status;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(targetBool.ToString(), status);
        animator.applyRootMotion = false;
    }

    /*public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(AnimatorParametersStrings.isInteracting, false);
        animator.applyRootMotion = false;
    }*/
}