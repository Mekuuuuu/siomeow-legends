using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {
    [SerializeField] Image musicOnIcon;
    [SerializeField] Image musicOffIcon;
    private bool muted = false;

    void Start () {
        // Always ensure "muted" is set to 0 if it's not explicitly configured
        if (!PlayerPrefs.HasKey("muted")) {
            PlayerPrefs.SetInt("muted", 0); // Default to not muted (ON)
        }

        Load(); // Load the saved value (or the default if no value is saved)
        UpdateButtonIcon();
        AudioListener.pause = muted;
    }

    public void OnButtonPressed() {
        // Toggle the muted state
        muted = !muted;
        AudioListener.pause = muted;

        Save(); // Save the new state
        UpdateButtonIcon();
    }

    private void UpdateButtonIcon() {
        // Show the correct icon based on the "muted" state
        if (!muted) {
            musicOnIcon.enabled = true;
            musicOffIcon.enabled = false;
        } else {
            musicOnIcon.enabled = false;
            musicOffIcon.enabled = true;
        }
    }

    private void Load() {
        // Load the muted state (default is false if no saved state exists)
        muted = PlayerPrefs.GetInt("muted") == 1;
    }

    private void Save() {
        // Save the muted state
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);
    }
}