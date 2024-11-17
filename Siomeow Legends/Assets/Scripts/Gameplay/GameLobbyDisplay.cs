using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class GameLobbyDisplay : NetworkBehaviour
{
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private Transform charactersHolder;
    [SerializeField] private CharacterSelectButton selectButtonPrefab;
    [SerializeField] private PlayerCard[] playerCards;
    [SerializeField] private GameObject characterInfoPanel; // REMOVE THIS SINCE WE WILL NOT USE IT. ctrl+f for instances of this.
    [SerializeField] private TMP_Text characterNameText; // REMOVE THIS SINCE WE WILL NOT USE IT. ctrl+f for instances of this.
    [SerializeField] private Button lockInButton;

    private List<CharacterSelectButton> characterButtons = new List<CharacterSelectButton>();
    private NetworkList<GameLobbyState> players;

    private void Awake()
    {
        players = new NetworkList<GameLobbyState>();
    }

    public override void OnNetworkSpawn()
    {
        if(IsClient)
        {
            Character[] allCharacters = characterDatabase.GetAllCharacters();

            foreach(var character in allCharacters)
            {
                var selectbuttonInstance = Instantiate(selectButtonPrefab, charactersHolder);
                selectbuttonInstance.SetCharacter(this, character);
                characterButtons.Add(selectbuttonInstance);
            }

            players.OnListChanged += HandlePlayersStateChanged;
        }

        if(IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;


            // Makes sure anyone who is connectred is added
            foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                HandleClientConnected(client.ClientId);
            }
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            players.OnListChanged += HandlePlayersStateChanged;
        }

        if(IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;


        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        players.Add(new GameLobbyState(clientId));
    }

    private void HandleClientDisconnected(ulong clientId)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId == clientId)
            {
                players.RemoveAt(i);
                break;
            }
        }
    }

    public void Select(Character character)
    {
        for(int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId != NetworkManager.Singleton.LocalClientId) { continue; } // looking for user clientId

            if (players[i].IsLockedIn) { return;}                                      // cant change character is already locked in

            if (players[i].CharacterId == character.Id) { return; }                     // current character selected already selected

            if (IsCharacterTaken(character.Id, false)) { return; }                      // false => client only, true => server-wide
        }

        characterNameText.text = character.DisplayName;
        characterInfoPanel.SetActive(true);
        SelectServerRpc(character.Id);

    }

    [ServerRpc(RequireOwnership = false)]
    private void SelectServerRpc(int characterId, ServerRpcParams serverRpcParams = default)
    {   
        for(int i = 0; i < players.Count; i++)
        {
            // ignore all clients that is not ourselves / user
            if (players[i].ClientId != serverRpcParams.Receive.SenderClientId) { continue; }

            // cancel if character is not valid
            if (!characterDatabase.IsValidCharacterId(characterId)) { return; }

            // make sure this character is not locked in (true => regardless of who the user is; host or client)
            if (IsCharacterTaken(characterId, true)) { return; }

            players[i] = new GameLobbyState(
                players[i].ClientId,
                characterId,
                players[i].IsLockedIn
            );
        }
    }

    public void LockIn()
    {
        LockInServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void LockInServerRpc(ServerRpcParams serverRpcParams = default)
    {   
        for(int i = 0; i < players.Count; i++)
        {
            // ignore all clients that is not ourselves / user
            if (players[i].ClientId != serverRpcParams.Receive.SenderClientId) { continue; }

            // cancel if character is not valid
            if (!characterDatabase.IsValidCharacterId(players[i].CharacterId)) { return; }

            // make sure this character is not locked in (true => regardless of who the user is; host or client)
            if (IsCharacterTaken(players[i].CharacterId, true)) { return; }

            players[i] = new GameLobbyState(
                players[i].ClientId,
                players[i].CharacterId,
                true
            );
        }

        foreach (var player in players)
        {
            if (!player.IsLockedIn) { return; }

        }
        foreach(var player in players)
        {
            ServerManager.Instance.SetCharacter(player.ClientId, player.CharacterId);        
        }

        // MIKO REMINDER. ATTACH THIS TO A BUTTON THAT ONLY THE HOST CAN SEE
        ServerManager.Instance.StartGame();
    }

    private void HandlePlayersStateChanged(NetworkListEvent<GameLobbyState> changeEvent)
    {
        for (int i = 0; i < playerCards.Length; i++)
        {
            if (players.Count > i)
            {
                playerCards[i].UpdateDisplay(players[i]);
            }
            else
            {
                playerCards[i].DisableDisplay();
            }
        }

        foreach(var button in characterButtons)
        {
            if (button.IsDisabled) { continue; }

            if (IsCharacterTaken(button.Character.Id, false)) 
            { 
                button.SetDisabled();
            }

        }

        foreach(var player in players)
        {
            if (player.ClientId != NetworkManager.Singleton.LocalClientId) { continue; }

            if (player.IsLockedIn)
            {
                lockInButton.interactable = false;
                break;
            }

            if (IsCharacterTaken(player.CharacterId, false))
            {
                lockInButton.interactable = false;
                break;
            }

            lockInButton.interactable = true;
            break;
        }
    }

    private bool IsCharacterTaken(int characterId, bool checkAll)
    {
        for (int i = 0; i < players.Count; i++)
        {
            // for the disabledOverlay. we dont want to cross out the character we locked in in our screen, only other players
            if (!checkAll)
            {
                if (players[i].ClientId == NetworkManager.Singleton.LocalClientId) { continue; }
            }
            // other player has locked in the selected character
            if (players[i].IsLockedIn && players[i].CharacterId == characterId)
            {
                return true;
            }
        }

        return false;
    }
}
