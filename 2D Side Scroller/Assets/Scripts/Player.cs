using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Properties
    public bool IsDead { set; get; } = false;
    public float FallMultiplier { set; get; } = 1.2f;

    // Fields
    public float jumpForce = 7.5f;
    public AudioClip sfxJumpSound;
    public AudioClip sfxLandingSound;

    private bool isJumping = false;
    private bool isFalling = false;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rBody;
    private AudioSource sfxAudioPlayer;

    void OnEnable()
    {
        GameManager.current.OnPlayerKilled += Die;
        GameManager.current.OnPlayerRespawned += Respawn;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rBody = GetComponent<Rigidbody2D>();
        sfxAudioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is alive
        if (!IsDead)
        {
            // If the player is falling
            if (rBody.velocity.y < 0.5f)
            {
                if (!isFalling)
                {
                    isFalling = true;
                }

                if (isJumping)
                {
                    // Increase their fall speed to make the jump feel snappier
                    rBody.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1);
                }
            }
            //else if (Mathf.Abs(rBody.velocity.y) < 0.05f && isFalling)
            //{
            //    isFalling = false;
            //    sfxAudioPlayer.PlayOneShot(sfxLandingSound);
            //}

            // If the player is on the ground
            if (!isJumping && !isFalling)
            {
                // If the player falls behind the center of the screen, steadily accelerate until they return there
                if (transform.position.x < 0.0f)
                {
                    transform.position += new Vector3((0.0f - transform.position.x) * 0.01f, 0.0f, 0.0f);
                }
            }
        }

        // If the player isn't in the middle of a jump and clicks the left mouse button
        if (Input.GetMouseButtonDown(0) && isJumping == false)
        {
            isJumping = true;
            Jump();
        }
    }

    public void Respawn()
    {
        // 
        transform.position = Vector3.zero;
        isJumping = false;
        spriteRenderer.enabled = true;
        IsDead = false;
    }

    public void Die()
    {
        IsDead = true;
        spriteRenderer.enabled = false;
    }

    public void Jump()
    {
        rBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        sfxAudioPlayer.PlayOneShot(sfxJumpSound);
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.gameObject.tag == "Platform")
        {
            if (isJumping && isFalling)
            {
                isJumping = false;
                isFalling = false;
                sfxAudioPlayer.PlayOneShot(sfxLandingSound);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.tag == "Coin")
        {
            _other.gameObject.GetComponent<AudioSource>().Play();

            _other.gameObject.GetComponent<Coin>().SetInactiveDelayed(0.2f);

            GameManager.current.UpdateCoinCount();
        }
    }
}
