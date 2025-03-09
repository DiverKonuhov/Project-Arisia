using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SpiderAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 1.5f;
    public int damage = 10;
    public float attackCooldown = 2f;
    public float patrolRadius = 20f;
    public float patrolTime = 10f;

    private NavMeshAgent agent;
    private float lastAttackTime;
    private float patrolTimer;
    private Vector3 spawnPoint;
    private bool isChasing;

    public int maxHealth = 50;
    public int currentHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spawnPoint = transform.position;
        patrolTimer = patrolTime;
        currentHealth = maxHealth;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            agent.SetDestination(player.position);

            if (distanceToPlayer <= attackRange && Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
        else
        {
            isChasing = false;
            Patrol();
        }

        UpdateAgentSpeed();
    }

    void Patrol()
    {
        patrolTimer += Time.deltaTime;
        if (patrolTimer >= patrolTime)
        {
            Vector3 randomPoint = spawnPoint + Random.insideUnitSphere * patrolRadius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, patrolRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            patrolTimer = 0;
        }
    }

    void Attack()
    {
        if (player.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
            Debug.Log($"Spider attacked player! Remaining health: {playerHealth.currentHealth}");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        Debug.Log($"Spider took {damage} damage! Health: {currentHealth}");

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        Debug.Log("Spider died!");
        Destroy(gameObject);
    }

    void UpdateAgentSpeed()
    {
        agent.speed = isChasing ? 3.5f : 1.5f;
        agent.acceleration = isChasing ? 8 : 3;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(spawnPoint, patrolRadius);
    }
}
