using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDirection2D : MonoBehaviour
{
    [SerializeField] protected Transform _reference;

    public enum InputMode { Directional, ScreenPosition }
    
    public InputMode Mode { get; set; }
    public float AimAngle { get; private set; }
    public bool Lock { get; set; }

    protected Vector2 _rotationInput = new Vector3();
    public Vector2 RotationInput
    {
        get
        {
            return _rotationInput;
        }
        set
        {
            
            if (Lock)
            {
                return;
            }
            _rotationInput = value;

            switch (Mode)
            {
                case InputMode.Directional:
                    AimAngle =  (Mathf.Atan2(_rotationInput.y, _rotationInput.x) * Mathf.Rad2Deg);
                    break;
                case InputMode.ScreenPosition:
                    Vector3 referenceScreenPosition = Camera.main.WorldToScreenPoint(_reference.position);
                    referenceScreenPosition.Set(_rotationInput.x - referenceScreenPosition.x, _rotationInput.y - referenceScreenPosition.y, 0);
                    AimAngle = (Mathf.Atan2(referenceScreenPosition.y, referenceScreenPosition.x) * Mathf.Rad2Deg);
                    break;
                default:
                    AimAngle = 0;
                    break;
            }

            if (_rotationInput.sqrMagnitude > 1)
            {
                _rotationInput.Normalize();
            }

        }

    }


}
