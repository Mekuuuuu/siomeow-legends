using System;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class MultiplayerMenuDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject connectingPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject lobbiesList;
    [SerializeField] private TMP_InputField joinCodeInputField;

    private async void Awake()
    {
        try
        {
            // Ensure Unity Services are initialized
            if (!UnityServices.State.Equals(ServicesInitializationState.Initialized))
            {
                await UnityServices.InitializeAsync();
            }

            // Check if the player is already signed in
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Player Id: {AuthenticationService.Instance.PlayerId}");
            }
            else
            {
                Debug.Log("Player is already signed in.");
            }


            // await UnityServices.InitializeAsync();
            // await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"Player Id: {AuthenticationService.Instance.PlayerId}");
        }
        catch (Exception e)
        {
            Debug.Log("UnityServices Initialize or AuthenticationService Error " + e);
            Debug.LogError(e);
            return;
        }

        connectingPanel.SetActive(false);
        menuPanel.SetActive(true);
        lobbiesList.SetActive(true);

    }

    public void StartHost()
    {
        HostManager.Instance.StartHost();
    }

    public async void StartClient()
    {
        await ClientManager.Instance.StartClient(joinCodeInputField.text);
    }
}
