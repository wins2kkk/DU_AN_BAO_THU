using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;

    public Animator animator;
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform firePoint; // The point from which the projectile will be fired
    public float shootRange = 10f; // Range within which the mushroom will shoot at the player
    public float shootInterval = 2f; // Time between each shot

    private Transform player;
    private bool facingRight = true; // Assuming the Mushroom initially faces right
    private bool isPlayerInRange = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(ShootAtPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        isPlayerInRange = distanceToPlayer <= shootRange;

        if (isPlayerInRange)
        {
            FlipTowardsPlayer();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Shoot()
    {
        Vector2 direction = (player.position - firePoint.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectile.GetComponent<dan_boss>().speed;
    }

    void FlipTowardsPlayer()
    {
        if (player.position.x > transform.position.x && !facingRight)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void Die()
    {
        Debug.Log("enemy die");

        animator.SetBool("isDead", false);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        // Destroy the object after the death animation completes
        Destroy(gameObject);
    }

    IEnumerator ShootAtPlayer()
    {
        while (true)
        {
            if (isPlayerInRange)
            {
                Shoot();
            }
            yield return new WaitForSeconds(shootInterval);
        }
    }
}
