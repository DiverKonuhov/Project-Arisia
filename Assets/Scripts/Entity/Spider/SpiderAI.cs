using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class SpiderAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 3.5f;
    public float patrolRadius = 10f;
    public float detectionRange = 8f;

    [Header("Attack Settings")]
    public float attackCooldown = 2f;
    public float attackRange = 1.5f;
    public SpiderWeapon spiderWeapon;

    [Header("Health Settings")]
    public string spiderId = "spider";
    public Slider healthBar;

    [Header("Effects")]
    public GameObject deathEffect;

    private NavMeshAgent agent;
    private Transform player;
    private float lastAttackTime;
    private Vector3 spawnPoint;
    private bool isDead;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spiderWeapon = GetComponentInChildren<SpiderWeapon>();
        spawnPoint = transform.position;
        player = GameObject.FindWithTag("Player")?.transform; // Добавлена проверка null
        
        if (agent != null)
        {
            agent.speed = patrolSpeed;
        }

        // Защищенная инициализация здоровья
        if (HealthManager.Instance != null)
        {
            HealthManager.Instance.RegisterHealth(spiderId, healthBar);
            var healthData = HealthManager.Instance.GetHealthData(spiderId);
            if (healthData != null)
            {
                healthData.onDeathEvent.AddListener(Die);
            }
            else
            {
                Debug.LogError($"Failed to get health data for {spiderId}");
            }
        }
        else
        {
            Debug.LogError("HealthManager.Instance is null!");
        }

        SetRandomDestination();
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);

            if (distanceToPlayer <= attackRange && Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;
                spiderWeapon.Attack();
            }
        }
        else if (agent.remainingDistance < 0.5f)
        {
            SetRandomDestination();
        }
    }

    private void SetRandomDestination()
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
        
        HealthManager.Instance.TakeDamage(spiderId, damageAmount);
        
        // Исправленный вызов метода
        var healthData = HealthManager.Instance.GetHealthData(spiderId);
        if (healthData != null && healthData.currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead || this == null) return;
        
        isDead = true;
        
        if (agent != null)
        {
            agent.isStopped = true;
        }

        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject, 0.5f);
    }
}