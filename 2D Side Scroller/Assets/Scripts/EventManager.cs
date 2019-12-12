using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    public event Action OnPlayerKilled;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerKilled()
    {
        // Invoke the player killed action if it has methods subscribed to it
        OnPlayerKilled?.Invoke();
        PlatformManager.current.ResetPlatforms();
    }
}
