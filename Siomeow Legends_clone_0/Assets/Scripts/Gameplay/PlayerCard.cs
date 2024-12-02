using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private CharacterDatabase characterDatabase;
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
    

    public void UpdateDisplay(GameLobbyState state)
    {
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

    private void ClearUnselected(GameLobbyState state)
    {
        if (state.CharacterId == 1)
        {
            meowWizardIcon.SetActive(false);
            meowKingIcon.SetActive(false);
            meowRogueIcon.SetActive(false);
        }
        else if (state.CharacterId == 2)
        {
            meowKnightIcon.SetActive(false);
            meowKingIcon.SetActive(false);
            meowRogueIcon.SetActive(false);
        }
        else if (state.CharacterId == 3)
        {
            meowKnightIcon.SetActive(false);
            meowWizardIcon.SetActive(false);
            meowRogueIcon.SetActive(false);
        }
        else if (state.CharacterId == 4)
        {
            meowKnightIcon.SetActive(false);
            meowWizardIcon.SetActive(false);
            meowKingIcon.SetActive(false);
        }
    }

    private void ShowSelected(GameLobbyState state)
    {
        // Meow Knight
        if (state.CharacterId == 1) 
        {
            if (state.IsLockedIn)
            {
                meowKnightLockedInIcon.SetActive(true);
                playerLockedInBackground.SetActive(true);
            }
            else
            {
                meowKnightIcon.SetActive(true);
                playerBackground.SetActive(true);
            }
        }

        // Meow Wizard
        else if (state.CharacterId == 2) 
        {
            if (state.IsLockedIn)
            {
                meowWizardLockedInIcon.SetActive(true);
                playerLockedInBackground.SetActive(true);
            }
            else
            {
                meowWizardIcon.SetActive(true);
                playerBackground.SetActive(true);
            }
        }

        // Meow King
        else if (state.CharacterId == 3) 
        {
            if (state.IsLockedIn)
            {
                meowKingLockedInIcon.SetActive(true);
                playerLockedInBackground.SetActive(true);
            }
            else
            {
                meowKingIcon.SetActive(true);
                playerBackground.SetActive(true);
            }
        }

        // Meow Rogue
        else if (state.CharacterId == 4) 
        {
            if (state.IsLockedIn)
            {
                meowRogueLockedInIcon.SetActive(true);
                playerLockedInBackground.SetActive(true);
            }
            else
            {
                meowRogueIcon.SetActive(true);
                playerBackground.SetActive(true);
            }
        }

        // Invalid Character
        else
        {
            // HANDLEEEEEE
        }
    }
}
