using UnityEngine;

public class BOT_SKELETON : MonoBehaviour
{
    // References to components
    public Animator animator;
    [SerializeField]
    GameObject findPlayer;
    private Rigidbody2D rb;

    // Movement parameters
    public float moveSpeed = 5f;
    private Vector2 movement;
    public float movementRange = 5f; // Distance between patrol points

    // Attack parameters
    public float raycastDistance = 5f; // Khoảng cách tia
    public float damage = 10f; // Sát thương gây ra
    public float attackRange = 1f; // Phạm vi tấn công

    // Enemy states
    private bool isDead = false;     // Initially set to false
    private bool isHit;
    private bool isAttacking;
    public AudioSource attackSound; // AudioSource for attack sound effect
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Simplified patrolling: Move back and forth within a range
        float maxPositionX = transform.position.x + movementRange / 2;
        float minPositionX = transform.position.x - movementRange / 2;

        // Change direction if reaching the edge of movement range
        if (transform.position.x >= maxPositionX || transform.position.x <= minPositionX)
        {
            movement *= -1;
        }

        // Movement logic
        float targetX = transform.position.x + movementRange; // Set target X position
        float epsilon = 0.001f; // A small tolerance for floating-point precision

        // Check if reaching the edge of movement range (considering tolerance)
        if (Mathf.Abs(transform.position.x - targetX) < epsilon)
        {
            movementRange *= -1; // Reverse movement direction
            targetX = transform.position.x + movementRange; // Update target X position
        }

        // Create target position with adjusted X based on movement range
        Vector2 targetPosition = new Vector2(targetX, transform.position.y);

        // Update movement based on direction
        Vector2 direction; // Declaration
                           // lỗi direction = (targetPosition - transform.position).normalized; // Initialization



        // Set animation state based on behavior
        if (movement == Vector2.zero)
        {
            animator.SetTrigger("idle"); // Trigger idle animation
        }
        else
        {
            animator.SetTrigger("run"); // Trigger run animation
        }

        // Additional animation triggers based on logic (e.g., attack, death)
        if (isAttacking)
        {
            animator.SetTrigger("attack"); // Trigger attack animation
            // Play attack sound when animation starts
            if (!attackSound.isPlaying)
            {
                attackSound.Play();
            }
        }
        else if (isDead)
        {
            animator.SetTrigger("death"); // Trigger death animation
        }

        // Trigger TakeHit animation when hit
        if (isHit)
        {
            animator.SetTrigger("takehit"); // Trigger TakeHit animation
            isHit = false; // Reset isHit flag after triggering animation
        }

        // Player detection and chasing logic (assuming you have a way to check if the Player is within range)
        Vector2 playerDirection = findPlayer.transform.position - transform.position;
        float playerDistance = Vector2.Distance(transform.position, findPlayer.transform.position);

        // Chase player only if within attack range
        if (playerDistance <= attackRange)
        {
            // Determine facing direction based on Player's position
            float facingDirection = Mathf.Sign(playerDirection.x); // -1 if left, 1 if right

            // Rotate based on facing direction (consider using transform.localScale.x)
            transform.localScale = new Vector3(facingDirection, 1f, 1f);

            // Normalize and set movement direction
            playerDirection.Normalize();
            movement = playerDirection;

            // Attack if in range
            if (playerDistance <= raycastDistance)
            {
                isAttacking = true;
            }
        }
        else
        {
            // Stop chasing if outside attack range (optional)
            movement = Vector2.zero;
            isAttacking = false;
        }
    }

    public void FixedUpdate()
    {
        moveCharacter(movement);
    }

    void moveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }
}
