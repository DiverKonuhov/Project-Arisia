// PlayerHealth.cs
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthBar;
    
    private int currentHealth;
    
    void Start()
    {
        currentHealth = HealthManager.Instance.playerMaxHealth;
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
            healthBar.value = (float)currentHealth / HealthManager.Instance.playerMaxHealth;
    }
    
    void Die()
    {
        Debug.Log("Player died!");
        HealthManager.Instance.PlayerDied();
    }
    
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}