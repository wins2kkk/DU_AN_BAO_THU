using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For UI manipulation

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public float jumpForce = 5f; // Jump force
    public float dashDuration = 0.5f; // Dash duration (seconds)
    public float dashSpeedMultiplier = 2f; // Dash speed multiplier
    public float climbSpeed = 3f; // Climb speed
    public Text coinText; // Reference to the UI Text element
    private int coinCount = 0; // Variable to store the number of collected coins
    private bool isGrounded; // Check if the player is on the ground
    private bool isClimbing = false; // Check if the player is climbing a ladder
    private bool nearLadder = false; // Check if the player is near a ladder
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private bool facingRight = true; // Check the current facing direction of the player
    private Animator animator; // Reference to the Animator component
    private bool isAttacking = false; // Check if the player is attacking
    private bool isDashing = false; // Check if the player is dashing

    private Vector3 spawnPoint;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        animator = GetComponent<Animator>(); // Get the Animator component
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing from this game object");
        }
        if (animator == null)
        {
            Debug.LogError("Animator component is missing from this game object");
        }
        if (coinText == null)
        {
            Debug.LogError("Coin Text UI component is missing");
        }
        UpdateCoinText();

        spawnPoint = transform.position;

    }

    void Update()
    {
        if (!isClimbing)
        {
            if (!isAttacking && !isDashing)
            {
                // Move left and right
                float move = Input.GetAxis("Horizontal") * moveSpeed;
                rb.velocity = new Vector2(move, rb.velocity.y);

                // Check and flip the player's direction
                if (move > 0 && !facingRight)
                {
                    Flip();
                }
                else if (move < 0 && facingRight)
                {
                    Flip();
                }

                // Update Speed parameter for Animator
                animator.SetFloat("Speed", Mathf.Abs(move));

                // Jump
                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    animator.SetTrigger("Jump");
                }
            }

            // Dash
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && Mathf.Abs(rb.velocity.x) > 0 && isGrounded)
            {
                StartCoroutine(DashCoroutine());
            }

            // Attack
            if (Input.GetKeyDown(KeyCode.Q) && !isAttacking)
            {
                StartCoroutine(Attack());
            }

            // Climb
            if (Input.GetKeyDown(KeyCode.E) && nearLadder)
            {
                isClimbing = true;
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0; // Disable gravity while climbing
                animator.SetBool("IsClimbing", true);
            }
        }

        if (isClimbing)
        {
            float climb = Input.GetAxis("Vertical") * climbSpeed;
            rb.velocity = new Vector2(0, climb);

            // Stop climbing when the player moves away from the ladder or presses left/right
            if (!nearLadder || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                isClimbing = false;
                rb.gravityScale = 1; // Restore gravity
                animator.SetBool("IsClimbing", false);
            }
        }

        // Update idle and run states
        if (isGrounded && !isClimbing)
        {
            animator.SetBool("isGrounded", true);
            if (rb.velocity.x == 0 && !isAttacking && !isDashing)
            {
                animator.SetBool("Idle", true);
                animator.SetBool("Run", false);
            }
            else
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Run", true);
            }
        }
        else
        {
            animator.SetBool("isGrounded", false);
            animator.SetBool("Run", false);
        }
    }

    void Flip()
    {
        // Flip the player's direction
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            // Update the Animator parameter
            animator.SetBool("isGrounded", true);

            // Check if the player is in the Jump animation state
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                animator.SetBool("Idle", true); // Set to idle animation state
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // When the player is no longer on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            // Update the Animator parameter
            animator.SetBool("isGrounded", false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collided with a coin
        if (other.gameObject.CompareTag("Coin"))
        {
            AddCoin(1);
            Destroy(other.gameObject); // Destroy the coin object after collecting
        }

        // Check if the player is near a ladder
        if (other.gameObject.CompareTag("Ladder"))
        {
            nearLadder = true;
        }
        if (other.gameObject.CompareTag("RedZone")) 
            {
            Respawn();
            } 
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player is leaving the ladder
        if (other.gameObject.CompareTag("Ladder"))
        {
            nearLadder = false;
            isClimbing = false;
            rb.gravityScale = 1; // Restore gravity
            animator.SetBool("IsClimbing", false);
        }
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + coinCount.ToString();
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero; // Stop movement
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // Wait for the attack animation to finish
        isAttacking = false;
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        float originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0; // Disable gravity while dashing

        float dashSpeed = facingRight ? moveSpeed * dashSpeedMultiplier : -moveSpeed * dashSpeedMultiplier;
        rb.velocity = new Vector2(dashSpeed, 0); // Set the dash velocity

        animator.SetBool("IsDashing", true);

        yield return new WaitForSeconds(dashDuration); // Wait for the dash duration

        animator.SetBool("IsDashing", false);
        rb.gravityScale = originalGravityScale; // Restore original gravity
        isDashing = false;
    }

    

    private void Respawn()
    {
        // Di chuyển nhân vật về điểm hồi sinh
        transform.position = spawnPoint;
    }
}
