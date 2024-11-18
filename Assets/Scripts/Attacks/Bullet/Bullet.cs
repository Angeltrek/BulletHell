using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletLife = 1f;  // Defines how long before the bullet is deactivated
    public float rotation = 0f;
    public float speed = 1f;
    public int damage = 10; // Damage caused by the bullet

    private Vector2 spawnPoint;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= bulletLife)
        {
            DeactivateBullet();
        }
        spawnPoint = new Vector2(transform.position.x, transform.position.y);
        timer += Time.deltaTime;
        transform.position = Movement(timer);
    }

    private Vector2 Movement(float timer)
    {
        // Moves right according to the bullet's rotation
        float x = timer * speed * transform.right.x;
        float y = timer * speed * transform.right.y;
        return new Vector2(x + spawnPoint.x, y + spawnPoint.y);
    }

    private void DeactivateBullet()
    {
        timer = 0f; // Reset timer for reuse
        gameObject.SetActive(false); // Deactivate the bullet
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player")) // Ensure the player has the tag "Player"
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Apply damage to the player
                DeactivateBullet(); // Deactivate the bullet after hitting
            }
        }
    }
}
