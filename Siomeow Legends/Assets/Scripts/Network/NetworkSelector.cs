// using System;
// using System.Collections;
// using TMPro;
// using UnityEngine;

// public class NetworkSelector : MonoBehaviour
// {
//     [SerializeField] private TMP_InputField nameInputField;
//     [SerializeField] private GameObject StartPopUp;
//     [SerializeField] private GameObject LANMenu;
//     [SerializeField] private GameObject MultiplayerMenu;

//     public static NetworkSelector Instance { get; private set; }

//     public bool isLAN { get; private set; }

//     public string PlayerName { get; private set; }

//     private void Awake()
//     {
//         if (Instance != null && Instance != this)
//         {
//             Destroy(gameObject); // Prevent duplicates
//             return;
//         }

//         Instance = this;
//         DontDestroyOnLoad(gameObject); // Optional: Persist across scenes
//     }
    
//     public void setLAN()
//     {
//         if (string.IsNullOrWhiteSpace(nameInputField.text))
//         {
//             Debug.Log("Invalid Name");
//             // StartCoroutine(FlashInvalidInput());
//         }
//         else
//         {
//             PlayerName = nameInputField.text;
//             Debug.Log("Player name: " + PlayerName);
//             isLAN = true;
//             LANMenu.SetActive(true);
//             StartPopUp.SetActive(false);
//         }
//     }

//     public void setMultiplayer()
//     {
//         if (string.IsNullOrWhiteSpace(nameInputField.text))
//         {
//             Debug.Log("Invalid Name");
//             // StartCoroutine(FlashInvalidInput());
//         }
//         else
//         {
//             PlayerName = nameInputField.text;
//             Debug.Log("Player name: " + PlayerName);
//             isLAN = false;
//             MultiplayerMenu.SetActive(true);
//             StartPopUp.SetActive(false);
//         }
//     }

//     private IEnumerator FlashInvalidInput()
//     {
//         // Save the original text color
//         Color originalColor = nameInputField.placeholder.color;
//         string originalPlaceholderText = nameInputField.placeholder.GetComponent<TMP_Text>().text;

//         // Change the placeholder text to "Invalid Name" and set color to red
//         TMP_Text placeholderText = nameInputField.placeholder.GetComponent<TMP_Text>();
//         placeholderText.text = "Invalid Name";
//         placeholderText.color = Color.red;

//         // Wait for 1 second
//         yield return new WaitForSeconds(1f);

//         // Revert the placeholder text and color back to original
//         placeholderText.text = originalPlaceholderText;
//         placeholderText.color = originalColor;;
//     }
// }
