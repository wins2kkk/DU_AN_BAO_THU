using UnityEngine;

public class FallingTrap : MonoBehaviour
{
    public float fallSpeed = 5f; // Speed at which the trap falls
    public float destroyDelay = 5f; // Time after which the trap is destroyed if it doesn't hit the player

    [HideInInspector]
    public bool isFalling = false; // Flag to check if the trap should start falling

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // Make the trap initially kinematic to keep it stationary
    }

    void Update()
    {
        if (isFalling)
        {
            rb.isKinematic = false; // Disable kinematic mode to start falling
            rb.velocity = new Vector2(0, -fallSpeed); // Move the trap downwards
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Destroy the trap if it hits the player
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            // Destroy the trap if it hits the ground after some delay
            Destroy(gameObject, destroyDelay);
        }
    }
}
