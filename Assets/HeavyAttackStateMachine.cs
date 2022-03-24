using Helpers;
using PlayerScripts;
using UnityEngine;
using static Helpers.Literals;

public class HeavyAttackStateMachine : StateMachineBehaviour,IMediatorUser 
{
    private PlayerMechanics _playerMechanics;
    private const int MaxAttackCounter = 4;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerMechanics.AttackCounter++;
        if (_playerMechanics.AttackCounter >= MaxAttackCounter)
        {
            _playerMechanics.AttackCounter = 0;
        }
        
        Debug.Log("Exit " + _playerMechanics.AttackCounter);
        animator.SetInteger(PlayerParametersStrings.AttackCount, _playerMechanics.AttackCounter);
        animator.SetBool(PlayerParametersStrings.Attack, false);
        animator.SetBool(PlayerParametersStrings.HeavyAttack, false);   
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
    }

    public void ConfigureMediator(PlayerMediator med)
    {
        _playerMechanics = med.PlayerMechanics;
    }
}
