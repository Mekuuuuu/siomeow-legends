using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{

    [Header("General")]
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private GameLobbyDisplay gameLobbyDisplay;
    [SerializeField] private GameObject lockInButton;
    [SerializeField] private TMP_Text pickingLabel;
    [SerializeField] private GameObject visuals;
    [SerializeField] private TMP_Text playerNameText;

    [Header("Background")]
    [SerializeField] private GameObject playerBackground;
    [SerializeField] private GameObject playerLockedInBackground;

    // [SerializeField] private TMP_Text characterNameText; // REMOVE THIS SINCE WE WILL NOT USE IT. ctrl+f for instances of this.

    [Header("Meow Knight")]
    [SerializeField] private GameObject meowKnightIcon;
    [SerializeField] private GameObject meowKnightLockedInIcon;

    [Header("Meow Wizard")]
    [SerializeField] private GameObject meowWizardIcon;
    [SerializeField] private GameObject meowWizardLockedInIcon;

    [Header("Meow King")]
    [SerializeField] private GameObject meowKingIcon;
    [SerializeField] private GameObject meowKingLockedInIcon;
    
    [Header("Meow Rogue")]
    [SerializeField] private GameObject meowRogueIcon;
    [SerializeField] private GameObject meowRogueLockedInIcon;

    // private Coroutine pickingAnimationCoroutine;

    public void UpdateDisplay(GameLobbyState state)
    {

        // Show the lock-in button only if this is the local player's card
        lockInButton.SetActive(state.ClientId == NetworkManager.Singleton.LocalClientId && !state.IsLockedIn);

        // pickingLabel.SetActive(!(state.ClientId == NetworkManager.Singleton.LocalClientId) && !state.IsLockedIn);

        if (!(state.ClientId == NetworkManager.Singleton.LocalClientId) && !state.IsLockedIn)
        {
            pickingLabel.gameObject.SetActive(true);
            StartCoroutine(AnimatePickingLabel());
        }
        else
        {
            pickingLabel.gameObject.SetActive(false);
            StopCoroutine(AnimatePickingLabel());
        }

        if (state.CharacterId != -1)
        {
            var character = characterDatabase.GetCharacterById(state.CharacterId);
            Debug.Log(character);

            ShowSelected(state);
            ClearUnselected(state);
           
        }

        // Make separate text for picking in card when player is not locked in / disappear when locked in
        playerNameText.text = state.IsLockedIn ? $"Player {state.ClientId}" : $"Player {state.ClientId} (Picking...)";

        visuals.SetActive(true);
    }

    public void DisableDisplay()
    {
        visuals.SetActive(false);

    }

    public void LockIn()
    {
        gameLobbyDisplay.LockIn();
    }

    private void ClearUnselected(GameLobbyState state)
    {
        meowKnightIcon.SetActive(state.CharacterId == 1);
        meowWizardIcon.SetActive(state.CharacterId == 2);
        meowKingIcon.SetActive(state.CharacterId == 3);
        meowRogueIcon.SetActive(state.CharacterId == 4);
    }

    private void ShowSelected(GameLobbyState state)
    {

        switch (state.CharacterId)
        {
            case 1:
                meowKnightIcon.SetActive(!state.IsLockedIn);
                meowKnightLockedInIcon.SetActive(state.IsLockedIn);
                break;
            case 2:
                meowWizardIcon.SetActive(!state.IsLockedIn);
                meowWizardLockedInIcon.SetActive(state.IsLockedIn);
                break;
            case 3:
                meowKingIcon.SetActive(!state.IsLockedIn);
                meowKingLockedInIcon.SetActive(state.IsLockedIn);
                break;
            case 4:
                meowRogueIcon.SetActive(!state.IsLockedIn);
                meowRogueLockedInIcon.SetActive(state.IsLockedIn);
                break;
        }

        playerBackground.SetActive(!state.IsLockedIn);
        playerLockedInBackground.SetActive(state.IsLockedIn);
    }

    private IEnumerator AnimatePickingLabel()
    {
        // Start the Picking label animation
        List<string> pickingSequence = new List<string> { "Picking", "Picking.", "Picking..", "Picking..." };
        TextAnimator.StartAnimation(this, pickingLabel, pickingSequence, 0.5f);
        yield return null;
    }
}
