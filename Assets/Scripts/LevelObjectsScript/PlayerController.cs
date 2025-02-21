using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Скорость движения персонажа

    private CharacterController controller;

    // Переменная для хранения режима камеры
    public CameraMode cameraMode = CameraMode.ThirdPerson;

    public enum CameraMode
    {
        FirstPerson,
        ThirdPerson
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Управление курсором
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

        // Переключение между режимами камеры
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // Прокрутка вверх
        {
            cameraMode = CameraMode.FirstPerson;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // Прокрутка вниз
        {
            cameraMode = CameraMode.ThirdPerson;
        }

        // Получаем ввод от игрока
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Вычисляем направление движения
        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        // Применяем гравитацию
        moveDirection.y -= 9.81f * Time.deltaTime;

        // Двигаем персонажа
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}
