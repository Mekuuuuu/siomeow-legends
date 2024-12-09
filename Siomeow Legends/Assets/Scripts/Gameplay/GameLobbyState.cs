using System;
using NUnit.Framework;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct GameLobbyState : INetworkSerializable, IEquatable<GameLobbyState>
{
    public ulong ClientId;
    public FixedString64Bytes PlayerName;
    public int CharacterId;
    public bool IsLockedIn;

    public GameLobbyState(ulong clientId, FixedString64Bytes playerName, int characterId = -1, bool isLockedIn = false)
    {
        this.ClientId = clientId;
        this.PlayerName = playerName;
        this.CharacterId = characterId;
        this.IsLockedIn = isLockedIn;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref PlayerName);
        serializer.SerializeValue(ref CharacterId);  
        serializer.SerializeValue(ref IsLockedIn);
    }

    public bool Equals(GameLobbyState other)
    {
        return ClientId == other.ClientId && 
            PlayerName == other.PlayerName &&
            CharacterId == other.CharacterId &&
            IsLockedIn == other.IsLockedIn;
    }
}    

