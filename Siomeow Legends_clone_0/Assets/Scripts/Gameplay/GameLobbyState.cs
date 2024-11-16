using System;
using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;

public struct GameLobbyState : INetworkSerializable, IEquatable<GameLobbyState>
{
    public ulong ClientId;
    public int CharacterId;

    public GameLobbyState(ulong clientId, int characterId = -1, bool isReady = false)
    {
        this.ClientId = clientId;
        this.CharacterId = characterId;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref CharacterId);  
    }

    public bool Equals(GameLobbyState other)
    {
        return ClientId == other.ClientId && 
            CharacterId == other.CharacterId;
    }
}    

