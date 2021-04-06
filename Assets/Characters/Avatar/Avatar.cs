using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Avatar : Character2D, IHittable
{
    [SerializeField] protected VelocityMovement _movement = default;
    [SerializeField] protected AimDirection2D _aiming = default;
    [SerializeField] protected AvatarBlocker blocker = default;
    [SerializeField] protected AvatarAttacks _basicAttacks = default;

    protected bool _staggered = false;
    protected bool _defenseOrdered = false;

    public enum State
    {
        Free,
        Acting,
        Recovering
    }
    public State CurrentState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        var behaviours = _animator.GetBehaviours<LocomotionStateMachineBehaviour>();
        foreach (var behaviour in behaviours)
        {
            behaviour.StateEnter += OnLocomotionStateEnter;
        }
    }

    private void OnLocomotionStateEnter()
    {
        ChangeState(State.Free);
    }

    public override void Setup()
    {
        base.Setup();
        CurrentState = State.Free;

    }

    protected override void Update()
    {
        base.Update();
        _animator.SetFloat(AnimationConventions.AimDirectionKey, _aiming.AimAngle);
    }


    protected virtual void Stagger(bool value)
    {
        if (_staggered && value)
            return;

        _staggered = value;

        if (_staggered)
        {
            _animator.SetInteger(AnimationConventions.HitTypeKey, AnimationConventions.StaggerHitTypeValue);
            ChangeState(State.Recovering);
        }

    }


    public override void Hit(Hit attack)
    {
        bool attackBlocked = false;
        if(blocker.IsBlocking)
        {
            attackBlocked = blocker.TryBlock(attack, _rigidbody.transform.position);
        }

        if(attackBlocked)
        {
            BlockAttack(attack);
            return;
        }

        base.Hit(attack);

        if (!IsAlive)
            return;

        Stagger(true);
    }

    protected virtual void BlockAttack(Hit hit)
    {
        ChangeState(State.Recovering);
        if(CurrentStamina < hit.Damage)
        {
            _animator.SetInteger(AnimationConventions.HitTypeKey, AnimationConventions.GuardBreakTypeValue);
            CurrentStamina = 0;
            return;
        }
        CurrentStamina -= hit.Damage;
        _animator.SetInteger(AnimationConventions.HitTypeKey, AnimationConventions.BlockTypeValue);
    }

    protected override void Die()
    {
        base.Die();
        ChangeState(State.Recovering);
    }

    protected virtual void ChangeState(State newState)
    {
        bool isBecomingFree = newState != State.Free;
        _movement.Lock = isBecomingFree;
        _aiming.Lock = isBecomingFree;

        if(isBecomingFree)
        {
            _staggered = false;
        }

        CurrentState = newState;
        Debug.Log($"Avatar new state: {CurrentState}");
        UpdateDefenseState();
    }


    protected virtual void UpdateDefenseState()
    {
        bool block = _defenseOrdered && CurrentState == State.Free;
        _animator.SetBool(AnimationConventions.DefenseKey, block);

        if(!block)
        {
            blocker.DeactivateBlocking();
            return;
        }

        blocker.ActivateBlocking();
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

        if (CurrentState != State.Free)
            return;
        
        if (CurrentStamina <= 0)
            return;

        _animator.SetInteger(AnimationConventions.ActionTypeKey, 1);
        _basicAttacks.StartAttackChain();
        ChangeState(State.Acting);
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (CurrentState == State.Free)
        {
            _animator.SetInteger(AnimationConventions.ActionTypeKey, 2);
            ChangeState(State.Acting);
        }
    }


    public void OnDefendInput(InputAction.CallbackContext context)
    {
        _defenseOrdered =
            context.phase == InputActionPhase.Started
            || context.phase == InputActionPhase.Performed;
        UpdateDefenseState();
    }

    #endregion

    #region Animation Events
    private void OnAttackStart()
    {
        //Create Slash attack?


    }

    #endregion

}
