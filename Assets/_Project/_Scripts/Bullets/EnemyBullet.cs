using UnityEngine;

public class EnemyBullet : Projectile
{
    [Header("Effects")]
    [SerializeField] private GameObject _bulletExplosionPrefab; // Reference to the particle effect prefab

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        
            // Instantiate the particle effect at the bullet's position
            Instantiate(_bulletExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        
    }
}
