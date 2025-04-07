using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 2.4f;
    [SerializeField] private float runSpeed = 4.8f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;

    [Header("Stamina Settings")]
    [SerializeField] private float stamina = 10f;
    [SerializeField] private float staminaDrain = 1f;
    [SerializeField] private float staminaRegen = 0.5f;
    [SerializeField] private Slider staminaBar;

    [Header("Particle Settings")]
    //[SerializeField] private ParticleSystem runParticles;
    //[SerializeField] private RectTransform particleAnchor;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isRunning;
    private float lastStaminaUseTime;
    //private Vector3[] worldCorners = new Vector3[4];
    private Camera mainCamera;
    private Color originalStaminaColor;

    private void Start()
    {
        mainCamera = Camera.main;
        originalStaminaColor = staminaBar.fillRect.GetComponent<Image>().color;
        staminaBar.maxValue = stamina;
        staminaBar.value = stamina;

        /*if (runParticles != null)
        {
            var main = runParticles.main;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            runParticles.Stop();
        }*/
    }

    private void Update()
    {
        HandleCursor();
        HandleMovement();
        HandleStamina();
        //UpdateParticlesPosition();
        //HandleParticles();
        UpdateStaminaUI();
    }

    private void HandleCursor()
    {
        Cursor.lockState = Input.GetKey(KeyCode.LeftAlt) 
            ? CursorLockMode.None 
            : CursorLockMode.Locked;
        Cursor.visible = Input.GetKey(KeyCode.LeftAlt);
    }

    private void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        Vector3 move = transform.forward * Input.GetAxis("Vertical") 
                      + transform.right * Input.GetAxis("Horizontal");
        
        controller.Move(move * (isRunning ? runSpeed : speed) * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleStamina()
    {
        bool isMoving = Input.GetAxis("Vertical") != 0 
                     || Input.GetAxis("Horizontal") != 0;
        bool shouldDrain = Input.GetKey(KeyCode.LeftShift) && isMoving;

        if (shouldDrain && stamina > 0)
        {
            isRunning = true;
            stamina -= staminaDrain * Time.deltaTime;
            lastStaminaUseTime = Time.time;
        }
        else
        {
            isRunning = false;
            if (Time.time - lastStaminaUseTime >= 5f && stamina < 10)
                stamina += staminaRegen * Time.deltaTime;
        }

        stamina = Mathf.Clamp(stamina, 0, 10);
    }

    /*private void UpdateParticlesPosition()
    {
        if (particleAnchor == null || runParticles == null || mainCamera == null)
        {
            Debug.LogError($"Missing references: " +
                $"particleAnchor={particleAnchor}, " +
                $"runParticles={runParticles}, " +
                $"mainCamera={mainCamera}");
            return;
        }

        try
        {
            particleAnchor.GetWorldCorners(worldCorners);
            
            Vector3 centerPos = (worldCorners[0] + worldCorners[2]) * 0.5f;
            
            Vector3 screenPos = mainCamera.WorldToScreenPoint(centerPos);
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(
                new Vector3(
                    screenPos.x, 
                    screenPos.y, 
                    mainCamera.nearClipPlane + 0.5f
                )
            );
            
            runParticles.transform.position = worldPosition;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Particle position error: {e.Message}");
        }
    }*/

    /*private void HandleParticles()
    {
        if (runParticles == null) return;

        if (isRunning)
        {
            if (!runParticles.isPlaying) runParticles.Play();
        }
        else
        {
            if (runParticles.isPlaying) runParticles.Stop();
        }
    }*/

    private void UpdateStaminaUI()
    {
        staminaBar.value = stamina;
    }
}