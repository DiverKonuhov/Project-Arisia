using UnityEngine;

public class SpiderWeapon : MonoBehaviour
{
    public int damage = 10;
    public float cooldown = 2f;
    private float lastHitTime;

    public void Attack()
    {
        // Метод для вызова из SpiderAI
    }

    void OnTriggerStay(Collider other)
    {
        if (Time.time - lastHitTime < cooldown) return;

        if (other.CompareTag("Player"))
        {
            lastHitTime = Time.time;
            HealthManager.Instance.TakeDamage("player", damage);
        }
    }
}