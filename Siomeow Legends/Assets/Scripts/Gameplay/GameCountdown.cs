using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameCountdown : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private float remainingTime;

    private float minutes;
    private float seconds;

    private Color defaultColor;
    private Color warningColor;
    
    private float blinkInterval;
    private bool isBlinking = false;

     public event Action OnCountdownFinished; // Event to notify when the countdown finishes

    void Start()
    {
        defaultColor = countdownText.color;
        warningColor = Color.red;
        blinkInterval = 0.5f;
    }



    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            minutes = Mathf.FloorToInt(remainingTime / 60);
            seconds = Mathf.FloorToInt(remainingTime % 60);
            countdownText.text = string.Format("{0}:{1:00}", minutes, seconds);

            if (remainingTime <= 30 && !isBlinking)
            {
                StartCoroutine(BlinkWarning());
            }
        }
        else
        {
            remainingTime = 0;
            countdownText.text = "0:00";

            if (OnCountdownFinished != null)
            {
                OnCountdownFinished.Invoke(); // Notify listeners the countdown is finished
                Debug.Log("Countdown Finished");
            }

            enabled = false; // Disable further updates
        }
    }

    private IEnumerator BlinkWarning()
    {
        isBlinking = true;

        while (remainingTime > 0 && remainingTime <= 30)
        {
            countdownText.color = countdownText.color == defaultColor ? warningColor : defaultColor;
            yield return new WaitForSeconds(blinkInterval);
        }

        countdownText.color = defaultColor;
        isBlinking = false;
    }
}
