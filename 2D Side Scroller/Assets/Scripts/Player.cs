using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Properties
    public float DistanceRun { set; get; } = 0;
    public bool IsDead { set; get; } = false;
    public float JumpForce { set; get; } = 6.0f;
    public float FallMultiplier { set; get; } = 1.2f;

    private bool isInAir = false;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rBody;

    void OnEnable()
    {
        EventManager.current.OnPlayerKilled += Die;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDead)
        {
            //DistanceRun += Mathf.RoundToInt(Time.deltaTime * 20.0f);
            //print("Distance Run: " + DistanceRun);

            if (isInAir)
            {
                // If the player is falling
                if (rBody.velocity.y < 0)
                {
                    // Increase their fall speed to make the jump feel snappier
                    rBody.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1);
                }
            }
            else
            {
                // If the player falls behind, steadily increase their speed to get them back to the center of the screen
                if (transform.position.x < 0.0f)
                {
                    transform.position += new Vector3((0.0f - transform.position.x) * 0.01f, 0.0f, 0.0f);
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && isInAir == false)
        {
            isInAir = true;
            Jump();
        }
    }

    public void Respawn()
    {
        DistanceRun = 0;
        transform.position = Vector3.zero;
        isInAir = false;
    }

    public void Die()
    {
        print("Dead as a dodo");
        //IsDead = true;
        //spriteRenderer.enabled = false;
        Respawn();
    }

    public void Jump()
    {
        rBody.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.gameObject.tag == "Platform")
        {
            if (isInAir)
            {
                isInAir = false;
            }
        }
    }
}
