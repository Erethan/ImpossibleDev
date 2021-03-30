using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D _rigidbody;

    [Header("Speed")]
    [Tooltip("Force applied on the grounding rigidbody in the direction of the movement input. This force is only applied if movement has not reached it's target speed, and it does not overlap the target speed.")]
    [SerializeField] protected float _motorForce = 4f;

    [Tooltip("Target speed before considering speed differences in direction.")]
    [SerializeField] protected float _baseSpeed = 8.0f;


    [Tooltip("Factor increase in the final velocity if Run is set to true.")]
    [SerializeField] protected float _runMultiplier = 1.5f;

    [Tooltip("Factor applied on Base Speed to evaluate maximum speed in each different input direction. This f" +
        "unction domain is input angle and its image is the ratio of the force applied.")]
    [SerializeField]
    protected AnimationCurve _directionSpeedFactor = new AnimationCurve(
        new Keyframe(0, 1),
        new Keyframe(90, 0.7f),
        new Keyframe(180, 0.5f));

    [Header("Drag")]
    [SerializeField] private float _staticDrag = 10;

    [Tooltip("The ratio of the drag when its rigidbody is moving")]
    [Range(0, 1)] [SerializeField] private float _dinamicDragFactor = 0.25f;


    private List<DragFactorSource> _externalSourcesDragFactors = new List<DragFactorSource>();
    public int AddDragFactor(DragFactorSource dragSource)
    {
        _externalSourcesDragFactors.Add(dragSource);
        return _externalSourcesDragFactors.Count;
    }
    public void RemoveDragFactor(int sourceIndex) => _externalSourcesDragFactors.RemoveAt(sourceIndex);
    public void RemoveDragFactor(DragFactorSource dragSource) => _externalSourcesDragFactors.Remove(dragSource);

    public float ExternalSourcesDragFactor
    {
        get
        {
            float externalDrag = 1;
            for (int i = 0; i < _externalSourcesDragFactors.Count; i++)
            {
                externalDrag *= _externalSourcesDragFactors[i].Value;
            }
            return externalDrag;
        }
    }

    protected DragFactorSource _dinamicDragFactorSource;

    private Vector2 _movementInput;
    public Vector2 MovementInput
    {
        get { return _movementInput; }
        set
        {
            _movementInput = value;
            if (_movementInput.magnitude > 1)
            {
                _movementInput.Normalize();
            }
        }
    }

    public bool Run { get; set; }

    protected float _currentTargetSpeed;
    public Vector2 Velocity
    {
        get { return _rigidbody.velocity; }
    }


    protected void Awake()
    {
        _dinamicDragFactorSource = new DragFactorSource() { Value = _dinamicDragFactor };
        AddDragFactor(_dinamicDragFactorSource);
    }


    protected void FixedUpdate()
    {
        if (Mathf.Abs(MovementInput.x) <= float.Epsilon
            && Mathf.Abs(MovementInput.y) <= float.Epsilon)
        {
            _dinamicDragFactorSource.Value = 1;
        }
        else
        {
            float velocityMagnitude = _rigidbody.velocity.magnitude;
            UpdateDesiredTargetSpeed();
            Vector2 desiredMove = MovementInput;


            if (velocityMagnitude < _currentTargetSpeed)
            {
                Vector3 rawImpulse = desiredMove * _motorForce * MovementInput.magnitude;

                Vector3 clampedImpulse =
                    rawImpulse.normalized
                    * Mathf.Clamp(
                        rawImpulse.magnitude,
                        0,
                        _currentTargetSpeed * _rigidbody.mass / Time.fixedDeltaTime);

                _rigidbody.AddForce(clampedImpulse);
            }

            _dinamicDragFactorSource.Value = _dinamicDragFactor;


        }
        _rigidbody.drag = _staticDrag * ExternalSourcesDragFactor;
    }

    protected virtual void UpdateDesiredTargetSpeed()
    {
        if (MovementInput == Vector2.zero)
            return;

        Vector3 cross = Vector3.Cross(MovementInput, _rigidbody.transform.forward);
        float inputAngle = Vector3.SignedAngle(MovementInput, _rigidbody.transform.forward, cross);
        _currentTargetSpeed =
            _baseSpeed
            * _directionSpeedFactor.Evaluate(inputAngle)
            * (Run ? _runMultiplier : 1);

    }
}
