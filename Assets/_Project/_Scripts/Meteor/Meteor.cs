using UnityEngine;

public class Meteor : Projectile, IDamageable
{
    [Header("Health")]
    [SerializeField] private float _maxHealth = 5f;
    private float _currentHealth;
    private bool _isDestroyed;

    public float MaxHealth => _maxHealth;

    public float CurrentHealth
    {
        get
        {
            if (_currentHealth == 0 && !_isDestroyed)
            {
                return MaxHealth;
            }
            else
            {
                return _currentHealth;
            }
        }
        private set
        {
            _currentHealth = Mathf.Clamp(value, 0f, MaxHealth);
            _isDestroyed = _currentHealth == 0;
        }
    }

    public bool IsDestroyed => _isDestroyed;

    public delegate void MeteorDestroyed(Meteor meteor);
    public event MeteorDestroyed OnDestroyed;

    public virtual void TakeDamage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        if (IsDestroyed)
        {
            OnDestroyed?.Invoke(this);  // Notify that the meteor has been destroyed
            Destroy(gameObject);
        }
    }

    public override void Move(Vector2 upDirection)
    {
        Rigidbody2D.AddForce(upDirection * MovementSpeed);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if (CurrentHealth < MaxHealth && other.CompareTag("Bounds"))
        {
            OnDestroyed?.Invoke(this);  // Notify that the meteor has been destroyed
            Destroy(gameObject);
        }
    }
}
