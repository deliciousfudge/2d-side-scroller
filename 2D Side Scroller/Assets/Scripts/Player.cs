using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Properties
    public bool IsDead { set; get; } = false;

    // Fields
    public float jumpForce = 8.5f;
    public float fallMultiplier = 1.2f;
    public Vector3 playerSpawnPosition = new Vector3(-8.0f, 0.0f, 0.0f);
    public AudioClip sfxJumpSound;
    public AudioClip sfxLandingSound;
    private bool isJumping = false;
    private bool isFalling = false;
    private float distanceToGround = 0.0f;
    private Vector3 extents;

    // Components
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rBody;
    private AudioSource sfxAudioPlayer;
    private Animator animator;

    void OnEnable()
    {
        // Connect the die and respawn methods to the appropriate event delegates
        GameManager.current.OnPlayerKilled += Die;
        GameManager.current.OnPlayerRespawned += Respawn;
    }

    void Start()
    {
        // Create a reference to any components that will be interacted with later
        spriteRenderer = GetComponent<SpriteRenderer>();
        rBody = GetComponent<Rigidbody2D>();
        sfxAudioPlayer = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        distanceToGround = GetComponent<BoxCollider2D>().bounds.extents.y + 0.2f;
        extents = GetComponent<BoxCollider2D>().bounds.extents * 1.5f;
    }

    void Update()
    {
        if (!IsDead)
        {
            if (!isJumping && !isFalling)
            {
                // If the player touches the screen
                if (Input.GetMouseButtonDown(0))
                {
                    // Play the jump animation and carry out a jump
                    ChangeJumpState(true);
                }
            }
        }
    }
    void FixedUpdate()
    {
        // If the player is alive
        if (!IsDead)
        {
            // If the player is in the middle of a jump
            if (isJumping)
            {
                // If the player is moving downwards
                if (rBody.velocity.y < -0.1f)
                {
                    // Mark the player as falling
                    if (!isFalling)
                    {
                        ChangeFallState(true);
                    }

                    // Increase their fall speed to make the jump feel snappier
                    rBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1);
                }
            }

            // If the player is on the ground
            else if (!isJumping && !isFalling)
            {
                // If the player starts moving downwards
                if (Mathf.Abs(rBody.velocity.y) < -0.1f)
                {
                    // Mark the player as falling
                    if (!isFalling)
                    {
                        ChangeFallState(true);
                    }
                }

                // If the player falls behind the center of the screen
                else if (transform.position.x < -8.0f)
                {
                    // Steadily accelerate until they return there
                    transform.position += new Vector3((-8.0f - transform.position.x) * 0.01f, 0.0f, 0.0f);
                } 
            }
        }
    }

    /// <summary>
    /// Resets the player to their original state after being killed
    /// </summary>
    public void Respawn()
    {
        // Move the player back to the center of the screen and mark them as being on the ground
        transform.position = playerSpawnPosition;
        ChangeJumpState(false);
        ChangeFallState(false);
        spriteRenderer.enabled = true;
        IsDead = false;
    }

    /// <summary>
    /// Processes any required actions after the player has been killed
    /// </summary>
    public void Die()
    {
        IsDead = true;
        spriteRenderer.enabled = false;
    }

    /// <summary>
    /// Invokes the player jump action
    /// </summary>
    public void Jump()
    {
        rBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        sfxAudioPlayer.PlayOneShot(sfxJumpSound);
    }

    /// <summary>
    /// Mark the player as entering or exiting a jump
    /// </summary>
    /// <param name="_isJumping">Whether the player has started a jump (true) or completed one (false)</param>
    private void ChangeJumpState(bool _isJumping)
    {
        animator.SetBool("isJumping", _isJumping);
        isJumping = _isJumping;

        // If the player has started a jump
        if (_isJumping)
        {
            // Invoke the jump action
            Jump();
        }
    }

    /// <summary>
    /// Mark the player as entering or exiting a fall
    /// </summary>
    /// <param name="_isFalling">Whether the player has started falling (true) or has stopped falling (false)</param>
    private void ChangeFallState(bool _isFalling)
    {
        isFalling = _isFalling;
        animator.SetBool("isFalling", _isFalling);
    }

    private bool IsOnGround()
    {
        //return Physics.CheckBox(transform.position, extents);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector3.up, distanceToGround);

        if (hit.collider.tag == "Platform")
        {
            return true;
        }

        return false;
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        // If the player collides with a platform
        if (_collision.gameObject.tag == "Platform")
        {
            // If the player has just touched down after a jump
            if (isFalling)
            {
                ChangeJumpState(false);
                ChangeFallState(false);
            }
        }
        // If the player collides with an obstacle
        else if (_collision.gameObject.tag == "Obstacle")
        {
            // Kill the player
            GameManager.current.PlayerKilled();
        }
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        // If the player has run into a coin
        if (_other.tag == "Coin")
        {
            // Deactivate the coin on a timer so that the coin collection sound can play
            _other.gameObject.GetComponent<AudioSource>().Play();
            _other.gameObject.GetComponent<Coin>().SetInactiveDelayed(0.2f);

            // Update coin collection info
            GameManager.current.UpdateCoinCount();
        }
        // If the player has fallen below the screen bounds
        else if (_other.gameObject.tag == "DeathCollider")
        {
            // Kill the player
            GameManager.current.PlayerKilled();
        }
    }
}
