using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dan_boss : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f; // Lifetime of the projectile before it gets destroyed

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap"))
        {
            Destroy(gameObject);
        }
    }
}
