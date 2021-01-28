using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Rebind : MonoBehaviour
{
    
    [SerializeField] private InputActionAsset inputAsset;
    [SerializeField] private DeviceDisplayConfigurator _deviceDisplayConfigurator;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private string actionMapName;
    [SerializeField] private string actionName;

    [Header("Binding Events")]
    [SerializeField] private UnityEvent<Sprite> _spriteUpdate;
    [SerializeField] private UnityEvent<string> _keyNameUpdate;
    [SerializeField] private UnityEvent _operationStart;
    [SerializeField] private UnityEvent _operationCancel;
    [SerializeField] private UnityEvent _operationEnd;


    private InputAction bindingAction;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;

    private void OnEnable()
    {
        if(bindingAction == null)
        {
            bindingAction = inputAsset.FindActionMap(actionMapName).FindAction(actionName);
        }

        UpdateBindingView();
    }

    public void PerformInteractiveRebinding()
    {
        bindingAction.Disable();
        rebindOperation = 
            inputAsset.FindActionMap(actionMapName).FindAction(actionName).PerformInteractiveRebinding()
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

        operation.Dispose();


        UpdateBindingView();
        bindingAction.Enable();
    }

    public void UpdateBindingView()
    {
        
        if (bindingAction.controls.Count == 0)
        {
            


            return;
        }
        int controlBindingIndex = bindingAction.GetBindingIndexForControl(bindingAction.controls[0]);
        string currentBindingInput = InputControlPath.ToHumanReadableString(bindingAction.bindings[controlBindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        Debug.LogWarning(currentBindingInput + " - " + bindingAction.controls[0] + " - " + controlBindingIndex + " - " + _playerInput.currentControlScheme);

        Sprite icon = _deviceDisplayConfigurator.GetDeviceBindingIcon(
            bindingAction.controls[0].device.ToString(),
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
