using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorIntegerParameter : StateMachineBehaviour
{
    [SerializeField] private string _parameterName;
    [SerializeField] private int _defaultValue;

    [SerializeField] private bool _onSubStateOnly;
    [SerializeField] private bool _resetOnStateEnter;
    [SerializeField] private bool _resetOnStateExit;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_onSubStateOnly)
            return;

        if (!_resetOnStateEnter)
            return;

        animator.SetInteger(_parameterName, _defaultValue);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_onSubStateOnly)
            return;

        if (!_resetOnStateExit)
            return;
        animator.SetInteger(_parameterName, _defaultValue);
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (_onSubStateOnly)
            return;

        if (!_resetOnStateEnter)
            return;

        animator.SetInteger(_parameterName, _defaultValue);
    }


    public override void OnStateMachineExit(Animator animator, int layerIndex)
    {
        if (_onSubStateOnly)
            return;

        if (!_resetOnStateExit)
            return;

        animator.SetInteger(_parameterName, _defaultValue);
    }

}
