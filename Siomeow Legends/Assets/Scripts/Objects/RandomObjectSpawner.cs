using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] ItemPrefabs; 
    public float Radius = 5; 
    public int MaxObjects = 10; 
    public float SpawnInterval = 5.0f; 
    public int ObjectsPerSpawn = 1; 
    public float CheckRadius = 0.5f; // Radius to check for obstacles
    public LayerMask ObstacleLayer; // Layer to detect obstacles like walls

    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        InvokeRepeating("SpawnObjectsAtRandom", 0, SpawnInterval);
    }

    void SpawnObjectsAtRandom()
    {
        // Remove inactive or destroyed objects from the list
        spawnedObjects.RemoveAll(obj => obj == null || !obj.activeInHierarchy);

        int currentCount = spawnedObjects.Count;

        if (currentCount < MaxObjects)
        {
            Vector3 randomPos;
            int safetyCounter = 0;
            int objectsToSpawn = Mathf.Min(ObjectsPerSpawn, MaxObjects - currentCount);

            HashSet<int> chosenIndices = new HashSet<int>();
            while (chosenIndices.Count < objectsToSpawn)
            {
                int randomIndex = Random.Range(0, ItemPrefabs.Length);
                chosenIndices.Add(randomIndex);

                do
                {
                    randomPos = this.transform.position + (Vector3)(Random.insideUnitCircle * Radius);
                    safetyCounter++;
                } 
                while (Physics2D.OverlapCircle(randomPos, CheckRadius, ObstacleLayer) && safetyCounter < 100);

            }

            if (safetyCounter < 100)
            {
                foreach (int index in chosenIndices)
                {
                    randomPos = this.transform.position + (Vector3)(Random.insideUnitCircle * Radius);

                    GameObject spawned = Instantiate(ItemPrefabs[index], randomPos, Quaternion.identity);
                    spawnedObjects.Add(spawned);
                }
            }
            else
            {
                Debug.LogWarning("Could not find a valid spawn position!");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 center = this.transform.position;
        Vector3 size = new Vector3(Radius * 2, Radius * 2, 0);
        Gizmos.DrawWireCube(center, size);
    }
}
