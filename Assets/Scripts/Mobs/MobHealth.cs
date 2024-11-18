using UnityEngine;

public class MobHealth : MonoBehaviour
{
    public int health = 50;
    public GameObject[] lootItems;  // Lista de objetos que el mob puede soltar
    private Animator animator;
    private Collider2D mobCollider;  // Referencia al collider del mob
    private Rigidbody2D rb;  // Referencia al Rigidbody2D (si se usa para movimiento)
    private GameManager gameManager;  // Referencia al GameManager para actualizar el contador de muertes

    void Start()
    {
        // Obtener el componente Animator si existe
        animator = GetComponent<Animator>();

        // Obtener el collider y Rigidbody2D si existen
        mobCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        // Obtener el GameManager
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("No se encontró el GameManager en la escena.");
        }
    }

    public void TakeDamage(int damage)
    {
        // Activar la animación de "Hurt"
        if (animator != null)
        {
            animator.SetTrigger("Hurt"); // Trigger para la animación de daño
        }

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    // Método para verificar si el mob está muerto
    public bool IsDead()
    {
        return health <= 0;
    }


    void Die()
    {
        // Si tienes un Animator, puedes hacer que el enemigo muera con una animación
        if (animator != null)
        {
            animator.SetTrigger("Die"); // Asegúrate de tener un trigger "Die" en tu animador
        }

        // Desactivar el movimiento y el collider
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Detener cualquier movimiento que tenga
            rb.isKinematic = true; // Establecer el Rigidbody en cinemático para que no responda a la física
        }

        if (mobCollider != null)
        {
            mobCollider.enabled = false; // Desactivar el collider para que no detecte más colisiones
        }

        // Suelta loot
        DropLoot();

        // Notificar al GameManager para que incremente el contador de muertes
        if (gameManager != null)
        {
            gameManager.IncrementKillCounter();
        }

        // Lógica de muerte del enemigo
        Destroy(gameObject, 1f); // Espera 1 segundo antes de destruir el objeto para animación o efectos
    }

    // Método para soltar loot cuando el mob muere
    void DropLoot()
    {
        if (lootItems.Length > 0)
        {
            // Selecciona aleatoriamente un loot
            int lootIndex = Random.Range(0, lootItems.Length);
            // Instancia el loot en la posición del mob
            Instantiate(lootItems[lootIndex], transform.position, Quaternion.identity);
            Debug.Log("Loot dropped by the mob!");
        }
    }
}
