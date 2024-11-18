using UnityEngine;
using UnityEngine.UI;

public class HeartController : MonoBehaviour
{
    public int healthIncrease = 15; // Cantidad de salud que se suma al recoger el corazón
    public LayerMask playerLayer;   // Capa que representa al jugador
    public float floatSpeed = 0.5f; // Velocidad del movimiento flotante
    public float floatHeight = 0.5f; // Altura del movimiento flotante (amplitud del "flote")

    private Vector3 originalPosition;

    void Start()
    {
        // Guardar la posición original para el movimiento flotante
        originalPosition = transform.position;
    }

    void Update()
    {
        // Movimiento flotante: usa una función seno para crear un movimiento hacia arriba y hacia abajo
        float newY = originalPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el objeto que entra en el trigger es el jugador
        Debug.Log("Objeto en el trigger: " + other.gameObject.name);

        if ((playerLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            // Intentar obtener el script de salud del jugador
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                // Calcular cuánta salud se debe añadir
                int healthToAdd = Mathf.Min(healthIncrease, playerHealth.maxHealth - playerHealth.GetCurrentHealth());

                // Sumar la salud al jugador
                playerHealth.SetCurrentHealth(playerHealth.GetCurrentHealth() + healthToAdd);
                Debug.Log("Salud del jugador aumentada: " + healthToAdd);

                // Destruir el corazón después de ser recogido
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("No se encontró el componente PlayerHealth en el jugador");
            }
        }
    }
}
