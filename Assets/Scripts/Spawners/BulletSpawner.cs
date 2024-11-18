using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [Header("Bullet Attributes")]
    public GameObject bullet;
    public float bulletLife = 2f;
    public float speed = 1f;

    [Header("Spawner Attributes")]
    [SerializeField] private float firingRate = 2f;
    [SerializeField] private float rotationSpeed = 1f; // Velocidad de rotación configurable
    [SerializeField] private int poolSize = 10;

    // Variables para patrones de disparo
    private enum SpawnerPattern { Star, Square, Circle }

    private SpawnerPattern spawnerPattern;
    private List<GameObject> bulletPool = new List<GameObject>();
    private float firingTimer = 0f;

    private DragonBossController dragonController; // Referencia al controlador del dragón
    private bool isResting = false;
    private AudioManager audioManager; // Referencia al AudioManager

    void Start()
    {
        // Obtener la referencia al controlador del dragón
        dragonController = FindObjectOfType<DragonBossController>();

        // Obtener la referencia al AudioManager
        audioManager = FindObjectOfType<AudioManager>();

        // Elegir un patrón de disparo aleatorio
        spawnerPattern = (SpawnerPattern)Random.Range(0, 3);
    }

    void FixedUpdate()
    {
        if (dragonController != null)
        {
            isResting = dragonController.IsResting(); // Verificar si el dragón está descansando
        }

        // Mover el BulletSpawner para que siga al dragón
        if (dragonController != null)
        {
            transform.position = dragonController.transform.position; // Sigue al dragón
        }

        if (isResting) // Solo activar el spawner si el dragón está descansando
        {
            if (bulletPool == null || bulletPool.Count == 0) // Crear la pool si no existe
            {
                InitializeBulletPool();
            }

            firingTimer += Time.fixedDeltaTime;

            if (firingTimer >= firingRate)
            {
                Fire();
                firingTimer = 0;
            }
        }
        else
        {
            if (bulletPool != null && bulletPool.Count > 0) // Limpiar la pool cuando el dragón no está descansando
            {
                ClearBulletPool();
            }
        }
    }

    private void InitializeBulletPool()
    {
        bulletPool.Clear(); // Asegurarse de que la lista esté vacía antes de agregar nuevas balas

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bullet, transform.position, Quaternion.identity);
            obj.SetActive(false);
            bulletPool.Add(obj);
        }
    }

    private void ClearBulletPool()
    {
        foreach (GameObject obj in bulletPool)
        {
            obj.SetActive(false);
        }

        bulletPool.Clear(); // Limpiar la lista de la pool
    }

    private GameObject GetBulletFromPool()
    {
        foreach (GameObject pooledBullet in bulletPool)
        {
            if (!pooledBullet.activeInHierarchy)
            {
                return pooledBullet;
            }
        }

        // Si no hay balas inactivas, crear una nueva
        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        newBullet.SetActive(false);
        bulletPool.Add(newBullet);
        return newBullet;
    }

    public void Fire()
    {
        // Reproducir el sonido de invocación del dragón al disparar
        audioManager.PlayDragonSummoning();

        GameObject bulletToFire = GetBulletFromPool();

        if (bulletToFire != null)
        {
            bulletToFire.transform.position = transform.position;
            bulletToFire.transform.rotation = transform.rotation;
            bulletToFire.SetActive(true);

            Bullet bulletScript = bulletToFire.GetComponent<Bullet>();
            bulletScript.speed = speed;
            bulletScript.bulletLife = bulletLife;

            // Llamar al patrón de disparo
            FirePattern();
        }
    }

    private void FirePattern()
    {
        switch (spawnerPattern)
        {
            case SpawnerPattern.Star:
                FireStarPattern();
                break;
            case SpawnerPattern.Square:
                FireSquarePattern();
                break;
            case SpawnerPattern.Circle:
                FireCirclePattern();
                break;
        }
    }

    private void FireStarPattern()
    {
        float angleStep = 72f; // 360 grados / 5 disparos
        float angle = 0f;

        for (int i = 0; i < 5; i++)
        {
            FireBulletAtAngle(angle);
            angle += angleStep;
        }
    }

    private void FireSquarePattern()
    {
        // Disparar en las 4 direcciones: arriba, abajo, izquierda y derecha
        FireBulletAtAngle(0f);
        FireBulletAtAngle(90f);
        FireBulletAtAngle(180f);
        FireBulletAtAngle(270f);
    }

    private void FireCirclePattern()
    {
        // Disparar en un círculo de 360 grados
        float angleStep = 15f; // 360 grados / 24 disparos
        float angle = 0f;

        for (int i = 0; i < 24; i++)
        {
            FireBulletAtAngle(angle);
            angle += angleStep;
        }
    }

    private void FireBulletAtAngle(float angle)
    {
        GameObject bulletToFire = GetBulletFromPool();

        if (bulletToFire != null)
        {
            bulletToFire.transform.position = transform.position;
            bulletToFire.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            bulletToFire.SetActive(true);

            Bullet bulletScript = bulletToFire.GetComponent<Bullet>();
            bulletScript.speed = speed;
            bulletScript.bulletLife = bulletLife;
        }
    }
}
