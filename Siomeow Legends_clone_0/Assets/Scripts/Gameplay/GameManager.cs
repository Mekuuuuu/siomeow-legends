using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameCountdown countdown;
    [SerializeField] private float remainingTime = 60f;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            StartCountdown();
        }
    }

    private void StartCountdown()
    {
        if (IsServer)
        {
            InvokeRepeating(nameof(UpdateCountdown), 0f, 1f); // Update countdown every second
        }
    }

    private void UpdateCountdown()
    {
        if (remainingTime > 0)
        {
            remainingTime -= 1f;
            UpdateCountdownClientRpc(remainingTime);
        }
        else
        {
            CancelInvoke(nameof(UpdateCountdown));
            OnCountdownFinished();
        }
    }

    [ClientRpc]
    private void UpdateCountdownClientRpc(float time)
    {
        countdown.UpdateCountdownUI(time); // Update the UI on all clients
    }

    private void OnCountdownFinished()
    {
        countdown.OnCountdownFinished(); // Notify the countdown has finished
        Debug.Log("Countdown finished! Transitioning to next phase.");
    }
}
