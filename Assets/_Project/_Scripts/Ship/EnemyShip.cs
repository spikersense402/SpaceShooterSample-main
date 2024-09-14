using UnityEngine;

public class EnemyShip : Ship, IShooter
{
    #region Shooting
    [Header("Shooting")]
    [SerializeField] private Transform[] _spawnPoints;
    public Transform[] SpawnPoints => _spawnPoints;

    [SerializeField] private Projectile _bullet;
    public Projectile Projectile => _bullet;

    [SerializeField] private float _interval;
    private float _elapsedTime;

    public void Shoot(Vector2 upDirection)
    {
        foreach (var spawnPoint in _spawnPoints)
        {
            var bullet = Instantiate(_bullet, spawnPoint.position, Quaternion.identity);
            bullet.Move(upDirection);
        }
    }
    #endregion

    #region Movement
    [Header("Movement")]
    [SerializeField] private float _minDistanceThreshold = 7f;
    [SerializeField] private float _maxDistanceThreshold = 9f;
    private float _distanceThreshold;

    private Transform _planetTransform;
    private Vector2 _directionToPlanet;

    private bool _isWithinShootingRange;

    // Zigzag movement parameters
    [Header("Zigzag Movement")]
    [SerializeField] private float _zigzagFrequency = 17f;
    [SerializeField] private float _zigzagAmplitude = 2100f;
    private bool _useZigzagMovement;
    private float _zigzagOffset;

    [Header("Effects")]
    [SerializeField] private GameObject _enemyExplosionPrefab; // Reference to the particle effect prefab

    private void Start()
    {
        _distanceThreshold = Random.Range(_minDistanceThreshold, _maxDistanceThreshold);
    }

    public void Init(Transform planetTransform)
    {
        _planetTransform = planetTransform;
        _useZigzagMovement = Random.value > 0.5f;
    }

    private void Update()
    {
        _directionToPlanet = _planetTransform.position - transform.position;
        transform.up = _directionToPlanet;

        _isWithinShootingRange = _directionToPlanet.sqrMagnitude < _distanceThreshold * _distanceThreshold;

        if (_isWithinShootingRange)
        {
            if (_elapsedTime < _interval)
            {
                _elapsedTime += Time.deltaTime;
            }
            else
            {
                Shoot(transform.up);
                _elapsedTime = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_isWithinShootingRange)
        {
            if (_useZigzagMovement)
            {
                // Apply zigzag movement
                _zigzagOffset = Mathf.Sin(Time.time * _zigzagFrequency) * _zigzagAmplitude;
                Vector2 zigzagDirection = _directionToPlanet.normalized + (Vector2.Perpendicular(_directionToPlanet) * _zigzagOffset);
                Rigidbody2D.velocity = zigzagDirection * MovementSpeed;
            }
            else
            {
                // Normal movement towards the planet
                Rigidbody2D.velocity = _directionToPlanet.normalized * MovementSpeed;
            }
        }
        else
        {
            // Handle circular motion around the planet when close
            Vector2 direction = (Vector2.Perpendicular(_directionToPlanet) * Mathf.Sin(Time.time * _zigzagFrequency) * _zigzagAmplitude).normalized;
            Rigidbody2D.velocity = direction * MovementSpeed;
        }
    }
    #endregion

    public override void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);
        if (IsDestroyed)
        {
            // Instantiate the particle effect at the enemy ship's position
            Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
