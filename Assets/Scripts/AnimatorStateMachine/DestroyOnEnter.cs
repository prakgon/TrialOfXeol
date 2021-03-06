using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnEnter : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MenuController.Instance.ShowMenu();
        MenuController.Instance.DestroyCanvas();
        MenuController.Instance.DestroyInitialText();
        MenuController.Instance.DestroyInitialImage();
    }
}