using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxConnections = 4;
    [SerializeField] private string characterSelectScene = "GameLobby";
    [SerializeField] private string basicDungeonScene = "BasicDungeon";
    [SerializeField] private string forestMapScene = "ForestMap";
    [SerializeField] private string gameplayScene = "Gameplay";
    [SerializeField] private string lavaMapScene = "LavaMap";


    public static HostManager Instance { get; private set; }

    private bool gameHasStarted;
    private string lobbyId;
    public Dictionary<ulong, ClientData> ClientData { get; private set; }
    public string JoinCode { get; private set; }

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

    public async void StartHost()
    {
        if (!MainMenuManager.Instance.isLAN)
        {
            Debug.Log("Multiplayer Process");

            Allocation allocation;
            // Use Relay, not our own router
            try
            {
                allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
            }
            catch (Exception e)
            {
                Debug.LogError($"Relay create allocation request failed {e.Message}");
                throw;
            }
            
            Debug.Log($"server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
            Debug.Log($"server: {allocation.AllocationId}");

            // Get Join Code from Relay allocation
            try
            {
                JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            }
            catch
            {
                Debug.LogError("Relay get join code request failed");
                throw;
            }

            var relayServerData = new RelayServerData(allocation, "dtls"); // dtls is 'security protocol'(?)
            
            // Set everything up for us to connect to the Relay server
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            try
            {
                var createLobbyOptions = new CreateLobbyOptions();
                createLobbyOptions.IsPrivate = false; // MIKO CHECK
                createLobbyOptions.Data = new Dictionary<string, DataObject>()
                {
                    {
                        "JoinCode", new DataObject(
                            visibility: DataObject.VisibilityOptions.Member, // sets that player has to be in lobby to see the join code
                            value: JoinCode
                        )
                    }
                };

                // ping API to create a lobby
                Lobby lobby = await Lobbies.Instance.CreateLobbyAsync("My Lobby", maxConnections, createLobbyOptions);
                lobbyId = lobby.Id;
                StartCoroutine(HeartbeatLobbyCoroutine(15));
            }
            catch (LobbyServiceException e)
            {
                Debug.Log("Start Host Error: " + e);
                throw;
            }
        }
        else
        {
            Debug.Log("LAN Process");

            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port = 7777; // Ensure port is same
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    JoinCode = ip.ToString();
                    break;
                }
            }
            if (JoinCode == null)
            {
                throw new Exception("No network adapters with an IPv4 address in the system.");
            }

        }

        Debug.Log("General Network Process (Host)");

        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.OnServerStarted += OnNetworkReady;

        ClientData = new Dictionary<ulong, ClientData>(); // Avoid lingering data from previous instance

        NetworkManager.Singleton.StartHost();
    }

    public void StopHost()
    {
        NetworkManager.Singleton.Shutdown();
    }

    // need to ping the service regularly so that it wont be erased by Lobby service
    private IEnumerator HeartbeatLobbyCoroutine(float waitTimeSeconds)
    {
        var delay = new WaitForSeconds(waitTimeSeconds);
        while(true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }
    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        Debug.Log($"Approval request received from Client ID: {request.ClientNetworkId}");
        Debug.Log($"Payload length: {request.Payload.Length}");
        Debug.Log($"Raw payload: {BitConverter.ToString(request.Payload)}");

        // reject if room is full or game has started 
        if (ClientData.Count >= 4 || gameHasStarted)
        {
            response.Approved = false;
            return;
        }
        response.Approved = true;
        response.CreatePlayerObject = false; // dont want to automatically happen. want to manually spawnm
        response.Pending = false;

        string playerName;
        if (request.ClientNetworkId == 0) // Host
        {
            playerName = MainMenuManager.Instance.PlayerName ?? "Host";
            Debug.Log($"Host connected. Setting player name: {playerName}");
        }
        else // Clients
        {
            Debug.Log($"Payload received on host: {BitConverter.ToString(request.Payload)}");
            playerName = System.Text.Encoding.UTF8.GetString(request.Payload);
            Debug.Log($"Here is player name: {playerName}");
        }
        
        ClientData[request.ClientNetworkId] = new ClientData(request.ClientNetworkId, playerName);
        Debug.Log("Current clients:");
        foreach (var client in ClientData)
        {
            ulong clientId = client.Key;
            ClientData data = client.Value;

            Debug.Log($"Client ID: {clientId}, Player Name: {data.playerName}");
        }

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

        System.Random random = new System.Random();
        int randomNumber = random.Next(1, 4); // Generates a number between 1 (inclusive) and 4 (exclusive)

        switch (randomNumber)
        {
            case 1:
            NetworkManager.Singleton.SceneManager.LoadScene(basicDungeonScene, LoadSceneMode.Single);
            break;

            case 2:
            NetworkManager.Singleton.SceneManager.LoadScene(forestMapScene, LoadSceneMode.Single);
            break;

            case 3:
            NetworkManager.Singleton.SceneManager.LoadScene(lavaMapScene, LoadSceneMode.Single);
            break;
        }
        // NetworkManager.Singleton.SceneManager.LoadScene(forestMapScene, LoadSceneMode.Single);
        // NetworkManager.Singleton.SceneManager.LoadScene(gameplayScene, LoadSceneMode.Single);
    }

    /**********************************************************************

    NOT COMPLETE. NEED TO HANDLE CONNECTION AND DISCONNECTION

    ***********************************************************************/


}
