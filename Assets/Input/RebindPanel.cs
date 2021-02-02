using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class RebindPanel : MonoBehaviour
{
    private RebindingOperation _rebindOperation;


    public RebindingOperation TryStartRebindingOperation(InputAction bindingAction)
    {
        if (_rebindOperation != null)
        {
            return null;
        }

        _rebindOperation =
           bindingAction.PerformInteractiveRebinding()
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Gamepad>/Start")
            .WithControlsExcluding("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindCompleted(operation))
            .Start();
        return _rebindOperation;
    }

    private void RebindCompleted(RebindingOperation operation)
    {
      

        operation.Dispose();
        _rebindOperation = null;
    }
}
