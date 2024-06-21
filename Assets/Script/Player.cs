using System.Collections;
using UnityEngine;
using UnityEngine.UI; // For UI manipulation

public class Player : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f; // Movement speed
    [SerializeField] public float jumpForce = 5f; // Jump force
    [SerializeField] public float dashDuration = 0.5f; // Dash duration (seconds)
    [SerializeField] public float dashSpeedMultiplier = 2f; // Dash speed multiplier
    [SerializeField] public float climbSpeed = 3f; // Climb speed
    [SerializeField] public Text coinText; // Reference to the UI Text element
    [SerializeField] public Slider healthSlider; // Reference to the UI Slider for health
    [SerializeField] public GameObject damageTextPrefab; // Prefab for the damage text
    [SerializeField] private int coinCount = 0; // Variable to store the number of collected coins
    [SerializeField] private int health = 100; // Player health
    [SerializeField] private bool isGrounded; // Check if the player is on the ground
    [SerializeField] private bool isClimbing = false; // Check if the player is climbing a ladder
    [SerializeField] private bool nearLadder = false; // Check if the player is near a ladder
    [SerializeField] private Rigidbody2D rb; // Reference to the Rigidbody2D component
    [SerializeField] private bool facingRight = true; // Check the current facing direction of the player
    [SerializeField] private Animator animator; // Reference to the Animator component
    [SerializeField] private bool isAttacking = false; // Check if the player is attacking
    [SerializeField] private bool isDashing = false; // Check if the player is dashing
    [SerializeField] private bool isKnockback = false; // Check if the player is knocked back
    [SerializeField] public GameObject gameOverPanel;//
    [SerializeField] private bool gameOver = false;
    [SerializeField] private Vector3 spawnPoint;

    [SerializeField] public Animator animator1;
    [SerializeField] public Transform attackPoint;
    [SerializeField] public LayerMask enemyLayers;

    [SerializeField] public float attackRange = 0.5f;
    [SerializeField] public int attackDamege = 40;

    [SerializeField] public int maxHealth = 100;
    [SerializeField] private int currentHealth;




    void Start()
    {
        currentHealth = maxHealth;

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
        if (healthSlider == null)
        {
            Debug.LogError("Health Slider UI component is missing");
        }
        if (damageTextPrefab == null)
        {
            Debug.LogError("Damage Text Prefab is missing");
        }

        UpdateCoinText();
        healthSlider.value = health; // Set initial health value
        spawnPoint = transform.position;
    }

    void Update()
    {
        if (!gameOver && !isClimbing && !isKnockback)
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
                rb.gravityScale = 0;
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
                rb.gravityScale = 1;
            }
        }

        // Update idle and run states
        if (isGrounded && !isClimbing)
        {
            animator.SetBool("isGrounded", true);
            if (rb.velocity.x == 0 && !isAttacking && !isDashing)
            {
                animator.SetBool("Idle", true);
                //animator.SetBool("Run", false);
            }
            else
            {
                animator.SetBool("Idle", false);
                //animator.SetBool("Run", true);
            }
        }
        else
        {
            animator.SetBool("isGrounded", false);
            //animator.SetBool("Run", false);
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

        // Check if the player collided with a trap
        if (collision.gameObject.CompareTag("Trap"))
        {
            StartCoroutine(Knockback(collision));
            TakeDamage(10);
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
        if (other.gameObject.CompareTag("Coin2"))
        {
            AddCoin(10); 
            Destroy(other.gameObject); 
        }
        if (other.gameObject.CompareTag("Coin3"))
        {
            AddCoin(20);
            Destroy(other.gameObject);
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
            rb.gravityScale = 1;
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

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Thử lấy component GoblinController từ enemy
            GoblinController goblinController = enemy.GetComponent<GoblinController>();
            if (goblinController != null)
            {
                goblinController.TakeDamage(attackDamege);
            }

            // Thử lấy component Mushroom từ enemy
            Mushroom mushroom = enemy.GetComponent<Mushroom>();
            if (mushroom != null)
            {
                mushroom.TakeDamage(attackDamege);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);



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

    private IEnumerator Knockback(Collision2D collision)
    {
        isKnockback = true;
        animator.SetTrigger("KnockbackTrigger");

        float knockbackDirection = collision.transform.position.x > transform.position.x ? -1 : 1;
        rb.velocity = new Vector2(knockbackDirection * moveSpeed, rb.velocity.y);

        yield return new WaitForSeconds(0.5f); // Thời gian bị bật ra sau

        isKnockback = false;
    }

    private void TakeDamage(int damage)
    {
        health -= damage; // Giảm máu
        healthSlider.value = health; // Cập nhật thanh máu trên UI
        currentHealth -= damage; // Cập nhật máu hiện tại

        if (currentHealth <= 0)
        {
            Die();
        }

        // Instantiate the damage text at the player's position
        GameObject damageText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
        damageText.GetComponent<Text>().text = "-" + damage;
        Destroy(damageText, 1f); // Destroy the damage text after 1 second

        if (health <= 0)
        {
            //
            animator.SetTrigger("Dead");

            // Optionally, disable player controls or perform other game over logic here
            // For example:
            // GetComponent<PlayerController>().enabled = false;

            // Show game over panel after the death animation finishes
            StartCoroutine(ShowGameOverPanel());
        }
    }
    void Die()
    {
        Debug.Log("Player died");

    }

    private IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(2.3f); // Thời gian dựa trên chiều dài thực của animation chết

        // Hiển thị game over panel
        gameOverPanel.SetActive(true);
        Time.timeScale = 0; // Đóng băng thời gian khi hiển thị game over panel
    }

    private void Respawn()
    {
        // Code Respawn giữ nguyên

        gameOver = false; // Đặt lại biến gameOver thành false khi người chơi respawn
        Time.timeScale = 1; // Khôi phục thời gian khi bắt đầu trò chơi mới
        //
    }
}//
