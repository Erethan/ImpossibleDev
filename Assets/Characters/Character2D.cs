using UnityEngine;
using UnityEngine.Events;

public class Character2D : MonoBehaviour, IHittable
{
    [SerializeField] protected CharacterStatistics _stats;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Rigidbody2D _rigidbody;

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
        protected set
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
        protected set
        {
            bool valueChanged = _currentMana != value;
            _currentMana = value;
            if (valueChanged)
            {
                _manaUpdate.Invoke(_currentMana);
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
        protected set
        {
            bool valueChanged = _currentStamina != value;
            _currentStamina = value;
            if (valueChanged)
            {
                _staminaUpdate.Invoke(_currentStamina);
            }
        }
    }

    public Animator Animator => _animator;

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
    }

    public virtual void Hit(Hit hit)
    {
        if (!IsAlive)
            return;

        if(CurrentHealth <= hit.Damage)
        {
            CurrentHealth = 0;
            Die();
            return;
        }

        CurrentHealth -= hit.Damage;
    }

    

    protected virtual void Die()
    {
        _isAlive = false;
        _animator.SetInteger(AnimationConventions.HitTypeKey, AnimationConventions.DeathHitTypeValue);
    }

    public virtual void SpendStamina(float value)
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina - value, 0, _stats.Stamina);
    }

}
