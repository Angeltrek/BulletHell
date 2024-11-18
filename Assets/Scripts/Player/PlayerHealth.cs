using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Salud máxima del jugador
    private int currentHealth;

    private Animator animator; // Referencia al Animator del jugador
    private PlayerController playerController; // Referencia al controlador del jugador
    public float damageCooldown = 0.2f; // Tiempo de espera antes de poder recibir daño nuevamente
    private bool canTakeDamage = true; // Determina si el jugador puede recibir daño
    public PlayerHealthUI healthUI; // Referencia al script que maneja la UI de corazones

    private AudioManager audioManager; // Referencia al AudioManager

    void Start()
    {
        currentHealth = maxHealth; 
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();

        // Obtener el AudioManager
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
        }

        // Actualizar los corazones en la UI al inicio
        if (healthUI != null)
        {
            healthUI.UpdateHearts(currentHealth, maxHealth);
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetCurrentHealth(int health)
    {
        currentHealth = health;
        if (healthUI != null)
        {
            healthUI.UpdateHearts(currentHealth, maxHealth);
        }

                    // Reproducir sonido de curación
        if (audioManager != null)
        {
            audioManager.PlayPlayerHeal();
        }
    }

    public void TakeDamage(int damage)
    {
        if (canTakeDamage)
        {
            currentHealth -= damage;

            // Reproducir sonido de daño
            if (audioManager != null)
            {
                audioManager.PlayPlayerHit();
            }

            if (animator != null)
            {
                animator.SetTrigger("TakeDamage");
            }

            playerController.SetCanMove(false);

            // Actualizar la UI de corazones
            if (healthUI != null)
            {
                healthUI.UpdateHearts(currentHealth, maxHealth);
                Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");
            }

            StartCoroutine(DamageCooldown());

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        playerController.SetCanMove(true);
        canTakeDamage = true;
    }

    private void Die()
    {
        Debug.Log("Player is dead!");

        // Reproducir sonido de muerte
        if (audioManager != null)
        {
            audioManager.PlayPlayerDied();
        }

        // Cargar la escena de Game Over
        SceneManager.LoadScene("GameOver"); 
    }
}
