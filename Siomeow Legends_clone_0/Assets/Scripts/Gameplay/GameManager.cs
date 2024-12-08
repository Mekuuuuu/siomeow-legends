using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameCountdown gameCountdown; // Reference to the GameCountdown script

    void Start()
    {
        // Subscribe to the OnCountdownFinished event
        if (gameCountdown != null)
        {
            gameCountdown.OnCountdownFinished += HandleCountdownFinished;
        }
    }

    private void HandleCountdownFinished()
    {
        // Logic to handle when the countdown reaches zero
        Debug.Log("Countdown finished! Handle game over or next steps here.");
        EndGame();
    }

    private void EndGame()
    {
        // Example end game logic
        Debug.Log("Game Over!");
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        if (gameCountdown != null)
        {
            gameCountdown.OnCountdownFinished -= HandleCountdownFinished;
        }
    }
}
