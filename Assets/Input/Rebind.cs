using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Rebind : MonoBehaviour
{
    
    [SerializeField] private InputActionReference actionReference;
    [SerializeField] private DeviceDisplayConfigurator _deviceDisplayConfigurator;

    [Header("Binding Events")]
    [SerializeField] private UnityEvent<Sprite> _spriteUpdate;
    [SerializeField] private UnityEvent<string> _keyNameUpdate;
    [SerializeField] private UnityEvent _operationStart;
    [SerializeField] private UnityEvent _operationCancel;
    [SerializeField] private UnityEvent _operationEnd;


    private InputAction _rebindingAction;
    private InputActionRebindingExtensions.RebindingOperation _rebindOperation;

    private void OnEnable()
    {
        if(_rebindingAction == null)
        {
            _rebindingAction = actionReference.action;
        }

        UpdateBindingView();
    }

    public void PerformInteractiveRebinding()
    {
        _rebindingAction.Disable();
        _rebindOperation =
           _rebindingAction.PerformInteractiveRebinding(_rebindingAction.GetBindingIndexForControl(_rebindingAction.controls[0]))
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Gamepad>/Start")
            .WithControlsExcluding("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindCompleted(operation))
            .Start();
        _operationStart.Invoke();

    }

    private void RebindCompleted(InputActionRebindingExtensions.RebindingOperation operation)
    {
        if(operation.canceled)
        {
            _operationCancel.Invoke();
        }

        _operationEnd.Invoke();


        UpdateBindingView();
        operation.Dispose();
        _rebindOperation = null;
    }

    public void UpdateBindingDevice( PlayerInput playerInput)
    {
        UpdateBindingView();
    }

    public void UpdateBindingView()
    {
        if (_rebindingAction.controls.Count == 0)
        {
            return;
        }


        int controlBindingIndex = _rebindingAction.GetBindingIndexForControl(_rebindingAction.controls[0]);
        string currentBindingInput = InputControlPath.ToHumanReadableString(_rebindingAction.bindings[controlBindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        

        Sprite icon = _deviceDisplayConfigurator.GetDeviceBindingIcon(
            _rebindingAction.controls[0].device.ToString(),
            currentBindingInput);

        if (icon != null)
        {
            _spriteUpdate.Invoke(icon);
        }
        else
        {
            _keyNameUpdate.Invoke(currentBindingInput);
        }

    }
    
    
}
