using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LANMenuDisplay : MonoBehaviour
{

    [SerializeField] private string gameplaySceneName = "LANGameplay";

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        // NetworkManager.Singleton.OnServerStarted += OnNetworkReady;
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(gameplaySceneName, LoadSceneMode.Single);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        // reject if room is full or game has started 
        // if (ClientData.Count >= 3 || gameHasStarted)
        // {
        //     response.Approved = false;
        //     return;
        // }
        response.Approved = true;
        response.CreatePlayerObject = true; // SHOULD BE FALSE!!!! dont want to automatically happen. want to manually spawnm
        response.Pending = false;

        // ClientData[request.ClientNetworkId] = new ClientData(request.ClientNetworkId);
        // Debug.Log($"Added client {request.ClientNetworkId}");
    }
}
