using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsScreen : MonoBehaviour
{
    /// <summary>
    /// Updates gameplay logic any time a new frame is displayed to the screen
    /// </summary>
    void Update()
    {
        // If the player taps the screen
        if (Input.GetMouseButtonDown(0))
        {
            // Launch into the game
            SceneManager.LoadScene("Game");
        }
    }
}
