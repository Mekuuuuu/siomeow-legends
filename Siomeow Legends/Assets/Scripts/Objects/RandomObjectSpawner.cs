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
    private List<Vector2> validPositions = new List<Vector2>();

    public int MaxObjects = 10;
    public float SpawnInterval = 5.0f;
    public int ObjectsPerSpawn = 1;

    void Start()
    {
        // Pre-generate valid positions
        GenerateValidPositions();
        InvokeRepeating("SpawnObjectsAtRandom", 0, SpawnInterval);
    }

    void GenerateValidPositions()
    {
        validPositions.Clear();  // Clear the previous valid positions

        for (float x = -width / 2; x <= width / 2; x += MinSpawnDistance)
        {
            for (float y = -height / 2; y <= height / 2; y += MinSpawnDistance)
            {
                Vector2 position = new Vector2(x, y) + spawnCenter;
                if (IsValidPosition(position))
                {
                    validPositions.Add(position);
                }
            }
        }

        Debug.Log($"Generated {validPositions.Count} valid positions.");
    }

    bool IsValidPosition(Vector2 position)
    {
        if (IsPositionTooCloseToOthers(position)) return false;

        if (IsOverlappingWithObstacles(position)) return false;

        return true;
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
        Vector2 boxSize = new Vector2(1f, 1f); 
        foreach (LayerMask layer in ObstacleLayer)
        {
            if (Physics2D.OverlapBox(position, boxSize, 0f, layer))
            {
                return true;
            }
        }
        return false;
    }

    void SpawnObjectsAtRandom()
    {
        spawnedObjects.RemoveAll(obj => obj == null || !obj.activeInHierarchy);

        int currentCount = spawnedObjects.Count;
        if (currentCount < MaxObjects)
        {
            int objectsToSpawn = Mathf.Min(ObjectsPerSpawn, MaxObjects - currentCount);

            for (int i = 0; i < objectsToSpawn; i++)
            {
                if (validPositions.Count == 0)
                {
                    Debug.LogWarning("No more valid positions available!");
                    break;
                }

                int index = Random.Range(0, validPositions.Count);
                Vector2 spawnPos = validPositions[index];
                GameObject spawned = Instantiate(ItemPrefabs[Random.Range(0, ItemPrefabs.Length)], spawnPos, Quaternion.identity);
                spawnedObjects.Add(spawned);

                validPositions.RemoveAt(index);
            }
        }
    }

    // Draw the spawn area in the editor for visual debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector2 size = new Vector2(width, height);
        Gizmos.DrawWireCube(spawnCenter, size);
    }
}
