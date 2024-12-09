using Unity.Netcode;
using UnityEngine;

public class CharacterSpawner : NetworkBehaviour
{
    [SerializeField] private CharacterDatabase characterDatabase;
    public override void OnNetworkSpawn()
    {
        // Only server should run this code
        if (!IsServer) { return; }

        foreach (var client in HostManager.Instance.ClientData)
        {
            var character = characterDatabase.GetCharacterById(client.Value.characterId);
            if (character != null)
            {
                Debug.Log("Name: " + character.DisplayName);
                var spawnPos = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f)); // sample spawn point
                var characterInstance = Instantiate(character.GameplayPrefab, spawnPos, Quaternion.identity); // Te Rai, diri ka mubutang spawn points position
                characterInstance.SpawnAsPlayerObject(client.Value.clientId);

                // Get the PlayerStats component
                var playerStats = characterInstance.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    // Initialize the PlayerUIManager with the PlayerStats component
                    if (NetworkManager.Singleton.LocalClientId == client.Value.clientId)
                    {
                        PlayerUIManager.Instance.Initialize(playerStats);
                    }
                }
                else
                {
                    Debug.LogWarning("PlayerOverlay component not found on spawned character prefab.");
                }
            }
        }
    }
}
