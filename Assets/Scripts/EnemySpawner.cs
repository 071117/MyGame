using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Настройки спавна")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float minX = -2f;
    [SerializeField] private float maxX = 2f;
    [SerializeField] private float spawnY = 6f;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= spawnInterval)
        {
            SpawnEnemy();
            _timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        Vector2 spawnPos = new Vector2(Random.Range(minX, maxX), spawnY);
        Instantiate(enemyPrefabs[index], spawnPos, Quaternion.identity);
    }
}
