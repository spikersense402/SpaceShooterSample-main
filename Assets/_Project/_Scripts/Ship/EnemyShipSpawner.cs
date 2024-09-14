using UnityEngine;

public class EnemyShipSpawner : MonoBehaviour, IIntervalSpawner
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform[] _spawnPoints;
    public Transform[] SpawnPoints => _spawnPoints;

    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _interval;
    public float Interval => _interval;

    [Header("Prefab")]
    [SerializeField] private GameObject _enemyShipPrefab; // Single reference for the enemy ship prefab

    // Implementing the interface property
    public GameObject SpawnObjectPrefab => _enemyShipPrefab;

    private float _elapsedTime;

    [SerializeField] private Transform _planetTransform;

    private void Update()
    {
        if (_elapsedTime < Interval)
        {
            _elapsedTime += Time.deltaTime;
        }
        else
        {
            SpawnShips(_planetTransform);
            _elapsedTime = 0f;
        }
    }

    private void SpawnShips(Transform planetTransform)
    {
        // Instantiate the enemy ship prefab
        var instantiatedObject = Instantiate(SpawnObjectPrefab,
            (Vector2)SpawnPoints[Random.Range(0, SpawnPoints.Length)].position + Random.insideUnitCircle * _spawnRadius,
            Quaternion.identity);

        // Cast to Ship and initialize if possible
        var ship = instantiatedObject.GetComponent<Ship>();
        if (ship != null)
        {
            // Initialize the ship if it has an Init method
            (ship as EnemyShip)?.Init(planetTransform);
        }
    }
}
