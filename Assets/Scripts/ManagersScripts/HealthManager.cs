using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    [System.Serializable]
    public class HealthData
    {
        public string id;
        public int maxHealth = 100;
        [HideInInspector] public int currentHealth;
        public Slider healthBar;
        public UnityEvent onDeathEvent;
    }

    [Header("Base Health Settings")]
    public HealthData[] baseHealthSettings;

    private Dictionary<string, HealthData> healthRegistry = new Dictionary<string, HealthData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeBaseHealth();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void InitializeBaseHealth()
    {
        foreach (var setting in baseHealthSettings)
        {
            RegisterHealth(setting.id, setting.healthBar, setting.maxHealth);
            
            // Здесь можно настроить стандартные события смерти
            if (setting.id.Contains("spider"))
            {
                setting.onDeathEvent = new UnityEvent();
            }
        }
    }

public void RegisterHealth(string id, Slider healthBar = null, int maxHealth = 100)
{
    if (healthRegistry == null)
    {
        healthRegistry = new Dictionary<string, HealthData>();
    }

    if (!healthRegistry.ContainsKey(id))
    {
        var newData = new HealthData()
        {
            id = id,
            maxHealth = maxHealth,
            currentHealth = maxHealth,
            healthBar = healthBar
        };
        
        healthRegistry[id] = newData;
        UpdateHealthUI(newData);
        
        // Автоматически добавляем событие смерти для пауков
        if (id.Contains("spider"))
        {
            newData.onDeathEvent = new UnityEvent();
        }
    }
}

    public void TakeDamage(string targetId, int damage)
    {
        if (!healthRegistry.ContainsKey(targetId))
        {
            Debug.LogWarning($"Health ID {targetId} not registered!");
            return;
        }

        HealthData target = healthRegistry[targetId];
        target.currentHealth = Mathf.Max(0, target.currentHealth - damage);
        UpdateHealthUI(target);

        if (target.currentHealth <= 0)
        {
            target.onDeathEvent?.Invoke();
        }
    }

    private void UpdateHealthUI(HealthData data)
    {
        if (data.healthBar != null)
        {
            data.healthBar.maxValue = data.maxHealth;
            data.healthBar.value = data.currentHealth;
        }
    }

    public HealthData GetHealthData(string id)
    {
        return healthRegistry.ContainsKey(id) ? healthRegistry[id] : null;
    }
    
}