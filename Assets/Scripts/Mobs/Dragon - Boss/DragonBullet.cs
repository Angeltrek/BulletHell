using UnityEngine;

public class DragonBullet : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float lifeTime;

    private Rigidbody2D rb;

    void Awake()
    {
        // Asignar el Rigidbody2D cuando se crea la bala
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction, float speed, float lifeTime)
    {
        this.direction = direction.normalized; // Asegúrate de que la dirección esté normalizada
        this.speed = speed;
        this.lifeTime = lifeTime;

        // Aplica la velocidad al Rigidbody2D
        if (rb != null)
        {
            rb.linearVelocity = this.direction * this.speed; // Usa velocity en lugar de linearVelocity
        }

        // Rota la bala para que apunte hacia la dirección del movimiento
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180f)); // Agregar 180 grados

        // Destruye la bala después del tiempo de vida
        Invoke(nameof(DestroyBullet), lifeTime);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if (collision.CompareTag("Player")) // Asegúrate de que el jugador tenga la etiqueta "Player"
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); // Aplica daño al jugador
                DestroyBullet(); // Desactiva la bala después de impactar
            }
        }
    }

    void OnDisable()
    {
        CancelInvoke(); // Asegúrate de cancelar el temporizador si la bala se desactiva antes
    }
}
