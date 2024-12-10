using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Characters/Character")]
public class Character : ScriptableObject
{
    [SerializeField] private int id = -1;
    [SerializeField] private string displayName = "New Display Name";
    [SerializeField] private Sprite portrait;
    [SerializeField] private Sprite icon;
    [SerializeField] private Sprite pressedIcon;
    [SerializeField] private Sprite disabledIcon;
    [SerializeField] private NetworkObject gameplayPrefab;

    public int Id => id;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public Sprite Portrait => portrait;
    public Sprite PressedIcon => pressedIcon; 
    public Sprite DisabledIcon => disabledIcon;
    public NetworkObject GameplayPrefab => gameplayPrefab;
}
