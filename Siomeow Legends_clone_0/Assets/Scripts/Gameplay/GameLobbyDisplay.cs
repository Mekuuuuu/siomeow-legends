using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameLobbyDisplay : NetworkBehaviour
{
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private Transform charactersHolder;
    [SerializeField] private CharacterSelectButton selectButtonPrefab;
    [SerializeField] private PlayerCard[] playerCards;
    [SerializeField] private TMP_Text joinCodeText;
    [SerializeField] private Button StartGameButton;
    [SerializeField] private TMP_Text LobbyStatusText;
    [SerializeField] private GameObject hostDisconnectedPanel;
    [SerializeField] private GameObject hostConfirmLeavePanel;
    [SerializeField] private GameObject clientConfirmLeavePanel;
    [SerializeField] private string mainMenuScene;

    private List<CharacterSelectButton> characterButtons = new List<CharacterSelectButton>();
    private NetworkList<GameLobbyState> players;
    private string joinCode;
    private bool isAllLockedIn = false;

    private void Awake()
    {
        players = new NetworkList<GameLobbyState>();
        StartGameButton.interactable = false;
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
        
        if (IsHost)
        {
            joinCode = HostManager.Instance.JoinCode;
            StartGameButton.gameObject.SetActive(true);
            StartGameButton.interactable = false;
        }

        // Could change this to IsHost but using this if we decide to use dedicated servers in the future
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
            players.OnListChanged -= HandlePlayersStateChanged; // prev += was working fine but seems wrong so changed to -=
        }

        if (!IsHost)
        {
            hostDisconnectedPanel.SetActive(true);
        }

        if(IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        string playerName = HostManager.Instance.ClientData[clientId].playerName;
        Debug.Log("Game Lobby Display: Client Connected: " + playerName);
        players.Add(new GameLobbyState(clientId, playerName));
        SetJoinCodeClientRpc(joinCode);
    }

    private void HandleClientDisconnected(ulong clientId)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId == clientId)
            {
                players.RemoveAt(i);
                playerCards[i].ClearPlayerCard();
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
                players[i].PlayerName,
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
                players[i].PlayerName,
                players[i].CharacterId,
                true
            );
        }

        // Will not start until every player is locked in
        foreach (var player in players)
        {
            if (!player.IsLockedIn) { return; }

        }
        foreach(var player in players)
        {
            HostManager.Instance.SetCharacter(player.ClientId, player.CharacterId);        
        }
    }

    private void HandlePlayersStateChanged(NetworkListEvent<GameLobbyState> changeEvent)
    {

        // Handles connected players in lobby. Shows connected player's info. Hides player card if no connected player
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
            if (IsCharacterTaken(button.Character.Id, false)) 
            { 
                button.SetDisabled();
            }
            else
            {
                // If the character is not taken, check if the button is disabled
                if (button.IsDisabled)
                {
                    button.ForceResetToNormal(); // Re-enable the button if the character is available
                }
            }

        }

        isAllLockedIn = true;
        foreach(var player in players)
        {
            if (!player.IsLockedIn)
            {
                isAllLockedIn = false;
                break;
            }
            
        }
        if (IsHost)
        {
            StartGameButton.interactable = isAllLockedIn && players.Count > 1;
            Debug.Log($"Start Game Button {(isAllLockedIn ? "Enabled" : "Disabled")}");
        }

        UpdateLobbyStatusClientRpc(isAllLockedIn);
        
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

    public void StartGame()
    {
        AudioManager.instance.PlayButtonClick();
        StopAllCoroutines();
        HostManager.Instance.StartGame();
    }

    [ClientRpc]
    private void SetJoinCodeClientRpc(string joinCode)
    {
        joinCodeText.text = joinCode;
    }

    [ClientRpc]
    private void UpdateLobbyStatusClientRpc(bool isAllLockedIn)
    {
        if (IsHost) return; // Skip for the host

        LobbyStatusText.text = "Waiting for host...";
        LobbyStatusText.gameObject.SetActive(true);

        List<string> sequence = isAllLockedIn 
        ? new List<string> { "Waiting for host", "Waiting for host.", "Waiting for host..", "Waiting for host..." } 
        : new List<string> { "Players picking", "Players picking.", "Players picking..", "Players picking..." };

        StartCoroutine(AnimateLobbyStatusText(sequence));
    }

    private IEnumerator AnimateLobbyStatusText(List<string> sequence)
    {
        TextAnimator.StartAnimation(this, LobbyStatusText, sequence, 0.5f);
        yield return null;
    }

    public void Back()
    {
        AudioManager.instance.PlayButtonClick();
        if (IsHost) hostConfirmLeavePanel.SetActive(true);
        else clientConfirmLeavePanel.SetActive(true);
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
