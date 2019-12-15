using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverScreen : MonoBehaviour
{
    /// <summary>
    /// Updates gameplay logic any time a new frame is displayed to the screen
    /// </summary>
    void Update()
    {
        // If the player taps the screen, respawn the player and end the gameover sequence
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.current.PlayerRespawned();
        }
    }
}
