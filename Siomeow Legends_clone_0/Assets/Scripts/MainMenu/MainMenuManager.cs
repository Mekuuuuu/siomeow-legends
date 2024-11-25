using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject HomeScreen;
    [SerializeField] GameObject StartPopUp;
    [SerializeField] GameObject LANMenu;
    [SerializeField] GameObject MultiplayerMenu;
    [SerializeField] GameObject SettingsMenu;
    [SerializeField] GameObject QuitMenu;
    
    void Awake()
    {
        HomeScreen.SetActive(true);
        StartPopUp.SetActive(false);
        LANMenu.SetActive(false);
        MultiplayerMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        QuitMenu.SetActive(false);
    }
}
