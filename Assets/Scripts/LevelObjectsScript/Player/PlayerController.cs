using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 2.4f; // Уменьшенная скорость обычной ходьбы
    [SerializeField] private float runSpeed = 4.8f; // Уменьшенная скорость бега
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;

    [SerializeField] private float stamina = 10f; // Уменьшенная максимальная выносливость
    [SerializeField] private float staminaDrain = 1f; // Уменьшенный расход выносливости при беге
    [SerializeField] private float staminaRegen = 0.5f; // Уменьшенное восстановление выносливости в секунду

    [SerializeField] private UnityEngine.UI.Image staminaBar; // Ссылка на UI элемент для отображения выносливости

    private Vector3 velocity;
    private bool isGrounded;
    private bool isRunning;

    private Color originalStaminaColor;
    private float alpha = 1f; // Альфа-канал для эффекта мигания

    private void Start()
    {
        originalStaminaColor = staminaBar.color; // Сохраняем оригинальный цвет
    }

    private void Update()
    {
        // Обработка скрытия/показ курсора
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Убедимся, что игрок "прилипает" к земле
        }

        // Движение вперед/назад
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal"); // Добавлено для бокового движения
        Vector3 move = transform.forward * moveZ + transform.right * moveX; // Теперь учитываем движение по X

        // Проверяем, удерживается ли Shift для бега
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            isRunning = true;
            stamina -= staminaDrain * Time.deltaTime; // Расходуем выносливость
            stamina = Mathf.Max(stamina, 0); // Не допускаем отрицательных значений
        }
        else
        {
            isRunning = false;
            if (stamina < 10)
            {
                StartCoroutine(StaminaBlink()); // Запускаем мигание, если выносливость низкая
            }
            if (stamina < 10)
            {
                stamina += staminaRegen * Time.deltaTime; // Восстанавливаем выносливость
            }
        }

        float currentSpeed = isRunning ? runSpeed : speed;
        controller.Move(move * currentSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Обновляем UI для выносливости
        UpdateStaminaUI();
    }

    private IEnumerator StaminaBlink()
    {
        while (stamina < 2)
        {
            alpha = Mathf.PingPong(Time.time, 1); // Создаем эффект мигания
            staminaBar.color = new Color(originalStaminaColor.r, originalStaminaColor.g, originalStaminaColor.b, alpha);
            yield return null; // Ждем следующего кадра
        }
        staminaBar.color = originalStaminaColor; // Возвращаем оригинальный цвет после завершения мигания
    }

    private void UpdateStaminaUI()
    {
        staminaBar.fillAmount = stamina / 10f; // Обновляем заполнение UI элемента
    }
}
