using UnityEngine;
using TMPro;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float bulletLife = 2f;
    public float shootCooldown = 0.2f; // Tiempo de cooldown entre disparos
    public int maxShots = 200; // Número máximo de balas
    public float waitTimeAfterMaxShots = 3f; // Tiempo de espera después de alcanzar el máximo de disparos

    private int currentShots = 0; // Contador de disparos realizados
    private float nextShotTime = 0f; // Tiempo en que se puede realizar el siguiente disparo
    private bool canShoot = true; // Estado para controlar si puede disparar

    // Referencia al Animator
    private Animator animator;

    // Referencias para el UI
    public TextMeshProUGUI bulletNumberText;  // Referencia al TextMeshPro que muestra el número de balas
    public GameObject heartPanel;  // Panel donde se encuentra el TextMeshPro

    // Referencia al AudioManager
    private AudioManager audioManager;

    void Start()
    {
        // Obtener el Animator del jugador
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator no encontrado en el jugador.");
        }

        // Inicializar el contador de balas
        UpdateBulletText();

        // Obtener referencia al AudioManager
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager no encontrado.");
        }
    }

    void Update()
    {
        // Revisar si se presiona el botón de disparo y si el jugador puede disparar
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("BulletPrefab o FirePoint no asignado en el Inspector.");
            return;
        }

        // Verificar si hay balas para disparar
        if (currentShots >= maxShots)
        {
            canShoot = false; // No puede disparar más si no hay balas
            bulletNumberText.color = Color.red; // Cambiar color del texto a rojo
            return;
        }

        // Activar la animación de disparo
        if (animator != null)
        {
            animator.SetTrigger("isShooting");
        }

        // Instanciar la bala
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Configurar la dirección según hacia dónde mire el jugador
        Vector2 shootDirection = firePoint.right;
        if (transform.localScale.x < 0) // Si el jugador está mirando hacia la izquierda
        {
            shootDirection = -firePoint.right;
        }

        // Inicializar la bala
        PlayerBullet bulletScript = bullet.GetComponent<PlayerBullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(shootDirection, bulletSpeed, bulletLife);
        }

        // Actualizar el contador de balas y el texto
        currentShots++;
        UpdateBulletText();

        // Reproducir sonido de disparo
        if (audioManager != null)
        {
            audioManager.PlayPlayerFire(); // Asumiendo que existe este método en AudioManager
        }

        // Ajustar el cooldown para el próximo disparo
        nextShotTime = Time.time + shootCooldown;
    }

    public void IncreaseBullets(int amount)
    {
        maxShots = maxShots + amount;
        Debug.Log("Balas actuales: " + maxShots);
        UpdateBulletText();

        if (audioManager != null)
        {
            audioManager.PlayPlayerGetBullets(); // Asumiendo que existe este método en AudioManager
        }
    }

    // Método para actualizar el texto de balas
    void UpdateBulletText()
    {
        bulletNumberText.text = (maxShots - currentShots).ToString(); // Mostrar balas restantes

        // Si no hay balas, el texto se pone rojo y no puede disparar
        if (currentShots >= maxShots)
        {
            bulletNumberText.color = Color.red;
        }
        else
        {
            bulletNumberText.color = Color.white; // Volver a color blanco si hay balas
        }
    }
}
