using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public static WeaponSystem Instance;

    [System.Serializable]
    public class WeaponConfig
    {
        public string weaponId;
        public int baseDamage;
        public string damageType;
        public float cooldown;
    }

    [Header("Weapon Configurations")]
    public WeaponConfig[] weaponConfigs = {
        new WeaponConfig { weaponId = "spider_melee", baseDamage = 10, damageType = "melee", cooldown = 2f },
        new WeaponConfig { weaponId = "player_sword", baseDamage = 15, damageType = "melee", cooldown = 0.5f }
    };

    void Awake()
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

    public void ApplyDamage(string weaponId, string targetId, int baseDamage)
    {
        Debug.Log($"Пытаемся нанести урон: {weaponId} -> {targetId}");
        
        WeaponConfig config = System.Array.Find(weaponConfigs, x => x.weaponId == weaponId);
        if (config == null)
        {
            Debug.LogError($"Оружие {weaponId} не найдено!");
            return;
        }

        HealthManager.Instance.TakeDamage(targetId, baseDamage);
        Debug.Log($"Урон нанесен: {baseDamage}");
    }

    private int CalculateFinalDamage(WeaponConfig config, string targetId, int baseDamage)
    {
        // Здесь можно добавить логику модификаторов урона
        return baseDamage;
    }
}