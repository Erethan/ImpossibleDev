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
            float targetValue = Mathf.Clamp(value, 0, _stats.Health);
            bool valueChanged = _currentHealth != targetValue;
            if (targetValue < _currentHealth)
            {
                _lastHealthSpentTime = Time.time;
            }


            _currentHealth = targetValue;

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
            float targetValue = Mathf.Clamp(value, 0, _stats.Mana);
            bool valueChanged = _currentMana != targetValue;
            if (targetValue < _currentMana)
            {
                _lastManaSpentTime = Time.time;
            }

            _currentMana = targetValue;
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
            float targetValue = Mathf.Clamp(value, 0, _stats.Stamina);
            bool valueChanged = _currentStamina != targetValue;
            if(targetValue < _currentStamina)
            {
                _lastStaminaSpentTime = Time.time;
            }

            _currentStamina = targetValue;
            if (valueChanged)
            {
                _staminaUpdate.Invoke(_currentStamina);
            }
        }
    }

    public Animator Animator => _animator;

    protected bool _isAlive;
    public bool IsAlive => _isAlive;

    private float _lastHealthSpentTime;
    private float _lastManaSpentTime;
    private float _lastStaminaSpentTime;

    protected virtual void Update()
    {
        UpdateAnimationParameters();
        ResourcesRegenaration();
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
    
    protected virtual void ResourcesRegenaration()
    {
        if (Time.time > _lastHealthSpentTime + _stats.HealthRegenDelay)
        {
            CurrentHealth += _stats.HealthRegen * Time.deltaTime;
        }

        if (Time.time > _lastManaSpentTime + _stats.ManaRegenDelay)
        {
            CurrentStamina += _stats.ManaRegen * Time.deltaTime;
        }

        if (Time.time > _lastStaminaSpentTime + _stats.StaminaRegenDelay)
        {
            CurrentStamina += _stats.StaminaRegen * Time.deltaTime;
        }
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
