using System;
using UnityEngine;

public class StateChangeStateMachineBehaviour : StateMachineBehaviour
{
    public event Action StateEnter, StateExit;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        StateEnter?.Invoke();
        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        StateExit?.Invoke();

    }


}
