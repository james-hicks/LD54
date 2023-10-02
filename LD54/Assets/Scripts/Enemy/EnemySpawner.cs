using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int _maxEnemies = 1;
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private Transform[] _spawnPoints;


    private List<GameObject> _enemies = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if(_enemies.Count < _maxEnemies)
        {
            GameObject Enemy = Instantiate(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)], _spawnPoints[Random.Range(0, _spawnPoints.Length)]);
            Enemy.GetComponent<EnemyMovement>().UnitDeath += RemoveEnemy;
            _enemies.Add(Enemy);
        }
    }

    private void RemoveEnemy(GameObject Enemy)
    {
        _enemies.Remove(Enemy);
    }
}
