using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject disabledOverlay;
    [SerializeField] private Button button;

    private GameLobbyDisplay characterSelect;
    public Character Character {get; private set;} 
    public bool IsDisabled {get; private set;}

    public void SetCharacter(GameLobbyDisplay characterSelect, Character character)
    {
        iconImage.sprite = character.Icon;
        this.characterSelect = characterSelect;
        this.Character = character;
    }

    public void SelectCharacter()
    {
        characterSelect.Select(Character);
    }

    public void SetDisabled()
    {
        IsDisabled = true;
        iconImage.sprite = Character.DisabledIcon; 
        disabledOverlay.SetActive(true);
        button.interactable = false;
    }

    public void SetPressed()
    {
        if (!IsDisabled)
        {
            iconImage.sprite = Character.PressedIcon;
        }
    }

    public void ResetToNormal()
    {
        if (!IsDisabled)
        {
            iconImage.sprite = Character.Icon; // Reset to normal icon
        }
    }
}
