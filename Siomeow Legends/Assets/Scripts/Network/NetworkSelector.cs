using System;
using TMPro;
using UnityEngine;

public class NetworkSelector : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;

    public static NetworkSelector Instance { get; private set; }

    public bool isLAN { get; private set; }

    public string PlayerName { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: Persist across scenes
    }
    
    public void setLAN()
    {
        if (string.IsNullOrWhiteSpace(nameInputField.text))
        {
            Debug.Log("Invalid Name");
        }
        else
        {
            PlayerName = nameInputField.text;
            Debug.Log("Player name: " + PlayerName);
            isLAN = true;
        }
    }

    public void setMultiplayer()
    {
        if (string.IsNullOrWhiteSpace(nameInputField.text))
        {
            Debug.Log("Invalid Name");
        }
        else
        {
            PlayerName = nameInputField.text;
            Debug.Log("Player name: " + PlayerName);
            isLAN = false;
        }
    }
}
