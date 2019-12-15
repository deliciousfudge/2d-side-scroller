using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Properties
    public bool IsDead { set; get; } = false; // Tracks whether the player has been killed

    // Fields
    public float jumpForce = 8.5f; // The amount of upward impulse applied to the player when starting a jump
    public float jumpDelayMax = 0.2f; // The time (in seconds) allowed to pass between tapping the screen and having the player jump
    public float fallMultiplier = 1.2f; // The gravity factor applied to the player when falling
    public Vector3 playerSpawnPosition = new Vector3(-8.0f, 0.0f, 0.0f); // The initial spawn location of the player
    public AudioClip sfxJumpSound; // The sound played when the player begins a jump

    private bool isJumping = false; // Whether the player is in the middle of a jump
    private bool isFalling = false; // Whether the player is falling downward
    private float timeSinceJumpPressed = 5.0f; // The amount of time (in seconds) since the player tapped the screen to jump

    // Components
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rBody;
    private AudioSource sfxAudioPlayer;
    private Animator animator;

    /// <summary>
    /// Processes gameplay logic when the class gameobject has become enabled and active
    /// </summary>
    void OnEnable()
    {
        // Connect the die and respawn methods to the appropriate event delegates
        GameManager.current.OnPlayerKilled += Die;
        GameManager.current.OnPlayerRespawned += Respawn;
    }

    /// <summary>
    /// Processes gameplay logic prior to the first frame being displayed
    /// </summary>
    void Start()
    {
        // Create a reference to any components that will be interacted with later
        spriteRenderer = GetComponent<SpriteRenderer>();
        rBody = GetComponent<Rigidbody2D>();
        sfxAudioPlayer = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Updates gameplay logic any time a new frame is displayed to the screen
    /// </summary>
    void Update()
    {
        if (!IsDead)
        {
            // If the player is on the ground
            if (!isJumping && !isFalling)
            {
                // If the player touches the screen
                if (timeSinceJumpPressed < jumpDelayMax)
                {
                    // Play the jump animation and carry out a jump
                    ChangeJumpState(true);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                timeSinceJumpPressed = 0.0f;
            }
            else
            {
                timeSinceJumpPressed += Time.deltaTime;
            }


        }
    }

    /// <summary>
    /// Updates gameplay logic on a fixed timestep when in-game physics are being processed
    /// </summary>
    void FixedUpdate()
    {
        if (!IsDead)
        {
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

    /// <summary>
    /// Event that is called when the gameobject enters a collision
    /// </summary>
    /// <param name="_collision">The result of the collision</param>
    private void OnCollisionEnter2D(Collision2D _collision)
    {
        // If the player collides with a platform
        if (_collision.gameObject.tag == "PlatformSegment")
        {
            print("hit the platform");
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

    /// <summary>
    /// Event that is called when the gameobject exits a collision
    /// </summary>
    /// <param name="_collision">The result of the collision</param>
    private void OnCollisionExit2D(Collision2D _collision)
    {
        // If the player leaves a platform
        if (_collision.gameObject.tag == "PlatformSegment")
        {
            // If the player has just touched down after a jump
            if (!isFalling && !isJumping)
            {
                ChangeFallState(true);
            }
        }
    }

    /// <summary>
    /// Event that is called when the gameobject enters a trigger
    /// </summary>
    /// <param name="_other">The collider component of the trigger</param>
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
