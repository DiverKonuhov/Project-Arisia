using UnityEngine;
using System.Collections.Generic;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance;

    [System.Serializable]
    public class DamageType
    {
        public string id;
        public float multiplier = 1f;
        public Color damageColor = Color.red;
    }

    [System.Serializable]
    public class EntityDamageSettings
    {
        public string entityId;
        public List<string> vulnerableTo = new List<string>();
        public List<string> resistantTo = new List<string>();
        public List<string> immuneTo = new List<string>();
    }

    [Header("Damage Types")]
    public DamageType[] damageTypes = {
        new DamageType { id = "melee", multiplier = 1f, damageColor = Color.red },
        new DamageType { id = "fire", multiplier = 1.2f, damageColor = new Color(1, 0.5f, 0) },
        new DamageType { id = "poison", multiplier = 0.8f, damageColor = Color.green }
    };

    [Header("Entity Settings")]
    public EntityDamageSettings spiderSettings = new EntityDamageSettings 
    { 
        entityId = "spider",
        vulnerableTo = new List<string> { "fire" },
        resistantTo = new List<string> { "poison" }
    };

    public EntityDamageSettings playerSettings = new EntityDamageSettings
    {
        entityId = "player",
        vulnerableTo = new List<string> { "poison" }
    };

    private Dictionary<string, EntityDamageSettings> entitySettingsDict;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSettings()
    {
        entitySettingsDict = new Dictionary<string, EntityDamageSettings>
        {
            { spiderSettings.entityId, spiderSettings },
            { playerSettings.entityId, playerSettings }
        };
    }

    public void ApplyDamage(string targetId, string damageTypeId, int baseDamage)
    {
        if (!entitySettingsDict.TryGetValue(targetId, out EntityDamageSettings entitySettings))
        {
            Debug.LogError($"Entity settings for {targetId} not found!");
            return;
        }

        if (entitySettings.immuneTo.Contains(damageTypeId))
        {
            Debug.Log($"{targetId} is immune to {damageTypeId} damage");
            return;
        }

        DamageType dmgType = System.Array.Find(damageTypes, x => x.id == damageTypeId);
        if (dmgType == null)
        {
            Debug.LogError($"Damage type {damageTypeId} not found!");
            return;
        }

        float damageMultiplier = 1f;
        if (entitySettings.resistantTo.Contains(damageTypeId)) damageMultiplier *= 0.5f;
        if (entitySettings.vulnerableTo.Contains(damageTypeId)) damageMultiplier *= 1.5f;

        int finalDamage = Mathf.RoundToInt(baseDamage * dmgType.multiplier * damageMultiplier);
        HealthManager.Instance.TakeDamage(targetId, finalDamage);
        ShowDamageEffect(targetId, dmgType.damageColor);
    }

    private void ShowDamageEffect(string targetId, Color color)
    {
        // Реализуйте визуальные эффекты здесь
        Debug.Log($"Applied damage to {targetId} (Color: {color})");
    }
}