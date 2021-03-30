using UnityEngine;
using UnityEngine.InputSystem;

public class Avatar : Character2D
{
    [SerializeField] protected VelocityMovement _movement = default;
    [SerializeField] protected AimDirection2D _aiming = default;

    protected bool _staggered = false;


    protected virtual void Stagger(bool value)
    {
        if (!_staggered && value)
        {
            _animator.SetInteger(AnimationConventions.HitTypeKey, AnimationConventions.StaggerHitTypeValue);
        }

        _staggered = value;
        _movement.enabled = !_staggered;
    }


    public override void ReceiveHit(Attack attack)
    {
        base.ReceiveHit(attack);

        if (_staggered)
            return;

        if (!IsAlive)
            return;

        Stagger(true);

    }


    protected override void Update()
    {
        base.Update();
        _animator.SetFloat(AnimationConventions.AimDirectionKey, _aiming.AimAngle);
    }

    #region Input Events
    public void OnMovementInput(InputAction.CallbackContext context)
    {
        _movement.MovementInput = context.ReadValue<Vector2>();
    }

    public void OnDirectionLookInput(InputAction.CallbackContext context)
    {
        _aiming.Mode  = AimDirection2D.InputMode.Directional;
        _aiming.RotationInput = context.ReadValue<Vector2>();
    }

    public void OnScreenLookInput(InputAction.CallbackContext context)
    {
        _aiming.Mode  = AimDirection2D.InputMode.ScreenPosition;
        _aiming.RotationInput = context.ReadValue<Vector2>();

    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        Debug.Log("OnAttackInput");
        if (!_staggered && _movement.enabled)
        {
            _animator.SetInteger(AnimationConventions.ActionTypeKey, 1);
        }
    }

    #endregion

    #region Animation Events
    private void OnAttackStart()
    {
        //Create Slash attack?
    }


    private void OnRecovered()
    {
        Stagger(false);
    }
    #endregion
}
