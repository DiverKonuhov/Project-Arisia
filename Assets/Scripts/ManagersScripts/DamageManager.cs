using UnityEngine;
using System.Collections.Generic;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance;

    [System.Serializable]
    public class DamageType
    {
        public string id; // "melee", "fire", "poison"
        public float multiplier = 1f; // Модификатор урона
        public Color damageColor = Color.red; // Для визуальных эффектов
    }

    [System.Serializable]
    public class EntityDamageSettings
    {
        public string entityId; // Соответствует ID из HealthManager
        public List<string> vulnerableTo; // Типы урона, к которым уязвим
        public List<string> resistantTo; // Типы урона, к которым устойчив
        public List<string> immuneTo; // Типы урона, которые игнорируются
    }

    [Header("Настройки типов урона")]
    public DamageType[] damageTypes;

    [Header("Настройки сущностей")]
    public EntityDamageSettings[] entitiesSettings;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ApplyDamage(string targetId, string damageTypeId, int baseDamage)
    {
        // 1. Находим настройки сущности
        EntityDamageSettings entitySettings = System.Array.Find(entitiesSettings, x => x.entityId == targetId);
        if (entitySettings == null)
        {
            Debug.LogError($"Entity with id {targetId} not found!");
            return;
        }

        // 2. Проверяем иммунитеты
        if (entitySettings.immuneTo.Contains(damageTypeId))
        {
            Debug.Log($"{targetId} immune to {damageTypeId} damage!");
            return;
        }

        // 3. Рассчитываем итоговый урон
        DamageType dmgType = System.Array.Find(damageTypes, x => x.id == damageTypeId);
        float finalDamage = baseDamage;

        if (entitySettings.resistantTo.Contains(damageTypeId))
            finalDamage *= 0.5f; // Устойчивость = 50% урона

        if (entitySettings.vulnerableTo.Contains(damageTypeId))
            finalDamage *= 1.5f; // Уязвимость = 150% урона

        finalDamage *= dmgType.multiplier;

        // 4. Применяем урон через HealthManager
        HealthManager.Instance.TakeDamage(targetId, Mathf.RoundToInt(finalDamage));

        // 5. Визуальный эффект (опционально)
        ShowDamageEffect(targetId, dmgType.damageColor);
    }

    private void ShowDamageEffect(string targetId, Color color)
    {
        // Здесь можно добавить логику визуальных эффектов
        Debug.Log($"Applied damage to {targetId} with color {color}");
    }
}