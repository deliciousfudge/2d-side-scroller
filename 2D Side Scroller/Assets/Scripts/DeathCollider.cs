using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        print("Collision affirmative");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        print("Hit death collider");
        EventManager.current.PlayerKilled();
    }
}
