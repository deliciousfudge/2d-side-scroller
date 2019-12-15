using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Properties
    public int CoinsCollected { set; get; } = 0; // The number of coins collected in the current runthrough

    // Fields
    public static GameManager current; // A reference to the single accessible instance of the class

    public GameObject player; // A reference to the player instance
    public GameObject uiGameover; // A reference to the gameover screen
    public AudioClip musicClipGameplay; // The background music played while in a runthrough
    public AudioClip musicClipGameover; // The background music played on the gameover screen
    public Text uiCoinsCollectedLabel; // A reference to the HUD label displaying the number of coins collected
    public Text gameoverCoinsCollectedLabel; // A reference to the gameover screen label displaying the number of coins collected

    // Components
    private AudioSource musicAudioPlayer;

    // Event delegates
    public event Action OnPlayerKilled; // Broadcasts when the player has been killed
    public event Action OnPlayerRespawned; // Broadcasts when the player has been respawned

    /// <summary>
    /// Processes gameplay logic immediately after objects are initialized
    /// </summary>
    private void Awake()
    {
        // Create a single accessible instance of the class (singleton pattern)
        current = this;
    }

    /// <summary>
    /// Processes gameplay logic prior to the first frame being displayed
    /// </summary>
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

    /// <summary>
    /// Processes resulting logic from the player being killed
    /// </summary>
    public void PlayerKilled()
    {
        // Invoke the player killed action if it has methods subscribed to it
        OnPlayerKilled?.Invoke();

        // Move all platforms off the screen and disable both the platform manager and player to save resources
        PlatformManager.current.gameObject.SetActive(false);
        PlatformManager.current.DisablePlatforms();
        player.SetActive(false);

        // Update and enable the gameover screen
        gameoverCoinsCollectedLabel.text = CoinsCollected.ToString();
        uiGameover.SetActive(true);
        SwitchGameMusic(musicClipGameover);
    }

    /// <summary>
    /// Processes resulting logic from the player being respawned
    /// </summary>
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

    /// <summary>
    /// Increments the number of coins collected and updates the coin count displayed in the UI
    /// </summary>
    public void UpdateCoinCount()
    {
        CoinsCollected += 1;
        uiCoinsCollectedLabel.text = CoinsCollected.ToString();
    }

    /// <summary>
    /// Switches from the current music track to a specified music track
    /// </summary>
    /// <param name="_newMusicClip">The new music track to be played</param>
    private void SwitchGameMusic(AudioClip _newMusicClip)
    {
        // If the music track to switch to is not already being played
        if (musicAudioPlayer.clip != _newMusicClip)
        {
            // Swap to the new music track
            musicAudioPlayer.Stop();
            musicAudioPlayer.clip = _newMusicClip;
            musicAudioPlayer.Play();
        }
    }
}
