using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class TextAnimator
{
    private static Dictionary<TMP_Text, Coroutine> activeAnimations = new Dictionary<TMP_Text, Coroutine>();

    public static void StartAnimation(MonoBehaviour host, TMP_Text textComponent, List<string> textSequence, float animationSpeed)
    {
        if (textComponent == null || host == null) return;

        // Stop any existing animation for this text component
        StopAnimation(host, textComponent);

        // Start a new animation
        Coroutine animationCoroutine = host.StartCoroutine(AnimateTextCoroutine(host, textComponent, textSequence, animationSpeed));
        activeAnimations[textComponent] = animationCoroutine;
    }


    public static void StopAnimation(MonoBehaviour host, TMP_Text textComponent)
    {
        if (textComponent == null || !activeAnimations.ContainsKey(textComponent)) return;

        if (activeAnimations[textComponent] != null)
        {
            host.StopCoroutine(activeAnimations[textComponent]);
        }

        activeAnimations.Remove(textComponent);
    }

    private static IEnumerator AnimateTextCoroutine(MonoBehaviour host, TMP_Text textComponent, List<string> textSequence, float animationSpeed)
    {
        int index = 0;

        while (true)
        {
            textComponent.text = textSequence[index];
            index = (index + 1) % textSequence.Count;
            yield return new WaitForSeconds(animationSpeed);
        }
    }
}