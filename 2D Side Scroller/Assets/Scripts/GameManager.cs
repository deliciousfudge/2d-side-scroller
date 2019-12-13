using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    public event Action OnPlayerKilled;
    public event Action OnPlayerRespawned;

    public GameObject uiInstructions;
    public GameObject uiGameover;
    public AudioClip musicClipGameplay;
    public AudioClip musicClipGameover;
    public Text coinsCollectedLabel;

    private AudioSource musicAudioPlayer;

    public int CoinsCollected { set; get; } = 0;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        uiGameover.SetActive(false);

        musicAudioPlayer = GetComponent<AudioSource>();
        musicAudioPlayer.clip = musicClipGameplay;
        musicAudioPlayer.loop = true;
        musicAudioPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerKilled()
    {
        // Invoke the player killed action if it has methods subscribed to it
        OnPlayerKilled?.Invoke();
        SwitchGameMusic(musicClipGameover);

        PlatformManager.current.DisablePlatforms();

        uiGameover.SetActive(true);
    }

    public void PlayerRespawned()
    {
        OnPlayerRespawned?.Invoke();
        PlatformManager.current.EnablePlatforms();

        CoinsCollected = 0;

        uiGameover.SetActive(false);

        print("Switching to gameplay music");

        SwitchGameMusic(musicClipGameplay);
    }

    public void UpdateCoinCount()
    {
        CoinsCollected += 1;
        coinsCollectedLabel.text = CoinsCollected.ToString();
    }

    private void SwitchGameMusic(AudioClip _newMusicClip)
    {
        musicAudioPlayer.Stop();
        musicAudioPlayer.clip = _newMusicClip;
        musicAudioPlayer.Play();
    }
}
