using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Avatar : Character
{

    [SerializeField] protected Rigidbody avatarRigidbody = default;
    [SerializeField] protected GroundMovement movement = default;
    [SerializeField] protected Animator animator = default;

    [Header("Actions")]
    [SerializeField] private Hitbox swordHitbox = default;
    [SerializeField] private HitType swordHitType = default;

    protected bool staggered = false;
    
    private void Start()
    {
        swordHitbox.Hit += OnAttackHit;
        swordHitbox.enabled = false;
    }


    public void OnMovementInput(InputAction.CallbackContext context)
    {
        movement.MovementInput = context.ReadValue<Vector2>();
    }
    public void OnDirectionLookInput(InputAction.CallbackContext context)
    {
        Debug.Log(GetComponent<PlayerInput>().currentControlScheme);
    }
    public void OnScreenLookInput(InputAction.CallbackContext context)
    {
        Debug.Log(GetComponent<PlayerInput>().currentControlScheme);
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
        movement.enabled = !staggered;
    }

    private void Update()
    {
        
        //Movement animation parameters
        Vector3 fowardVelocity = Vector3.Project(avatarRigidbody.velocity, transform.forward);
        Vector3 rightVelocity = Vector3.Project(avatarRigidbody.velocity, transform.right);
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
        receiver.Hit(swordHitType);
    }


    #region Animation Events
    private void OnAxeSlashStart()
    {
        swordHitbox.enabled = true;
    }

    private void OnAxeSlashEnd()
    {
        
        swordHitbox.enabled = false;
    }

    private void OnHitRecoveryAnimationEvent()
    {
        Stagger(false);
    }
    #endregion
}
