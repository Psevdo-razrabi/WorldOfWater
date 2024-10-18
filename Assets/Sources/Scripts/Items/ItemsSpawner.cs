using System.Collections.Generic;
using UnityEngine;

public class ItemsSpawner : MonoBehaviour
{
    [SerializeField] private List<Item> _prefabs;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private float _spawnRange;

    private float _timer;

    private void Update()
    {
        if (_timer <= 0)
        {
            SpawnItem();
            _timer = _spawnInterval;
        }
        else
        {
            _timer -= Time.deltaTime;
        }
    }

    private void SpawnItem()
    {
        var spawnPosition = transform.position + new Vector3(Random.Range(-_spawnRange, _spawnRange), 0, 0);
        var randomItem = _prefabs[Random.Range(0, _prefabs.Count)];
        var item = Instantiate(randomItem, spawnPosition, transform.rotation);
    }
}
