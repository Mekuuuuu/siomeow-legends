using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankingScreen : NetworkBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject rankingEntryPrefab;
    [SerializeField] private Transform rankingPanel;
    [SerializeField] private GameObject rankingScreenUI;
    [SerializeField] private string mainMenuScene;
    private NetworkList<GameplayState> players;

    private void Awake()
    {
        players = new NetworkList<GameplayState>();
    }
    public override void OnNetworkSpawn()
    {
        if (IsHost)
        {
            foreach (var client in HostManager.Instance.ClientData)
            {
                ulong clientId = client.Key;
                ClientData clientData = client.Value;

                var gameplayState = new GameplayState(
                    clientId,
                    new FixedString64Bytes(clientData.playerName),
                    clientData.characterId,
                    clientData.KillCount
                );

                players.Add(gameplayState);
                
            }
            Debug.Log("Here are the connected players from GameplayState:");
            foreach (GameplayState player in players)
            {
                Debug.Log($"ClientId: {player.ClientId}, PlayerName: {player.PlayerName.ToString()}, CharacterId: {player.CharacterId} with KillCount {player.KillCount}");
            }
        }

        if (IsServer)
        {
            DisplayRankingClientRpc();
        }
    }

    [ClientRpc]
    private void DisplayRankingClientRpc()
    {
        // Clear the previous ranking entries
        foreach (Transform child in rankingPanel)
        {
            Destroy(child.gameObject);
        }

        // Convert NetworkList to a regular List
        List<GameplayState> sortedPlayers = new List<GameplayState>();

        foreach (var player in players)
        {
            sortedPlayers.Add(player);
        }

        // Sort the players by kill count in descending order
        sortedPlayers.Sort((player1, player2) => player2.KillCount.CompareTo(player1.KillCount));

        // Instantiate the UI for each player in the sorted list
        foreach (var player in sortedPlayers)
        {
            GameObject entry = Instantiate(rankingEntryPrefab, rankingPanel);
            RankingEntry rankingEntry = entry.GetComponent<RankingEntry>();

            rankingEntry.SetPlayerName(player.PlayerName.ToString());
            rankingEntry.SetPlayerPortrait(player.CharacterId); // You could map this to actual portraits
            rankingEntry.SetKills(player.KillCount);
        }
    }

    public void LeaveLobby()
    {
        if (IsHost)
        {
            HostManager.Instance.StopHost();
        }
        else
        {
            ClientManager.Instance.StopClient();
        }
        NetworkManager.Singleton.ConnectionApprovalCallback = null;
        ReturnToMainMenu();
    }

    public void ReturnToMainMenu()
    {   
        AudioManager.instance.PlayButtonClick();
        SceneManager.LoadScene(mainMenuScene);
    }
}

