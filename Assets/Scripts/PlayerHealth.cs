using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar; // Сюда добавь Slider из UI

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        Debug.Log($"Player took {damage} damage! Health: {currentHealth}");
        UpdateHealthBar();

        if (currentHealth <= 0) Die();
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.value = (float)currentHealth / maxHealth;
    }

    void Die()
    {
        Debug.Log("Player died!");
        // Здесь можешь добавить свою логику смерти
    }
}
