using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayDisplay : NetworkBehaviour
{
    [SerializeField] private Camera fallbackCamera;
    [SerializeField] private GameObject hostDisconnectedPanel;
    [SerializeField] private string mainMenuScene;



    private NetworkList<GameplayState> players;

    private void Awake()
    {
        players = new NetworkList<GameplayState>();
    }

    public override void OnNetworkSpawn()
    {
        if(IsClient)
        {
            players.OnListChanged += HandlePlayersStateChanged;
        }
        
        if (IsHost)
        {
            foreach (var client in HostManager.Instance.ClientData)
            {
                ulong clientId = client.Key;
                ClientData clientData = client.Value;

                var gameplayState = new GameplayState(
                clientId,
                new FixedString64Bytes(clientData.playerName),
                clientData.characterId
                );

                players.Add(gameplayState);
                
            }
            Debug.Log("Here are the connected players from GameplayState:");
            foreach (GameplayState player in players)
            {
                Debug.Log($"ClientId: {player.ClientId}, PlayerName: {player.PlayerName.ToString()}, CharacterId: {player.CharacterId}");
            }
        }

        // Could change this to IsHost but using this if we decide to use dedicated servers in the future
        if(IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;


            // Makes sure anyone who is connectred is added
            foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                HandleClientConnected(client.ClientId);
            }
        }    
    }


    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            players.OnListChanged -= HandlePlayersStateChanged; // prev += was working fine but seems wrong so changed to -=
        }

        if (!IsHost)
        {
            fallbackCamera.gameObject.SetActive(true);
            hostDisconnectedPanel.SetActive(true);

        }

        if(IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        Debug.Log($"Client connected: {clientId}");
    }
    private void HandleClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client disconnected: {clientId}");
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId == clientId)
            {
                players.RemoveAt(i);
                // playerCards[i].ClearPlayerCard();
                break;
            }
        }
    }
    private void HandlePlayersStateChanged(NetworkListEvent<GameplayState> changeEvent)
    {
        Debug.Log("Player State Changed");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

}
