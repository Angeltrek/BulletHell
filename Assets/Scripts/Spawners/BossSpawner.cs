using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab;  // Prefab del boss a spawnnear
    public Transform spawnPoint;  // Punto donde aparecerá el boss
    private GameManager gameManager;  // Referencia al GameManager para obtener el contador de kills

    void Start()
    {
        // Buscar el GameManager en la escena
        gameManager = FindObjectOfType<GameManager>();
        
        // Asegurarnos de que el GameManager esté asignado
        if (gameManager == null)
        {
            Debug.LogError("GameManager no encontrado en la escena.");
        }
    }

    void Update()
    {
        // Verificar si el contador de kills alcanza 15
        if (gameManager != null && gameManager.killCount >= 15)
        {
            SpawnBoss();
            gameManager.killCount = 0;  // Reiniciar el contador de kills después de spawnnear el boss
        }
    }

    void SpawnBoss()
    {
        Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);

        // Puedes agregar alguna animación o efectos visuales aquí
        Debug.Log("¡Boss spawneado!");
    }
}
