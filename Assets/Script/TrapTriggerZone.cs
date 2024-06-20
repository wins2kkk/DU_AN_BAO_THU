using UnityEngine;

public class TrapTriggerZone : MonoBehaviour
{
    public GameObject trap; // Reference to the trap GameObject

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Enable the trap to start falling
            trap.GetComponent<FallingTrap>().isFalling = true;

            // Destroy the trigger zone to prevent further interactions
            Destroy(gameObject);
        }
    }
}
