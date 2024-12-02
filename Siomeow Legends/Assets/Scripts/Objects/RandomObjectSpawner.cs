using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] private float width = 5f;
    [SerializeField] private float height = 2f;
    [SerializeField] private float MinSpawnDistance = 1.0f;
    [SerializeField] private Vector2 spawnCenter = Vector2.zero;

    public GameObject[] ItemPrefabs;
    public LayerMask[] ObstacleLayer;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    public int MaxObjects = 10;
    public float SpawnInterval = 5.0f;
    public int ObjectsPerSpawn = 1;

    void Start()
    {
        InvokeRepeating("SpawnObjectsAtRandom", 0, SpawnInterval);
    }

    void SpawnObjectsAtRandom()
    {
        spawnedObjects.RemoveAll(obj => obj == null || !obj.activeInHierarchy);

        int currentCount = spawnedObjects.Count;
        if (currentCount < MaxObjects)
        {
            int objectsToSpawn = Mathf.Min(ObjectsPerSpawn, MaxObjects - currentCount);
            HashSet<int> chosenIndices = new HashSet<int>();
            List<Vector2> validPositions = new List<Vector2>();

            GenerateValidPositions(validPositions);

            int safetyCounter = 0;
            while (objectsToSpawn > 0 && safetyCounter < 100)
            {
                Vector2 spawnPos = validPositions[Random.Range(0, validPositions.Count)];
                if (!IsPositionTooCloseToOthers(spawnPos) && !IsOverlappingWithObstacles(spawnPos))
                {
                    GameObject spawned = Instantiate(ItemPrefabs[Random.Range(0, ItemPrefabs.Length)], spawnPos, Quaternion.identity);
                    spawnedObjects.Add(spawned);
                    objectsToSpawn--;
                }
                safetyCounter++;
            }

            if (safetyCounter >= 100)
            {
                Debug.LogWarning("Could not find a valid spawn position!");
            }
        }
    }

    private void GenerateValidPositions(List<Vector2> validPositions)
    {
        for (float x = -width / 2; x <= width / 2; x += MinSpawnDistance)
        {
            for (float y = -height / 2; y <= height / 2; y += MinSpawnDistance)
            {
                validPositions.Add(new Vector2(x, y) + spawnCenter);
            }
        }
    }

    private bool IsPositionTooCloseToOthers(Vector2 position)
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null && Vector2.Distance(obj.transform.position, position) < MinSpawnDistance)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsOverlappingWithObstacles(Vector2 position)
    {
        foreach (LayerMask obstacleLayer in ObstacleLayer)
        {
            if (Physics2D.OverlapBox(position, new Vector2(width / 10f, height / 10f), 0f, obstacleLayer))
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 size = new Vector2(width, height);
        Gizmos.DrawWireCube(spawnCenter, size);
    }
}
