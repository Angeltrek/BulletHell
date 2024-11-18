using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Image[] hearts; // Arreglo de imágenes de corazones
    public Sprite fullHeart; // Sprite para un corazón lleno
    public Sprite halfHeart; // Sprite para un medio corazón
    public Sprite emptyHeart; // Sprite para un corazón vacío

    public void UpdateHearts(int currentHealth, int maxHealth)
    {
        int totalHearts = hearts.Length;
        float healthPerHeart = maxHealth / (float)totalHearts;

        for (int i = 0; i < totalHearts; i++)
        {
            float heartHealth = Mathf.Clamp(currentHealth - (i * healthPerHeart), 0, healthPerHeart);

            if (heartHealth >= healthPerHeart)
            {
                hearts[i].sprite = fullHeart;
            }
            else if (heartHealth > 0)
            {
                hearts[i].sprite = halfHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}
