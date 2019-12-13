using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSegment : MonoBehaviour
{
    public Transform segmentStart;
    public Transform segmentEnd;
    public GameObject[] coins;
    public GameObject[] obstacles;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateCoins()
    {
        foreach(GameObject coin in coins)
        {
            coin.SetActive(true);
        }
    }
}
