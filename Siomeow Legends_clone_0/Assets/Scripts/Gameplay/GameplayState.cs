using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct GameplayState : INetworkSerializable, IEquatable<GameplayState>
{
    public ulong ClientId;
    public FixedString64Bytes PlayerName;
    public int CharacterId;

    public int KillCount;

    public GameplayState(ulong clientId, FixedString64Bytes playerName, int characterId = -1, int killCount = 0)
    {
        this.ClientId = clientId;
        this.PlayerName = playerName;
        this.CharacterId = characterId;
        this.KillCount = killCount;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref PlayerName);
        serializer.SerializeValue(ref CharacterId); 
        serializer.SerializeValue(ref KillCount);  

    }

    public bool Equals(GameplayState other)
    {
        return ClientId == other.ClientId && 
            PlayerName == other.PlayerName &&
            CharacterId == other.CharacterId;
    }

}
