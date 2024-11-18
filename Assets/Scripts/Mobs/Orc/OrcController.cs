using UnityEngine;

public class OrcController : MonoBehaviour
{
    public int weakAttackDamage = 3;         // Daño del ataque débil
    public int strongAttackDamage = 5;       // Daño del ataque fuerte
    public float attackRadius = 2.0f;       // Radio de ataque (distancia de ataque cuerpo a cuerpo)
    public float moveSpeed = 2.0f;          // Velocidad de movimiento hacia el jugador
    public float attackCooldown = 1.0f;     // Tiempo de cooldown entre ataques
    public LayerMask playerLayer;           // Capa del jugador

    private Animator animator;               // Referencia al Animator del orco
    private MobHealth mobHealth;             // Referencia al script de salud del orco
    private float nextAttackTime = 0f;       // Controla el tiempo de cooldown de ataques
    private Transform playerTransform;       // Referencia al transform del jugador
    private bool isWalking = false;          // Control de la animación de caminar

    private AudioManager audioManager;       // Referencia al AudioManager

    void Start()
    {
        // Obtener el Animator del orco
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator no encontrado en el orco. Animaciones deshabilitadas.");
        }

        // Obtener el MobHealth del orco
        mobHealth = GetComponent<MobHealth>();
        if (mobHealth == null)
        {
            Debug.LogWarning("MobHealth no encontrado en el orco. El orco no puede recibir daño.");
        }

        // Buscar al jugador en la escena
        playerTransform = GameObject.FindWithTag("Player")?.transform;

        // Obtener el AudioManager en la escena
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager no encontrado en la escena. Los sonidos no funcionarán.");
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // Verifica si el jugador está dentro del rango de ataque
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= attackRadius && Time.time >= nextAttackTime) // Si hay un jugador cerca y no está en cooldown
            {
                // Realiza un ataque débil o fuerte según algún criterio (por ejemplo, probabilidad o preferencia)
                if (Random.Range(0f, 1f) < 0.5f) // Ataque débil con 50% de probabilidad
                {
                    WeakAttack(playerTransform);
                }
                else
                {
                    StrongAttack(playerTransform);
                }

                nextAttackTime = Time.time + attackCooldown; // Reinicia el cooldown
            }
            else if (distanceToPlayer > attackRadius) // Si el jugador está fuera del rango de ataque, mover hacia él
            {
                MoveTowardsPlayer();
            }

            // Asegúrate de que el orco mire hacia el jugador
            LookAtPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

        if (!isWalking)
        {
            isWalking = true;
            animator.SetBool("isWalking", true);
        }
    }

    private void LookAtPlayer()
    {
        Vector2 direction = playerTransform.position - transform.position;

        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void WeakAttack(Transform player)
    {
        if (animator != null)
        {
            animator.SetTrigger("WeakAttack");
        }

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(weakAttackDamage);
            PlayGoblinAttack();
            Debug.Log("Ataque débil realizado al jugador.");
        }
    }

    private void StrongAttack(Transform player)
    {
        if (animator != null)
        {
            animator.SetTrigger("StrongAttack");
        }

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(strongAttackDamage);
            PlayGoblinAttack();
            Debug.Log("Ataque fuerte realizado al jugador.");
        }
    }

    public void TakeDamage(int damage)
    {
        if (mobHealth != null)
        {
            mobHealth.TakeDamage(damage);
            PlayGoblinHurt();
            Debug.Log($"El orco recibió {damage} de daño.");

            if (mobHealth.IsDead()) // Comprueba si el orco murió
            {
                PlayGoblinDeath();
                Debug.Log("El orco murió.");
            }
        }
    }

    private void PlayGoblinAttack()
    {
        if (audioManager != null)
        {
            audioManager.PlayGoblinAttack();
        }
    }

    private void PlayGoblinHurt()
    {
        if (audioManager != null)
        {
            audioManager.PlayGoblinHurt();
        }
    }

    private void PlayGoblinDeath()
    {
        if (audioManager != null)
        {
            audioManager.PlayGoblinDeath();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
