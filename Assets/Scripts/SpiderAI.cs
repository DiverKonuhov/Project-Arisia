using UnityEngine;
using UnityEngine.AI;

public class SpiderAI : MonoBehaviour
{
    public Transform target; // Перетащи игрока в это поле в инспекторе!
    private NavMeshAgent agent;
    private bool isAttacking;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (target == null) return;

        // Простейший ИИ:
        if (Vector3.Distance(transform.position, target.position) > 1.5f)
        {
            agent.SetDestination(target.position); // Преследование
            isAttacking = false;
        }
        else if (!isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    System.Collections.IEnumerator Attack()
    {
        isAttacking = true;
        target.GetComponent<PlayerHealth>().TakeDamage(10);
        yield return new WaitForSeconds(1.5f); // КД атаки
        isAttacking = false;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(10 * Time.deltaTime);
        }
    }
}