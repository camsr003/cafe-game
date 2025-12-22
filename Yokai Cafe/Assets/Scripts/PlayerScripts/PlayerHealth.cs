using UnityEngine;
using System; // Required for events

public class HealthSystem : MonoBehaviour
{
    public GameObject gameOverMenu;
    public float maxHealth = 100f;
    private float currentHealth;
    public event Action<float> OnHealthChanged; // Event to notify UI or other systems

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Public method to modify health from other scripts 
    public void ChangeHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); // Clamp health between 0 and maxHealth

        OnHealthChanged?.Invoke(currentHealth); // Trigger the event

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died!");
        // Game over screen
        // gameOverMenu.SetActive(true);
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;
    }
}
