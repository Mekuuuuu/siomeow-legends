using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameCountdown : NetworkBehaviour
{
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private float blinkThreshold = 30f; // Time threshold for blink (in seconds)
    [SerializeField] private Color blinkColor = Color.red; // Color for blink warning
    [SerializeField] private Color defaultColor = Color.white; // Default color for the text
    [SerializeField] private GameObject timesUp;

    private float lastBlinkTime = 0f; // Timer for blink effect
    private bool isBlinking = false; // To toggle the blinking effect

    public void UpdateCountdownUI(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        countdownText.text = $"{minutes}:{seconds:00}";

        // If the time reaches the blink threshold, start blinking
        if (time <= blinkThreshold)
        {
            HandleBlinking(time);
        }
        else
        {
            // Reset to default color if time exceeds threshold
            ResetBlinking();
        }
    }

    private void HandleBlinking(float time)
    {
        if (!isBlinking)
        {
            isBlinking = true;
            lastBlinkTime = Time.time; // Start blinking
        }

        // Toggle blink effect every half second
        if (Time.time - lastBlinkTime >= 0.5f)
        {
            lastBlinkTime = Time.time;

            // Toggle the color of the text (blink effect)
            countdownText.color = countdownText.color == blinkColor ? defaultColor : blinkColor;
        }
    }

    private void ResetBlinking()
    {
        if (isBlinking)
        {
            isBlinking = false;
            countdownText.color = defaultColor; // Ensure the text is in default color when no longer blinking
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void OnCountdownFinishedServerRpc()
    {
        Debug.Log("Countdown finished on server.");
        timesUp.SetActive(true); // Set active on the server
        OnCountdownFinishedClientRpc();
    }

    [ClientRpc]
    public void OnCountdownFinishedClientRpc()
    {
        Debug.Log("Countdown finished on client.");
        if (timesUp != null)
        {
            timesUp.SetActive(true); // Set active on the client
        }
    }
}