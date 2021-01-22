using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Rebind : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAction;

    [SerializeField] private UnityEvent _operationStart;
    [SerializeField] private UnityEvent _operationCancel;
    [SerializeField] private UnityEvent _operationComplete;

    public void PerformInteractiveRebinding()
    {
        inputAction.actionMaps[0].actions[0].PerformInteractiveRebinding()
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Gamepad>/Start")
            .WithControlsExcluding("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindCompleted(operation));
        _operationStart.Invoke();
    }

    private void RebindCompleted(InputActionRebindingExtensions.RebindingOperation operation)
    {
        if(operation.canceled)
        {
            _operationCancel.Invoke();
        }

        _operationComplete.Invoke();

        operation.Dispose();

    }

   
}
