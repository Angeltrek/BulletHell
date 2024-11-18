using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float lifeTime;

    private Rigidbody2D rb;

    // Referencia al AudioManager
    private AudioManager audioManager;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.enabled = true; // Asegura que el SpriteRenderer esté habilitado
        }

        // Buscar el AudioManager en la escena
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager no encontrado.");
        }
    }

    public void Initialize(Vector2 direction, float speed, float lifeTime)
    {
        this.direction = direction.normalized; // Asegúrate de que la dirección esté normalizada
        this.speed = speed;
        this.lifeTime = lifeTime;

        // Aplica la velocidad al Rigidbody2D
        if (rb != null)
        {
            rb.linearVelocity = this.direction * this.speed; // Usar velocity en lugar de linearVelocity
        }

        // Rota la bala para que apunte hacia la dirección del movimiento
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Destruye la bala después del tiempo de vida
        Invoke(nameof(DestroyBullet), lifeTime);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject); // Elimina el objeto completamente
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Colisión detectada con: {collision.name}, Tag: {collision.tag}");

        if (collision.CompareTag("Enemy"))
        {
            MobHealth enemyHealth = collision.GetComponent<MobHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(10);
                Debug.Log("Daño aplicado al enemigo.");
            }

            // Reproducir el sonido de impacto
            audioManager?.PlayPlayerFireHit();

            DestroyBullet();
        }

        if (collision.CompareTag("Ground"))
        {
            DestroyBullet();
        }
    }

    void OnDisable()
    {
        CancelInvoke(); // Asegúrate de cancelar el temporizador si la bala se desactiva antes
    }
}
