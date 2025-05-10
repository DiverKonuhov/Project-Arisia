using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public string weaponId = "player_sword";
    public int damage = 15;
    public float cooldown = 0.5f;
    public float attackRange = 2f;
    public float rayRadius = 0.3f;
    public LayerMask enemyLayer;

    [Header("Visuals")]
    public GameObject swingEffect;
    public GameObject hitEffect;
    public AudioClip swingSound;

    private float lastAttackTime;
    private AudioSource audioSource;
    private Collider weaponCollider;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        weaponCollider = GetComponent<Collider>();
        if (weaponCollider != null)
        {
            weaponCollider.isTrigger = true;
            weaponCollider.enabled = false;
        }
    }

    public void TryAttack()
    {
        if (Time.time - lastAttackTime < cooldown) return;

        lastAttackTime = Time.time;
        
        // Визуальные эффекты
        if (swingEffect != null) Instantiate(swingEffect, transform.position, transform.rotation);
        if (swingSound != null && audioSource != null) audioSource.PlayOneShot(swingSound);

        // Двойная проверка: Raycast + триггер
        bool raycastHit = CheckRaycast();
        bool triggerActive = weaponCollider != null;

        if (triggerActive)
        {
            weaponCollider.enabled = true;
            Invoke(nameof(DisableCollider), 0.2f);
        }

        Debug.Log($"Атака: Raycast={raycastHit}, Trigger={triggerActive}");
    }

    bool CheckRaycast()
    {
        RaycastHit hit;
        bool hasHit = Physics.SphereCast(
            transform.position,
            rayRadius,
            transform.forward,
            out hit,
            attackRange,
            enemyLayer
        );

        if (hasHit && hit.collider.CompareTag("Enemy"))
        {
            WeaponSystem.Instance.ApplyDamage(weaponId, "spider", damage);
            if (hitEffect != null) Instantiate(hitEffect, hit.point, Quaternion.identity);
            return true;
        }
        return false;
    }

    void DisableCollider()
    {
        if (weaponCollider != null)
            weaponCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Дополнительная проверка Raycast при триггере
            if (!CheckRaycast())
            {
                WeaponSystem.Instance.ApplyDamage(weaponId, "spider", damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rayRadius);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * attackRange);
    }
}