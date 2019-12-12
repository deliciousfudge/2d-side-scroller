using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float movementSpeed = 5.0f;

    private Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        EventManager.current.OnPlayerKilled += ResetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(Time.deltaTime, 0.0f, 0.0f) * movementSpeed;
    }

    void ResetPosition()
    {
        transform.position = startingPos;
    }
}
