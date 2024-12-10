using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [SerializeField] private List<AudioClip> bgmClips = new List<AudioClip>();
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource buttonClick;
    [SerializeField] private AudioSource pickup;
    [SerializeField] private AudioSource damage;
    [SerializeField] private AudioSource death;
    [SerializeField] private AudioSource dash;
    [SerializeField] private AudioSource heal;
    [SerializeField] private AudioSource crate;
    [SerializeField] private AudioSource error;
    [SerializeField] private AudioSource select;
    [SerializeField] private AudioSource lockin;

    [SerializeField] private List<AudioSource> audioSources = new List<AudioSource>();

    private string previousSceneName;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one AudioManager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);   
    }

    private void Start()
    {
        previousSceneName = SceneManager.GetActiveScene().name;
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "MainMenu")
        {
            StopPlayingClips();
            bgm.clip = bgmClips[0];
            bgm.Play();
        }

        // // testing purposes
        // else if (sceneName == "City 3")
        // {
        //     StopPlayingClips();
        //     bgm.clip = bgmClips[3];
        //     bgm.Play();
        // }
    }

    private void Update()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        // if (currentSceneName == "CharacterCustomization") return;
        if (currentSceneName != previousSceneName)
        {
            // Debug.Log("Scene has changed from " + previousSceneName + " to " + currentSceneName);
            previousSceneName = currentSceneName;
            StopPlayingClips();
            // if (currentSceneName == "CharacterCustomization")
            // {
            //     bgm.clip = bgmClips[6];
            //     bgm.Play();
            // }
            // else if (currentSceneName == "TestMenuSave")
            // {
            //     bgm.clip = bgmClips[0];
            //     bgm.Play();
            // }

            // else if (currentSceneName == "NewIntroScene")
            // {
            //     Debug.Log(true);
            // }

        }
    }

    public void StopPlayingClips()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource != null) audioSource.Stop();
        }
    }

    public void PlayButtonClick()
    {
        buttonClick.Play();
    }

    public void PlayDamage()
    {
        pickup.Play();
    }

    public void PlayPickup()
    {
        pickup.Play();
    }

    public void PlayDeath()
    {
        death.Play();
    }

    public void PlayDash()
    {
        dash.Play();
    }

    public void PlayError()
    {
        error.Play();
    }

    public void PlaySelect()
    {
        select.Play();
    }

    public void PlayLockin()
    {
        lockin.Play();
    }

    
    public void PlayHeal()
    {
        heal.Play();
    }
}