using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    public float weakAttackRadius = 1.5f; // Radio de ataque débil
    public float strongAttackRadius = 2.5f; // Radio de ataque fuerte
    public int weakAttackDamage = 10; // Daño del ataque débil
    public int strongAttackDamage = 20; // Daño del ataque fuerte
    public LayerMask enemyLayer; // Capa de los enemigos

    public float weakAttackCooldown = 1f; // Cooldown del ataque débil (en segundos)
    public float strongAttackCooldown = 2f; // Cooldown del ataque fuerte (en segundos)

    private Animator animator;
    private float nextWeakAttackTime = 0f; // Tiempo para el siguiente ataque débil
    private float nextStrongAttackTime = 0f; // Tiempo para el siguiente ataque fuerte
    private AudioManager audioManager; // Referencia al AudioManager

    void Start()
    {
        // Obtener el Animator para las animaciones
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator no encontrado en el jugador. Animaciones deshabilitadas.");
        }

        // Obtener el AudioManager
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
        }
    }

    void Update()
    {
        // Detecta las teclas para los ataques con cooldown
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextWeakAttackTime)
        {
            WeakAttack();
            nextWeakAttackTime = Time.time + weakAttackCooldown; // Establece el próximo tiempo de ataque débil
        }

        if (Input.GetKeyDown(KeyCode.Q) && Time.time >= nextStrongAttackTime)
        {
            StrongAttack();
            nextStrongAttackTime = Time.time + strongAttackCooldown; // Establece el próximo tiempo de ataque fuerte
        }
    }

    private void WeakAttack()
    {
        PerformAttack(weakAttackRadius, weakAttackDamage, "WeakAttack");
    }

    private void StrongAttack()
    {
        PerformAttack(strongAttackRadius, strongAttackDamage, "StrongAttack");
    }

    private void PerformAttack(float attackRadius, int damage, string animationTrigger)
    {
        // Activa la animación específica del ataque, si existe
        if (animator != null)
        {
            animator.SetTrigger(animationTrigger);
        }

        // Reproducir sonido de ataque
        if (audioManager != null)
        {
            audioManager.PlayPlayerAttack();
        }

        // Detecta enemigos dentro del radio del ataque
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayer);

        foreach (Collider2D enemy in enemiesHit)
        {
            // Aplica daño si el enemigo tiene un script de salud
            MobHealth enemyHealth = enemy.GetComponent<MobHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log($"Atacó a {enemy.name} con {damage} de daño usando {animationTrigger}.");
            }
        }
    }

    // Dibuja los radios de ataque en el editor para depuración
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, weakAttackRadius); // Radio del ataque débil

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, strongAttackRadius); // Radio del ataque fuerte
    }
}
