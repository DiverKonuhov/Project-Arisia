using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensitivity = 2.0f; // Чувствительность мыши
    [SerializeField] private float maxYAngle = 80.0f; // Максимальный угол вращения по вертикали
    [SerializeField] private float fovSensitivity = 5f; // Чувствительность для изменения поля зрения
    [SerializeField] private Camera playerCamera; // Ссылка на камеру игрока
    [SerializeField] private float sideTiltAmount = 2.0f; // Наклон камеры при боковом движении
    [SerializeField] private float runTiltAmount = 4.0f; // Увеличенный наклон при беге

    private float rotationX = 0.0f;
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponentInParent<CharacterController>();
        playerCamera.fieldOfView = 60; // Устанавливаем начальное поле зрения
    }

    private void Update()
    {
        // Проверяем, удерживается ли клавиша Alt
        bool isAltHeld = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);

        if (!isAltHeld)
        {
            // Получаем ввод от мыши только если Alt не удерживается
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Вращаем персонажа в горизонтальной плоскости
            transform.parent.Rotate(Vector3.up * mouseX * sensitivity);

            // Вращаем камеру в вертикальной плоскости
            rotationX -= mouseY * sensitivity;
            rotationX = Mathf.Clamp(rotationX, -maxYAngle, maxYAngle);
            transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
        }

        // Изменяем поле зрения в зависимости от движения
        UpdateFieldOfView();

        // Динамика наклона камеры при движении
        UpdateCameraTilt();
    }

    private void UpdateFieldOfView()
    {
        if (Input.GetKey(KeyCode.LeftShift)) // Если игрок бежит
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, 70, Time.deltaTime * fovSensitivity); // Увеличиваем FOV
        }
        else
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, 60, Time.deltaTime * fovSensitivity); // Возвращаем FOV к норме
        }
    }

    private void UpdateCameraTilt()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        if (moveDirection.magnitude > 0)
        {
            // Наклон камеры при боковом движении
            float tiltAmount = (Input.GetKey(KeyCode.LeftShift) ? runTiltAmount : sideTiltAmount) * Input.GetAxis("Horizontal");
            transform.localRotation *= Quaternion.Euler(0, 0, -tiltAmount);
        }
    }
}
