using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnPrefabs : MonoBehaviour
{
    public List<GameObject> prefabsToSpawn;
    public int numObjectsToSpawn;
    public float spawnRange;

    // Start is called before the first frame update
    public void Start()
    {
        for (int i = 0; i < numObjectsToSpawn; i++)
        {
            SpawnObject();
        }
    }

    public void SpawnObject() // 
    {
        GameObject prefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Count)];
        Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-spawnRange, spawnRange), 0f, Random.Range(-spawnRange, spawnRange));
        Quaternion spawnRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        GameObject spawnedObject = Instantiate(prefab, spawnPosition, spawnRotation, transform);
        spawnedObject.name = prefab.name + " (Clone)";
    }

    // Update is called once per frame
    public void Update()
    {

        
    }
}
