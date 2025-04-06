using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SpiderAI : MonoBehaviour
{
    [Header("Movement")]
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 3.5f;
    public float patrolRadius = 10f;
    public float detectionRange = 8f;

    [Header("Attack")]
    public int damage = 10;
    public float attackCooldown = 2f;
    public float attackRange = 1.5f;

    [Header("References")]
    public Transform weaponPoint; // Точка крепления оружия
    public GameObject deathEffect;

    private NavMeshAgent agent;
    private Transform player;
    private float lastAttackTime;
    private Vector3 spawnPoint;
    private bool isDead;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spawnPoint = transform.position;
        player = GameObject.FindWithTag("Player").transform;

        agent.speed = patrolSpeed;
        SetRandomDestination();
    }

    void Update()
    {
        if (isDead) return;

        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Режим преследования
        if (distanceToPlayer <= detectionRange)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);

            if (distanceToPlayer <= attackRange && Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
            }
        }
        // Режим патрулирования
        else if (agent.remainingDistance < 0.5f)
        {
            SetRandomDestination();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        DamageManager.Instance.ApplyDamage("player", "melee", damage);
    }

    void SetRandomDestination()
    {
        Vector3 randomPoint = spawnPoint + Random.insideUnitSphere * patrolRadius;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        HealthManager.Instance.TakeDamage("spider", damageAmount);

        if (HealthManager.Instance.GetHealthData("spider").currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;

        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, 0.5f); // Задержка для проигрывания анимации
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}