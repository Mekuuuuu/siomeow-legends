using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string characterSelectScene = "GameLobby";
    [SerializeField] private string gameplaySceneName = "Gameplay";

    public static ServerManager Instance { get; private set; }

    private bool gameHasStarted;
    public Dictionary<ulong, ClientData> ClientData { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.OnServerStarted += OnNetworkReady;

        ClientData = new Dictionary<ulong, ClientData>(); // Avoid lingering data from previous instance

        NetworkManager.Singleton.StartHost();
    }

    public void StartServer()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.OnServerStarted += OnNetworkReady;

        ClientData = new Dictionary<ulong, ClientData>(); // Avoid lingering data from previous instance

        NetworkManager.Singleton.StartServer();
    }

    // Manage if player can join the room / server
    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        // reject if room is full or game has started 
        if (ClientData.Count >= 3 || gameHasStarted)
        {
            response.Approved = false;
            return;
        }
        response.Approved = true;
        response.CreatePlayerObject = false; // dont want to automatically happen. want to manually spawnm
        response.Pending = false;

        ClientData[request.ClientNetworkId] = new ClientData(request.ClientNetworkId);
        Debug.Log($"Added client {request.ClientNetworkId}");
    }

    private void OnNetworkReady()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        NetworkManager.Singleton.SceneManager.LoadScene(characterSelectScene, LoadSceneMode.Single);
    }

    private void OnClientDisconnect(ulong clientId)
    {
        if (ClientData.ContainsKey(clientId))
        {
            if (ClientData.Remove(clientId))
            {
                Debug.Log($"Removed client {clientId}");
            }
        }
    }

    public void SetCharacter(ulong clientId, int characterId)
    {
        if (ClientData.TryGetValue(clientId, out ClientData data))
        {
            data.characterId = characterId;
        }
    }

    public void StartGame()
    {
        gameHasStarted = true;

        NetworkManager.Singleton.SceneManager.LoadScene(gameplaySceneName, LoadSceneMode.Single);
    }


}
