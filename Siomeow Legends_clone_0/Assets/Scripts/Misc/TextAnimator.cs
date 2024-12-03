using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text targetText;

    [Header("Animation Settings")]
    [SerializeField] private List<string> textSequence;
    [SerializeField] private float animationSpeed = 0.5f; // Time between text changes

    private Coroutine animationCoroutine;

    private void Start()
    {
        if (targetText == null)
        {
            targetText = GetComponent<TMP_Text>();
        }

        if (textSequence.Count > 0)
        {
            StartAnimation();
        }
    }

    public void StartAnimation()
    {
        if (animationCoroutine == null)
        {
            animationCoroutine = StartCoroutine(AnimateText());
        }
    }

    public void StopAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }

    private IEnumerator AnimateText()
    {
        int index = 0;

        while (true)
        {
            if (textSequence.Count > 0)
            {
                targetText.text = textSequence[index];
                index = (index + 1) % textSequence.Count; // Cycle back to the start
            }

            yield return new WaitForSeconds(animationSpeed);
        }
    }

    public void UpdateTextSequence(List<string> newTextSequence)
    {
        textSequence = newTextSequence;

        if (animationCoroutine != null)
        {
            StopAnimation();
            StartAnimation();
        }
    }
}
