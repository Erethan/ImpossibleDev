using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorIntegerParameter : StateMachineBehaviour
{
    public string parameterName;
    public int defaultValue;

    public bool resetOnStateEnter;
    public bool resetOnStateExit;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (resetOnStateEnter)
        {
            animator.SetInteger(parameterName, defaultValue);
        }
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (resetOnStateExit)
        {
            animator.SetInteger(parameterName, defaultValue);
        }
    }

}
