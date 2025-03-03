using UnityEngine;
using UnityEngine.UI; // Не забудьте добавить это пространство имен

public class BattlefieldPlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float stamina = 100f;
    public float staminaDrainRate = 10f; 
    public float staminaRecoveryRate = 5f; 
    public float minStaminaToRun = 10f; 

    private CharacterController characterController;
    private float currentStamina;
    private Vector3 velocity;
    private bool isGrounded;

    // Добавьте ссылку на Slider
    public Slider staminaSlider;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentStamina = stamina; 
        UpdateStaminaUI(); // Обновляем UI при старте
    }

    void Update()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f; // Сбрасываем вертикальную скорость при соприкосновении с землей
        }

        MovePlayer();
        RecoverStamina();
        UpdateStaminaUI(); // Обновляем UI в каждом кадре

        // Применяем гравитацию
        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void MovePlayer()
    {
        float currentSpeed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > minStaminaToRun)
        {
            currentSpeed = runSpeed;
            DrainStamina();
        }

        float moveHorizontal = Input.GetAxis("Horizontal") * currentSpeed;
        float moveVertical = Input.GetAxis("Vertical") * currentSpeed;

        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;

        characterController.Move(move * Time.deltaTime);
    }

    private void DrainStamina()
    {
        currentStamina -= staminaDrainRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, stamina); 
    }

    private void RecoverStamina()
    {
        if (currentStamina < stamina)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, stamina); 
        }
    }

    // Метод для обновления UI
    private void UpdateStaminaUI()
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina; // Обновляем значение слайдера
        }
    }
}
