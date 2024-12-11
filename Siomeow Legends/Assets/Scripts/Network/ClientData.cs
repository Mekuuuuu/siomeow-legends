using System;
using UnityEngine;

[Serializable]
public class ClientData
{
    public ulong clientId;
    public string playerName; // Add this property to store the player's name
    public int characterId = -1;

    private int _killCount;
    public int KillCount
    {
        get => _killCount;
        set
        {
            if (_killCount != value)
            {
                _killCount = value;
                OnKillCountChanged?.Invoke(clientId, _killCount);
            }
        }
    }

    public static event Action<ulong, int> OnKillCountChanged;

    public ClientData(ulong clientId, string playerName)
    {
        this.clientId = clientId;
        this.playerName = playerName; // Initialize player name
    }

    public void IncrementKillCount()
    {
        KillCount++;
        Debug.Log($"Client {playerName} now has {KillCount} kills.");
    }
}
