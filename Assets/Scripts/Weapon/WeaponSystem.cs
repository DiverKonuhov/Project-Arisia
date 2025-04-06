using UnityEngine;

public class SpiderWeapon : MonoBehaviour
{
    public int damage = 10;
    public float cooldown = 2f;
    private float lastHitTime;

    void OnTriggerStay(Collider other)
    {
        if (Time.time - lastHitTime < cooldown) return;

        if (other.CompareTag("Player"))
        {
            lastHitTime = Time.time;
            DamageManager.Instance.ApplyDamage("player", "spider_melee", damage);
        }
    }
}