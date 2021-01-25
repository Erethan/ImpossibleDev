using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Rebind : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAsset;
    [SerializeField] private string actionMapName;
    [SerializeField] private string actionName;

    [Header("Binding Events")]
    [SerializeField] private UnityEvent _operationStart;
    [SerializeField] private UnityEvent _operationCancel;
    [SerializeField] private UnityEvent _operationEnd;


    private InputActionRebindingExtensions.RebindingOperation rebindOperation;

    public void PerformInteractiveRebinding()
    {
        inputAsset.FindActionMap(actionMapName).FindAction(actionName).Disable();
        rebindOperation = 
            inputAsset.FindActionMap(actionMapName).FindAction(actionName).PerformInteractiveRebinding()
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Gamepad>/Start")
            .WithControlsExcluding("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindCompleted(operation));
        _operationStart.Invoke();

        rebindOperation.Start();

    }

    private void RebindCompleted(InputActionRebindingExtensions.RebindingOperation operation)
    {
        if(operation.canceled)
        {
            _operationCancel.Invoke();
        }

        _operationEnd.Invoke();

        operation.Dispose();
        inputAsset.FindActionMap(actionMapName).FindAction(actionName).Enable();

    }


}
