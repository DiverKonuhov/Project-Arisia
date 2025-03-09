using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Camera playerCamera;
    public float shootRange = 50f;
    public int shootDamage = 20;
    public float fireRate = 0.5f;
    public ParticleSystem muzzleFlash;

    private float lastFireTime;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= lastFireTime + fireRate)
        {
            Shoot();
            lastFireTime = Time.time;
        }
    }

    void Shoot()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, shootRange))
        {
            SpiderAI spider = hit.collider.GetComponent<SpiderAI>();
            if (spider != null)
            {
                spider.TakeDamage(shootDamage);
                Debug.Log($"Hit Spider! Remaining health: {spider.currentHealth}");
            }
        }
    }
}
