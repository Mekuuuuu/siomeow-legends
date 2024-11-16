using UnityEngine;
using UnityEngine.UI;


// THIS SCRIPT IS USED FOR CHARACTER SELECTION
// since we want it to be random, so this should be disregarded
public class CharacterSelectButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    private GameLobbyDisplay characterSelect;
    private Character character;

    public void SetCharacter(GameLobbyDisplay characterSelect, Character character)
    {
        iconImage.sprite = character.Icon;
        this.characterSelect = characterSelect;
        this.character = character;
    }

    public void SelectCharacter()
    {
        characterSelect.Select(character);
    }
}
