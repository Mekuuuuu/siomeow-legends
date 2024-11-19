using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject HomeScreen;
    [SerializeField] GameObject MultiplayerMenu;
    [SerializeField] GameObject SettingsMenu;
    [SerializeField] GameObject QuitMenu;
    
    void Awake()
    {
        HomeScreen.SetActive(true);
        MultiplayerMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        QuitMenu.SetActive(false);
    }
}
