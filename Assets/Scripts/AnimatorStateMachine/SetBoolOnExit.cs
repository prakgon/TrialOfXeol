using System.Collections;
using System.Collections.Generic;
using UIScripts.Menus;
using UnityEngine;

public class SetBoolOnExit : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) =>
        MenuController.Instance.GoNextPhase = true;
}