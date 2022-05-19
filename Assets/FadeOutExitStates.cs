using System.Collections;
using System.Collections.Generic;
using UIScripts.Menus;
using UnityEngine;

public class FadeOutExitStates : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) =>
        MenuController.Instance.ShowMenu();
}