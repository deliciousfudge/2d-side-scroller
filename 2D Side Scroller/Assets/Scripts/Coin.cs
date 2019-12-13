using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetInactive()
    {
        gameObject.SetActive(false);
    }

    public void SetInactiveDelayed(float _timeUntilInactive)
    {
        Invoke("SetInactive", _timeUntilInactive);
    }
}
