using Unity.Netcode;
using UnityEngine;

public class MultiplayerMenuDisplay : MonoBehaviour
{

    public void StartHost()
    {
        // Debug.Log(ServerManager.Instance == null ? "ServerManager is null" : "ServerManager is not null");
        ServerManager.Instance.StartHost();
    }

    public void StartServer()
    {
        ServerManager.Instance.StartServer();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
