using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    /// <summary>
    /// Makes the coin gameobject inactive
    /// </summary>
    void SetInactive()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Makes the coin gameobject inactive after a given period of time
    /// </summary>
    /// <param name="_timeUntilInactive">The time to wait before making the gameobject inactive, in seconds</param>
    public void SetInactiveDelayed(float _timeUntilInactive)
    {
        Invoke("SetInactive", _timeUntilInactive);
    }
}
