using UnityEngine;

public class GroundMovement : MonoBehaviour
{

    [Tooltip("Ground component that checks wether the player is grounded and can move.")]
    [SerializeField] protected Grounding _grounding = default;

    [Header("Speed")]
    [Tooltip("Force applied on the grounding rigidbody in the direction of the movement input. This force is only applied if movement has not reached it's target speed, and it does not overlap the target speed.")]
    [SerializeField] protected float _motorForce = 4f;

    [Tooltip("Target speed before considering speed differences in direction.")]
    [SerializeField] protected float _baseSpeed = 8.0f;


    [Tooltip("Factor increase in the final velocity if Run is set to true.")]
    [SerializeField] protected float _runMultiplier = 1.5f;

    [Tooltip("Factor applied on Base Speed to access maximum speed in each different input direction. This function domain is input angle and its image is the ratio of the force applied.")]
    [SerializeField]protected AnimationCurve _directionSpeedFactor = new AnimationCurve(
        new Keyframe(0, 1),
        new Keyframe(90, 0.7f),
        new Keyframe(180, 0.5f));

    [Tooltip("Factor applied on Base Speed do slowdown movement when climbing slopes. It receives the slope angle and it returns the raito of the force that is applied.")]
    [SerializeField] protected AnimationCurve _slopeCurveModifier = new AnimationCurve(
        new Keyframe(-90.0f, 1.0f),
        new Keyframe(20.0f, 1.0f),
        new Keyframe(40.0f, 0.6f),
        new Keyframe(70.0f, 0.0f));

    [Header("Drag")]
    [Tooltip("The ratio of the drag when its rigidbody is moving")]
    [Range(0, 1)] [SerializeField] private float _dinamicDragFactor = 0.25f;

    [SerializeField] private AnimationCurve _directionalDrag = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1), new Keyframe(180, 1) });
    protected float DirectionalDragFactor
    {
        get
        {
            Vector3 projection = Vector3.ProjectOnPlane(_grounding.Rigidbody.velocity, _grounding.Rigidbody.transform.up);
            return _directionalDrag.Evaluate(Vector3.SignedAngle(projection, _grounding.Rigidbody.transform.forward, _grounding.Rigidbody.transform.up));

        }
    }
    protected DragFactorSource _directionalDragFactorSource;
    protected DragFactorSource _dinamicDragFactorSource;

    private Vector2 movementInput;
    public Vector2 MovementInput
    {
        get { return movementInput; }
        set
        {
            movementInput = value;
            if (movementInput.magnitude > 1)
            {
                movementInput.Normalize();
            }
        }
    }

    public bool Run { get; set; }

    private Vector3 DesiredMovementDirection
    {
        get
        {
            Vector3 forwardReference = Vector3.ProjectOnPlane(_grounding.DirectionReference.transform.up, _grounding.ContactNormal).normalized;
            Vector3 rightReference = Vector3.Cross(forwardReference, -_grounding.ContactNormal).normalized;
            return forwardReference * MovementInput.y + rightReference * MovementInput.x;
        }
    }


    protected float SlopeMultiplier
    {
        get
        {
            float slopeAngle = Vector3.Angle(_grounding.ContactNormal, Vector3.up);
            return _slopeCurveModifier.Evaluate(slopeAngle);
        }
    }


    protected float currentTargetSpeed;
    public Vector3 Velocity
    {
        get { return _grounding.Rigidbody.velocity; }
    }

    protected void Awake()
    {
        _dinamicDragFactorSource = new DragFactorSource() { Value = _dinamicDragFactor };
        _directionalDragFactorSource = new DragFactorSource();
        _grounding.AddDragFactor(_dinamicDragFactorSource);
        //_grounding.AddDragFactor(_directionalDragFactorSource);
    }


    protected void FixedUpdate()
    {
        if ((Mathf.Abs(MovementInput.x) <= float.Epsilon && Mathf.Abs(MovementInput.y) <= float.Epsilon)
            || !_grounding.Grounded)
        {
            _dinamicDragFactorSource.Value = 1;
            _directionalDragFactorSource.Value = 1;
        }
        else
        {
            float velocityMagnitude = _grounding.Rigidbody.velocity.magnitude;
            UpdateDesiredTargetSpeed();
            Vector3 desiredMove = DesiredMovementDirection;

            desiredMove = Vector3.ProjectOnPlane(desiredMove, _grounding.ContactNormal);
            desiredMove = desiredMove.normalized;

            if (velocityMagnitude < currentTargetSpeed)
            {
                Vector3 rawImpulse = desiredMove * SlopeMultiplier * _motorForce * MovementInput.magnitude;

                Vector3 clampedImpulse =
                    rawImpulse.normalized
                    * Mathf.Clamp(
                        rawImpulse.magnitude,
                        0,
                        currentTargetSpeed * _grounding.Rigidbody.mass / Time.fixedDeltaTime);

                _grounding.Rigidbody.AddForce(clampedImpulse);
            }

            _dinamicDragFactorSource.Value = _dinamicDragFactor;
            _directionalDragFactorSource.Value = DirectionalDragFactor;
            

        }
    }

    protected virtual void UpdateDesiredTargetSpeed()
    {
        if (MovementInput == Vector2.zero)
            return;

        Vector3 movDir = DesiredMovementDirection;
        Vector3 cross = Vector3.Cross(movDir, _grounding.Rigidbody.transform.forward);
        float inputAngle = Vector3.SignedAngle(movDir, _grounding.Rigidbody.transform.forward, cross);
        currentTargetSpeed = 
            _baseSpeed
            * _directionSpeedFactor.Evaluate(inputAngle)
            * (Run ? _runMultiplier : 1);

    }

}
