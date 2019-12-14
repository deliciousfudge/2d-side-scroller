using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSegment : MonoBehaviour
{
    // Fields
    public Transform segmentStart;
    public Transform segmentEnd;
    public GameObject[] coins;
    public GameObject[] obstacles;

    private List<GameObject> coinList;

    void Awake()
    {
        coinList = new List<GameObject>();
    }

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

    private int RandomSort(GameObject _A, GameObject _B)
    {
        return Random.Range(-1, 2);
    }
}
