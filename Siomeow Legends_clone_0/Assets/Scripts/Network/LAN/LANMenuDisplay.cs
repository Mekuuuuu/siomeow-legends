using Unity.Netcode;
using UnityEngine;

public class LANMenuDisplay : MonoBehaviour
{

    public void StartHost()
    {
        HostManager.Instance.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
