// MeleeCombat.cs
using UnityEngine;

public class MeleeCombat : MonoBehaviour
{
    public Transform weaponTransform;
    public float attackRange = 2f;
    public int damage = 20;
    public float attackCooldown = 1f;
    public LayerMask enemyLayer;
    
    private float lastAttackTime;
    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }
    
    void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
        
        Collider[] hitEnemies = Physics.OverlapSphere(weaponTransform.position, attackRange, enemyLayer);
        
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.TryGetComponent<SpiderAI>(out SpiderAI spider))
            {
                spider.TakeDamage(damage);
                Debug.Log($"Hit spider! Damage: {damage}");
            }
        }
    }
    
    void OnDrawGizmosSelected()
    {
        if (weaponTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(weaponTransform.position, attackRange);
        }
    }
}