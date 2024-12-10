using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterSpawner : NetworkBehaviour
{
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private float width = 5f;
    [SerializeField] private float height = 2f;
    [SerializeField] private float MinSpawnDistance = 1.0f;
    [SerializeField] private float InitialSpawnDelay = 0f;
    [SerializeField] private Vector2 spawnCenter = Vector2.zero;

    [SerializeField] private LayerMask[] ObstacleLayer;

    private List<Vector2> validPositions = new List<Vector2>();
    public override void OnNetworkSpawn()
    {
        // Only server should run this code
        if (!IsServer) { return; }

        GenerateValidPositions();
        SpawnAllCharacters();
    }

    private void SpawnAllCharacters()
    {
        foreach (var client in HostManager.Instance.ClientData)
        {
            SpawnCharacterForClient(client.Key, client.Value.characterId);
        }
    }

    public void SpawnCharacterForClient(ulong clientId, int characterId)
    {
        var character = characterDatabase.GetCharacterById(characterId);
        if (character != null)
        {
            Vector2 spawnPos;
            if (TryGetValidSpawnPosition(out spawnPos))
            {
                var characterInstance = Instantiate(character.GameplayPrefab, spawnPos, Quaternion.identity);
                var networkObject = characterInstance.GetComponent<NetworkObject>();

                if (networkObject != null)
                {
                    networkObject.SpawnAsPlayerObject(clientId);
                    Debug.Log($"Spawned character {character.DisplayName} for client {clientId} at {spawnPos}");
                }
                else
                {
                    Debug.LogError($"Prefab for character {character.DisplayName} does not have a NetworkObject component.");
                }
            }
            else
            {
                Debug.LogWarning($"No valid spawn positions available for client {clientId}.");
            }
        }
        else
        {
            Debug.LogWarning($"Character with ID {characterId} not found in the database.");
        }
    }

    private void GenerateValidPositions()
    {
        validPositions.Clear();

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

    private bool IsValidPosition(Vector2 position)
    {
        // IsOverllapingWithObstacles logic from RandomObjectSpawner
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

    private bool TryGetValidSpawnPosition(out Vector2 spawnPosition)
    {
        if (validPositions.Count > 0)
        {
            int index = Random.Range(0, validPositions.Count);
            spawnPosition = validPositions[index];
            validPositions.RemoveAt(index); // Remove to prevent reuse
            return true;
        }

        spawnPosition = Vector2.zero;
        return false;
    }

    // Draw the spawn area in the editor for visual debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector2 size = new Vector2(width, height);
        Gizmos.DrawWireCube(spawnCenter, size);
    }
}
