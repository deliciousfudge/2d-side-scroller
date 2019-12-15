using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSegment : MonoBehaviour
{
    // Fields
    public Transform segmentStart; // The left bound location of the segment
    public Transform segmentEnd; // The right bound location of the segment
    public GameObject[] coins; // An array of coin instances displayed in the level
    public GameObject[] obstacles; // An array of obstacle instances displayed in the level

    private List<GameObject> coinList;

    /// <summary>
    /// Processes gameplay logic immediately after objects are initialized
    /// </summary>
    void Awake()
    {
        coinList = new List<GameObject>();
    }

    /// <summary>
    /// Picks a random number of coins to be displayed on the segment
    /// </summary>
    public void GenerateCoins()
    {
        // Randomly shuffle the array of coins
        System.Array.Sort(coins, RandomSort);

        // Pick a random number of coins to display
        int coinsToReveal = Random.Range(0, coins.Length);
        for (int i = 0; i < coinsToReveal; ++i)
        {
            coins[i].SetActive(true);
        }

        // Hide any remaining coins
        for (int i = coinsToReveal; i < coins.Length; ++i)
        {
            coins[i].SetActive(false);
        }
    }

    /// <summary>
    /// Picks a random number of obstacles to be displayed on the segment
    /// </summary>
    public void GenerateObstacles()
    {
        // Randomly shuffle the array of coins
        System.Array.Sort(obstacles, RandomSort);

        // Pick a random number of coins to display
        int obstaclesToReveal = Random.Range(0, obstacles.Length);
        for (int i = 0; i < obstaclesToReveal; ++i)
        {
            obstacles[i].SetActive(true);
        }

        // Hide any remaining coins
        for (int i = obstaclesToReveal; i < obstacles.Length; ++i)
        {
            obstacles[i].SetActive(false);
        }
    }

    /// <summary>
    /// Method used to pick one of two GameObjects at random. Used in conjunction with the coins and obstacles arrays to randomly shuffle their contents.
    /// </summary>
    /// <param name="_A">The first GameObject instance to choose from</param>
    /// <param name="_B">The second GameObject instance to choose from</param>
    /// <returns>The index (either 0 or 1) of the GameObject to select</returns>
    private int RandomSort(GameObject _A, GameObject _B)
    {
        return Random.Range(-1, 2);
    }
}
