using Helpers;
using UnityEngine;

namespace AnimatorStateMachine
{
    public class ResetAnimatorBoolOnUpdate : StateMachineBehaviour
    {
        public Literals.AnimatorParameters targetBool;
        public bool status;
        
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Debug.Log(stateInfo.normalizedTime);
            if (!animator.GetBool(Literals.AnimatorParameters.CanDoCombo.ToString())) return;
            if (stateInfo.normalizedTime > 2f) animator.SetBool(targetBool.ToString(), status);
        }
    }
}