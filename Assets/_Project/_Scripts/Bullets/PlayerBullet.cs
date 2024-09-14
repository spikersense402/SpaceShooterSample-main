using UnityEngine;

public class PlayerBullet : Projectile
{
    [Header("Effects")]
    [SerializeField] private GameObject _bulletExplosionPrefab;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        // Check if the object has an IDamageable component (for enemies, meteors, etc.)
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            // Instantiate the particle effect if the prefab is set
            if (_bulletExplosionPrefab != null)
            {
                Instantiate(_bulletExplosionPrefab, transform.position, Quaternion.identity);
            }

            // Apply damage to the target
            damageable.TakeDamage(1f); // Adjust damage as needed
        }

        // Always destroy the bullet when it hits something
        Destroy(gameObject);
    }
}
