using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputWindowStateMachineBehaviour : StateMachineBehaviour
{
    [SerializeField] private InputActionAsset _inputAsset;
    [SerializeField] private string _actionName;
    [SerializeField] private string _parameterName;
    [SerializeField] private int _value;


    [SerializeField] [Range(0, 1)] private float _normalizedStart;
    [SerializeField] [Range(0, 1)] private float _normalizedFinish;

    private Animator _animator;
    private bool _initialized;
    private bool _allowInput;

    private void Initialize()
    {
        _inputAsset.FindAction(_actionName).performed += OnInputPerform;
        _initialized = true;
    }

    private void OnInputPerform(InputAction.CallbackContext context)
    {
        if (_allowInput)
        {
            _animator.SetInteger(_parameterName, _value);
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!_initialized)
        {
            Initialize();
        }

        _animator = animator;
        _allowInput = false;
        float startDelay = _normalizedStart * stateInfo.length;
        float duration= (_normalizedFinish - _normalizedStart) * stateInfo.length;

        WindowTask(startDelay, Mathf.Clamp(duration,0,stateInfo.length) );
    }

    private async void WindowTask(float startDelay, float duration)
    {
        int startDelayMs = Mathf.FloorToInt(startDelay * 1000);
        int windowDuration = Mathf.FloorToInt(duration * 1000);

        await Task.Delay(startDelayMs);
        _allowInput = true;
        await Task.Delay(windowDuration);
        _allowInput = false;

    }



}
