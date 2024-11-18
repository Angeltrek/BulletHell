using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;  // Componente de AudioSource que reproduce los sonidos
    public AudioClip playerRunning;
    public AudioClip playerHit;
    public AudioClip playerDied;
    public AudioClip playerJump;
    public AudioClip playerFire;
    public AudioClip playerFireHit;
    public AudioClip playerHeal;
    public AudioClip playerGetBullets;
    public AudioClip playerAttack;
    public AudioClip goblinAttack;
    public AudioClip goblinHurt;
    public AudioClip goblinDeath;
    public AudioClip dragonFlying;
    public AudioClip dragonAttacking;
    public AudioClip dragonScreaming;
    public AudioClip dragonSummoning;
    public AudioClip backgroundMusic; // Clip de música de fondo

    void Start()
    {
        // Obtener el AudioSource del objeto si no se asignó manualmente
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Configurar y reproducir la música de fondo en bucle
        if (audioSource != null && backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic; // Asignar la música de fondo
            audioSource.loop = true; // Reproducirla en bucle
            audioSource.Play(); // Iniciar la música de fondo
        }
    }

    // Métodos para reproducir otros sonidos (sin cambios)
    public void PlayPlayerRunning()
    {
        if (playerRunning != null && audioSource != null)
        {
            audioSource.PlayOneShot(playerRunning);
        }
    }

    public void PlayPlayerHit()
    {
        if (playerHit != null && audioSource != null)
        {
            audioSource.PlayOneShot(playerHit);
        }
    }

    public void PlayPlayerDied()
    {
        if (playerDied != null && audioSource != null)
        {
            audioSource.PlayOneShot(playerDied);
        }
    }

    public void PlayPlayerJump()
    {
        if (playerJump != null && audioSource != null)
        {
            audioSource.PlayOneShot(playerJump);
        }
    }

    public void PlayPlayerFire()
    {
        if (playerFire != null && audioSource != null)
        {
            audioSource.PlayOneShot(playerFire);
        }
    }

    public void PlayPlayerFireHit()
    {
        if (playerFireHit != null && audioSource != null)
        {
            audioSource.PlayOneShot(playerFireHit);
        }
    }

    public void PlayPlayerHeal()
    {
        if (playerHeal != null && audioSource != null)
        {
            audioSource.PlayOneShot(playerHeal);
        }
    }

    public void PlayPlayerGetBullets()
    {
        if (playerGetBullets != null && audioSource != null)
        {
            audioSource.PlayOneShot(playerGetBullets);
        }
    }

    public void PlayPlayerAttack()
    {
        if (playerAttack != null && audioSource != null)
        {
            audioSource.PlayOneShot(playerAttack);
        }
    }

    public void PlayGoblinAttack()
    {
        if (goblinAttack != null && audioSource != null)
        {
            audioSource.PlayOneShot(goblinAttack);
        }
    }

    public void PlayGoblinHurt()
    {
        if (goblinHurt != null && audioSource != null)
        {
            audioSource.PlayOneShot(goblinHurt);
        }
    }

    public void PlayGoblinDeath()
    {
        if (goblinDeath != null && audioSource != null)
        {
            audioSource.PlayOneShot(goblinDeath);
        }
    }

    public void PlayDragonFlying()
    {
        if (dragonFlying != null && audioSource != null)
        {
            audioSource.PlayOneShot(dragonFlying);
        }
    }

    public void PlayDragonAttacking()
    {
        if (dragonAttacking != null && audioSource != null)
        {
            audioSource.PlayOneShot(dragonAttacking);
        }
    }

    public void PlayDragonScreaming()
    {
        if (dragonScreaming != null && audioSource != null)
        {
            audioSource.PlayOneShot(dragonScreaming);
        }
    }

    public void PlayDragonSummoning()
    {
        if (dragonSummoning != null && audioSource != null)
        {
            audioSource.PlayOneShot(dragonSummoning);
        }
    }
}
