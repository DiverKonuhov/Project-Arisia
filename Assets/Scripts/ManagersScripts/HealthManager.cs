using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Добавьте эту строку для работы с Slider

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    [System.Serializable]
    public class HealthData
    {
        public string id; // "player", "spider", "boss"
        public int maxHealth;
        [HideInInspector] public int currentHealth;
        public Slider healthBar; // Теперь будет работать
        public UnityEvent onDeathEvent;
    }

    [Header("Health Settings")]
    public HealthData[] healthSettings;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeHealth();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeHealth()
    {
        foreach (var setting in healthSettings)
        {
            setting.currentHealth = setting.maxHealth;
            UpdateHealthUI(setting);
        }
    }

    public void TakeDamage(string targetId, int damage)
    {
        HealthData target = System.Array.Find(healthSettings, x => x.id == targetId);
        if (target == null) return;

        target.currentHealth = Mathf.Max(0, target.currentHealth - damage);
        UpdateHealthUI(target);

        if (target.currentHealth <= 0)
        {
            target.onDeathEvent.Invoke();
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
        return System.Array.Find(healthSettings, x => x.id == id);
    }
}