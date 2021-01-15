using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Avatar : Character
{

    [SerializeField] protected Rigidbody _avatarRigidbody = default;
    [SerializeField] protected GroundMovement _movement = default;
    [SerializeField] protected GroundRotation _rotator = default;
    [SerializeField] protected Animator animator = default;

    [Header("Actions")]
    [SerializeField] private Hitbox _swordHitbox = default;
    [SerializeField] private HitType _swordHitType = default;

    protected bool staggered = false;
    
    private void Start()
    {
        _swordHitbox.Hit += OnAttackHit;
        _swordHitbox.enabled = false;
    }

    

    protected virtual void Stagger(bool value)
    {
        if (!staggered && value)
        {
            animator.SetInteger(CharacterAnimationConventions.HitTypeParameterName, 1);
        }

        staggered = value;
        _movement.enabled = !staggered;
    }

    private void Update()
    {
        
        //Movement animation parameters
        Vector3 fowardVelocity = Vector3.Project(_avatarRigidbody.velocity, transform.forward);
        Vector3 rightVelocity = Vector3.Project(_avatarRigidbody.velocity, transform.right);
        int fowardSpeedSign = Vector3.Angle(fowardVelocity, transform.forward) < 90 ? 1 : -1;
        int rightSpeedSign = Vector3.Angle(rightVelocity, transform.forward) < 90 ? 1 : -1;

        animator.SetFloat(CharacterAnimationConventions.ForwardSpeedParameterName, fowardVelocity.magnitude * fowardSpeedSign);
        animator.SetFloat(CharacterAnimationConventions.StrafeSpeedParameterName, rightVelocity.magnitude * rightSpeedSign);

      
    }

   
    private void OnAttackHit(Hitbox hitbox, HitReceiver receiver)
    {
        receiver.Hit(_swordHitType);
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
        if (!staggered && _movement.enabled)
        {
            animator.SetInteger(CharacterAnimationConventions.ActionsParameterName, 1);
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
