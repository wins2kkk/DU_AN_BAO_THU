using UnityEngine;
public Animator animator;


public class BOT : MonoBehaviour
{
    [SerializeField]
    GameObject findPlayer;
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public float raycastDistance = 5f; // Khoảng cách tia

    public float damage = 10f; // Sát thương gây ra
    public float attackRange = 5f; // Phạm vi tấn công

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 direction = findPlayer.transform.position - transform.position;
        float distance = Vector2.Distance(transform.position, findPlayer.transform.position);

        // Chase player only if within attack range
        if (distance <= attackRange)
        {
            // Determine facing direction based on Player's position
            float facingDirection = Mathf.Sign(direction.x); // -1 if left, 1 if right

            // Rotate based on facing direction (consider using transform.localScale.x)
            transform.localScale = new Vector3(facingDirection, 1f, 1f);

            // Normalize and set movement direction
            direction.Normalize();
            movement = direction;
        }
        else
        {
            // Stop chasing if outside attack range (optional)
            movement = Vector2.zero;
        }
    }

    public void FixedUpdate()
    {
        moveCharacter(movement);
        // Set animation state based on behavior
        if (movement == Vector2.zero)
        {
            animator.SetTrigger("Idle"); // Trigger idle animation
        }
        else
        {
            animator.SetTrigger("Run"); // Trigger run animation
        }

        // Additional animation triggers based on logic (e.g., attack, death)
        if (isAttacking)
        {
            animator.SetTrigger("Attack"); // Trigger attack animation
        }
        else if (isDead)
        {
            animator.SetTrigger("Death"); // Trigger death animation
        }
    }

    void moveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }
}
