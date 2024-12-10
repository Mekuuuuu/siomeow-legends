using System.Collections;
using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject HomeScreen;
    [SerializeField] GameObject StartPopUp;
    [SerializeField] GameObject LANMenu;
    [SerializeField] GameObject MultiplayerMenu;
    [SerializeField] GameObject SettingsMenu;
    [SerializeField] GameObject QuitMenu;

    [SerializeField] private TMP_InputField nameInputField;

    public static MainMenuManager Instance { get; private set; }

    public bool isLAN { get; private set; }
    public string PlayerName { get; private set; }
    
    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;

        HomeScreen.SetActive(true);
        StartPopUp.SetActive(false);
        LANMenu.SetActive(false);
        MultiplayerMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        QuitMenu.SetActive(false);
    }

    public void SetLAN()
    {
        if (string.IsNullOrWhiteSpace(nameInputField.text))
        {
            Debug.Log("Invalid Name");
            StartCoroutine(FlashInvalidInput());
        }
        else
        {
            PlayerName = nameInputField.text;
            Debug.Log("Player name: " + PlayerName);
            isLAN = true;
            LANMenu.SetActive(true);
            StartPopUp.SetActive(false);
        }
    }

    public void SetMultiplayer()
    {
        if (string.IsNullOrWhiteSpace(nameInputField.text))
        {
            Debug.Log("Invalid Name");
            StartCoroutine(FlashInvalidInput());
        }
        else
        {
            PlayerName = nameInputField.text;
            Debug.Log("Player name: " + PlayerName);
            isLAN = false;
            MultiplayerMenu.SetActive(true);
            StartPopUp.SetActive(false);
        }
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    private IEnumerator FlashInvalidInput()
    {
        // Save the original color of the placeholder text
        Color originalColor = nameInputField.placeholder.color;
        string originalPlaceholderText = nameInputField.placeholder.GetComponent<TMP_Text>().text;

        // Change the placeholder text to "Invalid Name" and set color to red
        TMP_Text placeholderText = nameInputField.placeholder.GetComponent<TMP_Text>();
        placeholderText.text = "Invalid Name";
        placeholderText.color = Color.red;

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Revert the placeholder text and color back to original
        placeholderText.text = originalPlaceholderText;
        placeholderText.color = originalColor;
    }
}
