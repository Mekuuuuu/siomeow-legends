using System;
using UnityEngine;

public class NetworkSelector : MonoBehaviour
{
    public static NetworkSelector Instance { get; private set; }

    public bool isLAN { get; private set; }

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
        isLAN = true;
    }

    public void setMultiplayer()
    {
        isLAN = false;
    }
}
