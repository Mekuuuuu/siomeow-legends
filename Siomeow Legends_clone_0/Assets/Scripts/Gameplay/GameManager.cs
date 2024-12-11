using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private GameCountdown countdown;
    [SerializeField] private float remainingTime = 60f;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            SyncPlayerPortraits();
            StartCountdown();
        }
    }

    private void SyncPlayerPortraits()
    {
        foreach (var client in HostManager.Instance.ClientData)
        {
            ulong clientId = client.Key;
            int characterId = client.Value.characterId;

            if (characterDatabase.IsValidCharacterId(characterId))
            {
                AssignPortraitClientRpc(clientId, characterId);
            }
        }
    }

    [ClientRpc]
    private void AssignPortraitClientRpc(ulong clientId, int characterId)
    {
        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
            Character character = characterDatabase.GetCharacterById(characterId);
            if (character != null)
            {
                PlayerUIManager.Instance.SetPlayerPortrait(character.Portrait);
            }
        }
    }

    private void StartCountdown()
    {
        if (IsServer)
        {
            InvokeRepeating(nameof(UpdateCountdown), 0f, 1f); // Update countdown every second
        }
    }

    private void UpdateCountdown()
    {
        if (remainingTime > 0)
        {
            remainingTime -= 1f;
            UpdateCountdownClientRpc(remainingTime);
        }
        else
        {
            CancelInvoke(nameof(UpdateCountdown));
            StartCoroutine(OnCountdownFinished());
        }
    }

    [ClientRpc]
    private void UpdateCountdownClientRpc(float time)
    {
        countdown.UpdateCountdownUI(time); // Update the UI on all clients
    }

    private IEnumerator OnCountdownFinished()
    {
        countdown.OnCountdownFinishedServerRpc(); // Notify the countdown has finished
        Debug.Log("Countdown finished! Transitioning to next phase.");
        yield return new WaitForSeconds(3f);
        NetworkManager.Singleton.SceneManager.LoadScene("RankingScreen", LoadSceneMode.Single);
    }
}
