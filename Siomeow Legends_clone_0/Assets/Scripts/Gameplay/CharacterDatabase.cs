using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Database", menuName = "Characters/Database")]
public class CharacterDatabase : ScriptableObject
{
    [SerializeField] private Character[] characters = new Character[0];

    // Return all characters
    public Character[] GetAllCharacters() => characters;
    // Return specific character via character Id
    public Character GetCharacterById(int id)
    {
        foreach(var character in characters)
        {
            if(character.Id == id)
            {
                return character;
            }
        }

        return null;
    }

    // Check if input id matches with any of character id
    public bool IsValidCharacterId(int id)
    {
        return characters.Any(x => x.Id == id);
    }
}
