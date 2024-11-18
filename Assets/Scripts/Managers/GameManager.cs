using UnityEngine;
using TMPro;  // Necesario para TextMeshPro

public class GameManager : MonoBehaviour
{
    public int killCount = 0;  // Contador de kills
    public TextMeshProUGUI killCounterText;  // Referencia al TextMeshPro que muestra el contador de kills

    void Start()
    {
        // Asegurarse de que el killCounterText esté asignado
        if (killCounterText == null)
        {
            Debug.LogError("killCounterText no asignado en el Inspector.");
        }
    }

    public void IncrementKillCounter()
    {
        killCount++;  // Incrementar el contador de kills

        // Actualizar el texto en pantalla
        if (killCounterText != null)
        {
            killCounterText.text = "Kills: " + killCount;
        }
    }
}
