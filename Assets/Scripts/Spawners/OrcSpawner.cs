using System.Collections;
using UnityEngine;

public class OrcSpawner : MonoBehaviour
{
    public GameObject orcPrefab;          // Prefab del orco a generar
    public Transform spawnPoint;          // Punto de aparición de los orcos
    public int maxOrcsPerWave = 3;       // Máximo de orcos por oleada
    public float spawnInterval = 7f;     // Intervalo de tiempo para generar orcos (en segundos)

    private bool canSpawn = true;         // Bandera para verificar si se puede generar más orcos
    private GameObject[] orcPool;        // Pool de orcos
    private int orcCount;                // Contador de orcos generados

    // Start is called before the first frame update
    void Start()
    {
        orcPool = new GameObject[maxOrcsPerWave];
        StartCoroutine(SpawnOrcs());
    }

    // Update is called once per frame
    void Update()
    {
        // Verificar si se pueden generar más orcos
        if (canSpawn && IsAllOrcsDead())
        {
            canSpawn = false; // Evitar crear orcos más de una vez hasta que todos estén muertos
            StartCoroutine(SpawnOrcs());
        }
    }

    // Corutina para generar orcos cada 7 segundos
    private IEnumerator SpawnOrcs()
    {
        // Generar orcos hasta el máximo de orcos por oleada
        int orcsToSpawn = Mathf.Min(maxOrcsPerWave, orcCount + 1);

        for (int i = 0; i < orcsToSpawn; i++)
        {
            if (orcPool[i] == null)
            {
                orcPool[i] = Instantiate(orcPrefab, spawnPoint.position, Quaternion.identity);
                // Puedes añadir la lógica de inicialización del orco aquí, como configurar su comportamiento
            }
        }

        // Esperar 7 segundos antes de generar más orcos
        yield return new WaitForSeconds(spawnInterval);

        // Permitir generar más orcos solo si todos los orcos están muertos
        canSpawn = true;
    }

    // Función para verificar si todos los orcos están muertos
    private bool IsAllOrcsDead()
    {
        foreach (GameObject orc in orcPool)
        {
            if (orc != null && orc.activeInHierarchy)
            {
                return false; // Si hay algún orco activo, no generamos más
            }
        }
        return true; // Si todos los orcos están muertos, se puede generar más
    }
}
