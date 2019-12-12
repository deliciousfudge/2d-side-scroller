using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Properties
    public float DistanceRun { set; get; } = 0;
    public bool IsDead { set; get; } = false;

    private SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        EventManager.current.OnPlayerKilled += InvokeDeath;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDead)
        {
            DistanceRun += Mathf.RoundToInt(Time.deltaTime * 20.0f);
            print("Distance Run: " + DistanceRun);
        }
    }

    public void InvokeSpawn()
    {
        DistanceRun = 0;
        transform.position = Vector3.zero;
    }

    public void InvokeDeath()
    {
        print("Dead as a dodo");
        //IsDead = true;
        //spriteRenderer.enabled = false;
        InvokeSpawn();
    }
}
