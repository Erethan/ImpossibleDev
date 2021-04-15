using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputWindowStateMachineBehaviour : StateMachineBehaviour
{
    [SerializeField] private InputActionAsset _inputAsset;
    [SerializeField] private string _actionName;
    [SerializeField] private string _bindingTag;

    [SerializeField] [Range(0, 1)] private float _normalizedStart;
    [SerializeField] [Range(0, 1)] private float _normalizedFinish;

    public string BindingTag => _bindingTag;

    public event Action ActionPerformed;

    private bool _initialized;
    private bool _allowInput;

    private void Initialize()
    {
        _inputAsset.FindAction(_actionName).performed += OnInputPerform;
        _initialized = true;

    }

    private void OnInputPerform(InputAction.CallbackContext context)
    {
        if (!_allowInput)
            return;
        Debug.Log($"Perform input ({_bindingTag})");
        ActionPerformed?.Invoke();

    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!_initialized)
        {
            Initialize();
        }

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
