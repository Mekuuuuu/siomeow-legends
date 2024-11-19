using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbiesList : MonoBehaviour
{
    [SerializeField] private Transform lobbyItemParent;
    [SerializeField] private LobbyItem lobbyItemPrefab;


    private bool isRefreshing;
    private bool isJoining;

    private void OnEnable()
    {        
        RefreshList();
    }

    public async void RefreshList()
    {
        if (isRefreshing) { return; }
        
        isRefreshing = true;

        try
        {
            var options = new QueryLobbiesOptions();
            options.Count = 25; // how many lobbies we want to get for pagination

            options.Filters = new List<QueryFilter>()
            {
                // we want any lobbies that have available slots greater than 0
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0"),
                // we want any lobbies that are not locked
                new QueryFilter(
                    field: QueryFilter.FieldOptions.IsLocked,
                    op: QueryFilter.OpOptions.EQ,
                    value: "0")
            };

            var lobbies = await Lobbies.Instance.QueryLobbiesAsync(options); // gives us the lobbies with the specified filter

            // clear already-existing LobbyItem UI to avoid duplicates
            foreach(Transform child in lobbyItemParent)
            {
                Destroy(child.gameObject);
            }

            // spawn the LobbyItem UI
            foreach(Lobby lobby in lobbies.Results)
            {
                var lobbyInstance = Instantiate(lobbyItemPrefab, lobbyItemParent);
                lobbyInstance.Initialise(this, lobby);
            }
        }
        catch(LobbyServiceException e)
        {
            Debug.Log("RefreshList Error " + e);
            isRefreshing = false;
            throw;
        }

        isRefreshing = false;

        Debug.Log("Finished refreshing lobby list");
    }

    public async void JoinAsync(Lobby lobby)
    {
        if (isJoining) { return; }

        isJoining = true;
        try
        {
            var joiningLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);
            string joinCode = joiningLobby.Data["JoinCode"].Value; // we can read data this because we are now a member of a lobby

            await ClientManager.Instance.StartClient(joinCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log("JoinAsync Error " + e);
            isJoining = false;
            throw;
        }

        isJoining = false;
    }
}
