using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorIntegerParameter : StateMachineBehaviour
{
    public string parameterName;
    public int defaultValue;

    public bool resetOnStateEnter;
    public bool resetOnStateExit;

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (resetOnStateEnter)
        {
            animator.SetInteger(parameterName, defaultValue);
        }
    }


    public override void OnStateMachineExit(Animator animator, int layerIndex)
    {
        if (resetOnStateExit)
        {
            animator.SetInteger(parameterName, defaultValue);
        }
    }

}
