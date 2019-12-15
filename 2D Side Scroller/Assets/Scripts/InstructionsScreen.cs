using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsScreen : MonoBehaviour
{
    public GameObject InstructionsContent;
    public GameObject LoadingLabel;

    private bool isLoadingScene = false; // Used to stop the player from attempting to load the scene more than once

    /// <summary>
    /// Processes gameplay logic immediately after objects are initialized
    /// </summary>
    void Awake()
    {
        // Set the instructions section of the menu to be shown
        InstructionsContent.SetActive(true);
        LoadingLabel.SetActive(false);
    }

    /// <summary>
    /// Updates gameplay logic any time a new frame is displayed to the screen
    /// </summary>
    void Update()
    {
        // If the player taps the screen
        if (Input.GetMouseButtonDown(0) && !isLoadingScene)
        {
            // Prevent the loading attempt from being fired a second time
            isLoadingScene = true;

            // Switch to the loading text
            InstructionsContent.SetActive(false);
            LoadingLabel.SetActive(true);

            // Start loading the game
            StartCoroutine(LoadGameScene());
        }
    }

    IEnumerator LoadGameScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(1);

        while (!async.isDone)
        {
            yield return null;
        }
    }
}
