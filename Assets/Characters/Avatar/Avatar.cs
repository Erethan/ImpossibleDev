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

    public void OnAttack()
    {
        if (!staggered)
        {
           //animator.SetInteger(AnimationParameterConventions.ActionsParameterName, 1);
        }
    }


    protected virtual void Stagger(bool value)
    {
        if (!staggered && value)
        {
            //animator.SetInteger(AnimationParameterConventions.HitTypeParameterName, 1);
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
        int rightSpeedSign = Vector3.Angle(fowardVelocity, transform.forward) < 90 ? 1 : -1;

        //animator.SetFloat(AnimationParameterConventions.ForwardSpeedParameterName, fowardVelocity.magnitude * fowardSpeedSign);
        //animator.SetFloat(AnimationParameterConventions.StrafeSpeedParameterName, rightVelocity.magnitude * rightSpeedSign);

      
    }

    public void ReceiveHit()
    {
        Stagger(true);
    }

    private void OnAttackHit(Hitbox hitbox, HitReceiver receiver)
    {
        receiver.Hit(_swordHitType);
    }


    #region Input
    public void OnMovementInput(InputAction.CallbackContext context)
    {
        _movement.MovementInput = context.ReadValue<Vector2>();
    }

    public void OnDirectionLookInput(InputAction.CallbackContext context)
    {
        _rotator.RotationInput = context.ReadValue<Vector2>();
    }

    public void OnScreenLookInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Vector3 avatarScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        input.Set(input.x - avatarScreenPosition.x, input.y - avatarScreenPosition.y);

        _rotator.RotationInput = input;
    }
    #endregion

    #region Animation Events
    private void OnAxeSlashStart()
    {
        _swordHitbox.enabled = true;
    }

    private void OnAxeSlashEnd()
    {
        
        _swordHitbox.enabled = false;
    }

    private void OnHitRecoveryAnimationEvent()
    {
        Stagger(false);
    }
    #endregion
}
