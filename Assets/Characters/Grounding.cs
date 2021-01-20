using System.Collections.Generic;
using UnityEngine;

public class Grounding : MonoBehaviour
{


    [Tooltip("Input direction is relative to the reference transform if different than null. This is usually a Camera or the character's root transform.")]
    [SerializeField] protected Transform _directionReference = default;
    public Transform DirectionReference => _directionReference;

    public enum Axis { X, Y, Z }

    [Tooltip("Movement and rotation is relative to the plane defined by NormalAxis.")]
    [SerializeField] private Axis _normalAxis = Axis.Y;
    public Axis NormalAxis => _normalAxis;

    [Header("Drag")]
    [SerializeField] private Rigidbody _rigidbody = default;
    [SerializeField] private float _sphereCastRadius = 0.01f;
    [SerializeField] private float _contactDistance = 0.5f;
    [SerializeField] private float _airDrag = 0.1f;
    [SerializeField] private float _staticDrag = 10;
    


    public Rigidbody Rigidbody { get { return _rigidbody; }}
    public Vector3 ContactPosition { get; private set; }
    public Vector3 ContactNormal { get; private set; }
    public bool Grounded { get; private set; }
    public bool AllowGroundContact { get; set; } = true;
    
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


    protected float _floatingEndTime;

  

    
    public Vector3 FarthestGroundingPostion
    {
        get
        {
            return transform.position + Vector3.down * (_sphereCastRadius + _contactDistance);
        }
    }


    public Vector3 FindPointBelow (float maxDistance)
    {
        if (Physics.SphereCast(transform.position, _sphereCastRadius/10, Vector3.down, out RaycastHit hitInfo, maxDistance, Physics.DefaultRaycastLayers))
        {
            return hitInfo.point;
        }

        return FarthestGroundingPostion + Vector3.down * Mathf.Infinity;
    }


    private void FixedUpdate()
    {
        bool forceFloating = Time.time < _floatingEndTime;
        
        
        if (AllowGroundContact
            && !forceFloating
            && Physics.SphereCast(transform.position, _sphereCastRadius, Vector3.down, out RaycastHit hitInfo, _contactDistance, Physics.DefaultRaycastLayers))
        {
            Grounded = true;
            ContactNormal = hitInfo.normal;
            ContactPosition = hitInfo.point;
            _rigidbody.drag = _staticDrag * ExternalSourcesDragFactor;
            
        }
        else
        {
            Grounded = false;
            ContactNormal = Vector3.up;
            _rigidbody.drag = _airDrag * ExternalSourcesDragFactor;
        }
    }

    //Use it to ignore grounding for 'duration' in seconds
    public void ForceFloating(float duration)
    {
        _floatingEndTime = Time.time + duration;
    }



    //TODO: Add sideways movement friction
}
