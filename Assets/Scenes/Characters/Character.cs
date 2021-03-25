using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [SerializeField] protected CharacterStatistics _stats;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Rigidbody _rigidbody;

    [SerializeField] protected UnityEvent<float> _healthUpdate;
    [SerializeField] protected UnityEvent<float> _manaUpdate;
    [SerializeField] protected UnityEvent<float> _staminaUpdate;

    protected float _currentHealth;
    public float CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        private set
        {
            bool valueChanged = _currentHealth != value;
            _currentHealth = value;
            if(valueChanged)
            {
                _healthUpdate.Invoke(_currentHealth);
            }
        }
    }

    protected float _currentMana;
    public float CurrentMana
    {
        get
        {
            return _currentMana;
        }
        private set
        {
            bool valueChanged = _currentMana != value;
            _currentMana = value;
            if (valueChanged)
            {
                _healthUpdate.Invoke(_currentMana);
            }
        }
    }

    protected float _currentStamina;
    public float CurrentStamina
    {
        get
        {
            return _currentStamina;
        }
        private set
        {
            bool valueChanged = _currentStamina != value;
            _currentStamina = value;
            if (valueChanged)
            {
                _healthUpdate.Invoke(_currentStamina);
            }
        }
    }
    

    protected bool _isAlive;
    public bool IsAlive => _isAlive;

    protected virtual void Update()
    {
        UpdateAnimationParameters();
    }

    protected virtual void Awake()
    {
        Setup();
    }

    public virtual void Setup()
    {
        CurrentHealth = _stats.Health;
        CurrentMana = _stats.Mana;
        CurrentStamina = _stats.Stamina;
        _isAlive = true;
    }

    protected virtual void UpdateAnimationParameters()
    {
        _animator.SetFloat(AnimationConventions.SpeedKey, _rigidbody.velocity.magnitude);
        _animator.SetFloat(AnimationConventions.DirectionKey, transform.eulerAngles.z);
    }

    public virtual void GetHit(Attack attack)
    {
        if (!IsAlive)
            return;
        CurrentHealth -= attack.Damage;
        
        if(CurrentHealth <= 0)
        {
            Die();
        }
    }


    public virtual void Die()
    {
        _isAlive = false;
        _animator.SetInteger(AnimationConventions.HitTypeKey, AnimationConventions.DeathHitTypeValue);
    }

}
