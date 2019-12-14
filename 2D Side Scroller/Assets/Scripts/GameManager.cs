using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Properties
    public int CoinsCollected { set; get; } = 0;

    // Fields
    public static GameManager current;
    public GameObject player;
    public GameObject uiInstructions;
    public GameObject uiGameover;
    public AudioClip musicClipGameplay;
    public AudioClip musicClipGameover;
    public Text uiCoinsCollectedLabel;
    public Text gameoverCoinsCollectedLabel;

    // Components
    private AudioSource musicAudioPlayer;

    // Events
    public event Action OnPlayerKilled;
    public event Action OnPlayerRespawned;

    private void Awake()
    {
        // Create a single accessible instance of the class (singleton pattern)
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Make sure that the gameover screen does not display by accident
        uiGameover.SetActive(false);

        // Start playing the background music
        musicAudioPlayer = GetComponent<AudioSource>();
        musicAudioPlayer.clip = musicClipGameplay;
        musicAudioPlayer.loop = true;
        musicAudioPlayer.Play();
    }

    public void PlayerKilled()
    {
        // Invoke the player killed action if it has methods subscribed to it
        OnPlayerKilled?.Invoke();

        // Move all platforms off the screen and disable both the platform manager and player to save resources
        PlatformManager.current.gameObject.SetActive(false);
        PlatformManager.current.DisablePlatforms();
 
        player.SetActive(false);

        // Enable the gameover screen
        gameoverCoinsCollectedLabel.text = CoinsCollected.ToString();
        uiGameover.SetActive(true);
        SwitchGameMusic(musicClipGameover);
    }

    public void PlayerRespawned()
    {
        // Invoke the player respawned action if it has methods subscribed to it
        OnPlayerRespawned?.Invoke();

        // Re-enable the platform manager and move the starting segment into position
        PlatformManager.current.EnablePlatforms();
        PlatformManager.current.gameObject.SetActive(true);


        // Re-enable the player and reset the coin count
        player.SetActive(true);
        CoinsCollected = 0;
        uiCoinsCollectedLabel.text = CoinsCollected.ToString();

        // Disable the gameover screen
        uiGameover.SetActive(false);
        SwitchGameMusic(musicClipGameplay);
    }

    public void UpdateCoinCount()
    {
        CoinsCollected += 1;
        uiCoinsCollectedLabel.text = CoinsCollected.ToString();
    }

    private void SwitchGameMusic(AudioClip _newMusicClip)
    {
        if (musicAudioPlayer.clip != _newMusicClip)
        {
            musicAudioPlayer.Stop();
            musicAudioPlayer.clip = _newMusicClip;
            musicAudioPlayer.Play();
        }
    }
}
