using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityMovement : MonoBehaviour
{
   
    [SerializeField] protected Rigidbody2D _rigidbody;

    [SerializeField] private float _baseSpeed = 1;

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
        _rigidbody.drag = _staticDrag * ExternalSourcesDragFactor;

        if (Mathf.Abs(MovementInput.x) <= float.Epsilon
            && Mathf.Abs(MovementInput.y) <= float.Epsilon)
        {
            _dinamicDragFactorSource.Value = 1;
            return;
        }

        _rigidbody.velocity = MovementInput * _baseSpeed;
        _dinamicDragFactorSource.Value = _dinamicDragFactor;
        
    }
}
