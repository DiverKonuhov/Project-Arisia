using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public PlayerWeapon weapon;
    public Animator animator;
    public string attackTrigger = "Attack";
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ЛКМ
        {
            Attack();
        }
    }
    
    void Attack()
    {
        // Анимация
        if (animator != null)
        {
            animator.SetTrigger(attackTrigger);
        }
        
        // Логика атаки
        weapon.TryAttack();
    }
}