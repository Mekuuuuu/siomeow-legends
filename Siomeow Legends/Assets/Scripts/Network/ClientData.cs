using System;
using UnityEngine;

[Serializable]
public class ClientData
{
    public ulong clientId;
    public string playerName; // Add this property to store the player's name
    public int characterId = -1;

    public ClientData(ulong clientId, string playerName)
    {
        this.clientId = clientId;
        this.playerName = playerName; // Initialize player name
    }
}
