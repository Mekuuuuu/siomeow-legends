using System.Net;
using System.Net.Sockets;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class LANMenuDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_InputField ipAddressInputField;

    public void StartHost()
    {
        HostManager.Instance.StartHost();
        Debug.Log("Start Host for LAN");
    }

    public async void StartClient()
    {
        await ClientManager.Instance.StartClient(ipAddressInputField.text);
        Debug.Log("Start Client for LAN");

    }
}
