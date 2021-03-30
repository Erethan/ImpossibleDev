using UnityEngine;
using UnityEngine.InputSystem;

public class Avatar : Character
{
    [SerializeField] protected Rigidbody _avatarRigidbody = default;
    [SerializeField] protected GroundMovement _movement = default;
    [SerializeField] protected GroundRotation _rotator = default;
    [SerializeField] protected float _animationLerpSpeed = 1;

    [Header("Actions")]
    [SerializeField] private Hitbox2D _swordHitbox = default;
    [SerializeField] private HitType _swordHitType = default;

    protected bool staggered = false;

    private void Start()
    {
        _swordHitbox.HitEvent += OnAttackHit;
        _swordHitbox.enabled = false;
    }


    protected virtual void Stagger(bool value)
    {
        if (!staggered && value)
        {
            _animator.SetInteger(AnimationConventions.HitTypeKey, 1);
        }

        staggered = value;
        _movement.enabled = !staggered;
    }


    private void OnAttackHit(IHittable hittable)
    {
        Attack swordHit = new Attack
        { 
            Type = _swordHitType,
            Damage = 1 
        };

        hittable.Hit(swordHit);
    }

    public void ReceiveHit()
    {
        Stagger(true);
    }


    #region Input Events
    public void OnMovementInput(InputAction.CallbackContext context)
    {
        _movement.MovementInput = context.ReadValue<Vector2>();
    }

    public void OnDirectionLookInput(InputAction.CallbackContext context)
    {
        _rotator.Mode = GroundRotation.InputMode.Directional;
        _rotator.RotationInput = context.ReadValue<Vector2>();
    }

    public void OnScreenLookInput(InputAction.CallbackContext context)
    {
        _rotator.Mode = GroundRotation.InputMode.ScreenPosition;
        _rotator.RotationInput = context.ReadValue<Vector2>();

    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        Debug.Log("OnAttackInput");
        if (!staggered && _movement.enabled)
        {
            _animator.SetInteger(AnimationConventions.ActionTypeKey, 1);
        }
    }

    #endregion

    #region Animation Events
    private void OnAttackStart()
    {
        _swordHitbox.enabled = true;
    }

    private void OnAttackEnd()
    {

        _swordHitbox.enabled = false;
    }

    private void OnRecovered()
    {
        Stagger(false);
    }
    #endregion
}
