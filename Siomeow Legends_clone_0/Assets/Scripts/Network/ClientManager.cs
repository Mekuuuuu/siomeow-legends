using System;
using System.Net;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    [SerializeField] private UnityTransport transport;
    public static ClientManager Instance { get; private set; }

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

    public async Task StartClient(string joinCode)
    {
        if (!NetworkSelector.Instance.isLAN)
        {    
            Debug.Log("Multiplayer Process");

            JoinAllocation allocation;
            // Use Relay, not our own router
            try
            {
                allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            }
            catch (Exception e)
            {
                Debug.LogError($"Relay get join code request failed {e.Message}");
                throw;
            }
            
            Debug.Log($"client: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
            Debug.Log($"host: {allocation.HostConnectionData[0]} {allocation.HostConnectionData[1]}");
            Debug.Log($"client: {allocation.AllocationId}");

            var relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);    
        }
        else
        {
            Debug.Log("LAN Process");

            if (IPAddress.TryParse(joinCode, out _))
            {
                transport.ConnectionData.Address = joinCode;
                transport.ConnectionData.Port = 7777; // Ensure the port matches the host's port
                Debug.Log("Client entered the IP Address: " + joinCode);
            }
            else
            {
                Debug.LogError("Invalid IP Address entered: " + joinCode);
                return;
            }
        }

        Debug.Log("General Network Process (Client)");


        try
        {
            string playerName = NetworkSelector.Instance.PlayerName ?? "Player";
            byte[] payload = System.Text.Encoding.UTF8.GetBytes(playerName);
            NetworkManager.Singleton.NetworkConfig.ConnectionData = payload;
            Debug.Log("Before: Here is client name (playerName):" + playerName);
            Debug.Log("Before: Here is client name (payload):" + System.Text.Encoding.UTF8.GetString(payload));
            Debug.Log("Before: Here is client name (ConnectionData):" + System.Text.Encoding.UTF8.GetString(NetworkManager.Singleton.NetworkConfig.ConnectionData));
            NetworkManager.Singleton.StartClient();
            Debug.Log("Client started successfully.");
            Debug.Log("After: Here is client name (playerName):" + playerName);
            Debug.Log("After: Here is client name (payload):" + System.Text.Encoding.UTF8.GetString(payload));
            Debug.Log("After: Here is client name (ConnectionData):" + System.Text.Encoding.UTF8.GetString(NetworkManager.Singleton.NetworkConfig.ConnectionData));
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to start client: {e.Message}");
        }

        // NetworkManager.Singleton.StartClient();
    }

    public void StopClient()
    {
        NetworkManager.Singleton.Shutdown();
    }

}
