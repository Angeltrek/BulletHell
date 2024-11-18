using UnityEngine;

public class DragonBossController : MonoBehaviour
{
    public float moveSpeed = 2f; // Velocidad de vuelo del dragón
    public float attackCooldown = 4f; // Tiempo entre ataques
    public float projectileSpeed = 5f; // Velocidad de los proyectiles
    private int currentHealth; // Salud actual del dragón

    public GameObject projectilePrefab; // Prefab del proyectil
    public Transform firePoint; // Punto desde donde el dragón escupe fuego

    private float nextAttackTime = 0f; // Tiempo en que el dragón puede atacar nuevamente
    private Transform player; // Referencia al jugador
    private bool isAttacking = false; // Si el dragón está atacando

    private Vector3 initialPosition; // Posición inicial del dragón
    private float moveTimer = 0f; // Temporizador de movimiento y ataque
    private float restTimer = 0f; // Temporizador de descanso
    private bool isResting = false; // Si el dragón está descansando
    private bool isFlying = false; // Si el dragón está volando

    private Animator animator;
    private AudioManager audioManager; // Referencia al AudioManager
    private float soundCooldown = 0f; // Temporizador de sonido
    public float flySoundInterval = 0.6f; // Intervalo de sonido cuando vuela
    public float restSoundInterval = 0.7f; // Intervalo de sonido cuando está descansando
    private float screamCooldown = 0f; // Temporizador de sonido de gritos
    public float screamMinInterval = 4f; // Intervalo mínimo entre gritos
    public float screamMaxInterval = 7f; // Intervalo máximo entre gritos

    void Start()
    {
        // Buscar al jugador
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Guardar la posición inicial del dragón
        initialPosition = transform.position;

        // Obtener el Animator y AudioManager
        animator = GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();

        if (animator == null)
        {
            Debug.LogError("Animator no encontrado en el dragón.");
        }

        if (audioManager == null)
        {
            Debug.LogError("AudioManager no encontrado.");
        }
    }

    void Update()
    {
        if (isResting)
        {
            Rest();
        }
        else
        {
            // Temporizador para el tiempo de movimiento y ataque
            moveTimer += Time.deltaTime;

            if (moveTimer >= 30f)
            {
                // Después de 10 segundos, regresa a la posición inicial y empieza a descansar
                ReturnToInitialPosition();
                isResting = true;
                isFlying = false; // El dragón deja de volar
                moveTimer = 0f; // Resetear temporizador
            }
            else
            {
                // Comportamiento de vuelo: el dragón se mueve hacia el jugador
                if (!isAttacking)
                {
                    FlyTowardsPlayer();
                }

                // Hacer que el dragón se voltee dependiendo de la posición del jugador
                FlipDragonBasedOnPlayer();

                // Controlar los ataques del dragón
                if (Time.time >= nextAttackTime && !isAttacking)
                {
                    Attack();
                }
            }
        }

        // Control de la velocidad del sonido según el estado de vuelo o descanso
        ControlFlyingSound();

        // Reproducir un grito del dragón de manera aleatoria
        HandleDragonScreaming();
    }

    // Método para mover al dragón hacia el jugador
    void FlyTowardsPlayer()
    {
        isFlying = true; // Activar el estado de vuelo
        animator.SetBool("isFlying", true); // Cambiar la animación a vuelo

        Vector3 direction = (player.position - transform.position).normalized; // Dirección hacia el jugador
        transform.position += direction * moveSpeed * Time.deltaTime; // Mover al dragón
    }

    // Método para voltear al dragón según la posición del jugador
    void FlipDragonBasedOnPlayer()
    {
        // Si el jugador está a la izquierda del dragón, voltear hacia la izquierda
        if (player.position.x > transform.position.x)
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            // Voltear el dragón (hacerlo mirar hacia la derecha)
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    // Método para atacar (escupir fuego)
    void Attack()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogError("ProjectilePrefab o FirePoint no asignados.");
            return;
        }

        isAttacking = true;
        animator.SetTrigger("Attack"); // Si tienes una animación de ataque, activa el trigger

        // Instanciar un proyectil y dispararlo hacia el jugador
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Vector2 shootDirection = (player.position - firePoint.position).normalized; // Dirección del disparo

        // Inicializar el proyectil
        DragonBullet bulletScript = projectile.GetComponent<DragonBullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(shootDirection, projectileSpeed, 2f); // Configura el proyectil
        }

        // Establecer el siguiente ataque
        nextAttackTime = Time.time + attackCooldown;
        isAttacking = false; // El dragón puede volver a moverse después de un ataque

        // Reproducir sonido de ataque
        audioManager.PlayDragonAttacking();
    }

    // Método para descansar y recuperar vida
    void Rest()
    {
        isFlying = false; // El dragón no está volando mientras descansa
        animator.SetBool("isFlying", false); // Desactivar animación de vuelo

        restTimer += Time.deltaTime;

        if (restTimer >= 5f)
        {
            // Si el dragón ha descansado durante 5 segundos, puede empezar a moverse de nuevo
            restTimer = 0f; // Resetear temporizador de descanso
            isResting = false;
            moveTimer = 0f; // Reiniciar el temporizador de movimiento
        }
    }

    // Método para hacer que el dragón regrese a su posición inicial
    void ReturnToInitialPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
    }

    // Método para controlar la velocidad de reproducción del sonido basado en el estado de vuelo
    void ControlFlyingSound()
    {
        soundCooldown -= Time.deltaTime;

        if (isFlying && soundCooldown <= 0f)
        {
            // Si está volando, reproducir sonido más rápido
            audioManager.PlayDragonFlying();
            soundCooldown = flySoundInterval; // Sonido más rápido
        }
        else if (isResting && soundCooldown <= 0f)
        {
            // Si está descansando, reproducir sonido más lento
            audioManager.PlayDragonFlying(); // Usamos el mismo sonido, pero con diferente intervalo
            soundCooldown = restSoundInterval; // Sonido más lento
        }
    }

    // Método para manejar los gritos aleatorios del dragón
    void HandleDragonScreaming()
    {
        screamCooldown -= Time.deltaTime;

        // Si el temporizador se agota, reproducir el grito y reiniciar el temporizador con un valor aleatorio
        if (screamCooldown <= 0f)
        {
            audioManager.PlayDragonScreaming();
            screamCooldown = Random.Range(screamMinInterval, screamMaxInterval);
        }
    }

    public bool IsResting()
    {
        return isResting; // Devolver el estado de reposo del dragón
    }
}
