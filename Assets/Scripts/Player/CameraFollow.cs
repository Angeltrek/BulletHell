using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;        // Referencia al Transform del jugador
    public Vector3 offset;          // Desplazamiento entre la cámara y el jugador

    // LateUpdate se llama después de que todos los objetos se han actualizado
    void LateUpdate()
    {
        if (player != null)
        {
            // Actualizar la posición de la cámara con el desplazamiento (en 2D, solo X e Y importan)
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
        }
    }
}
