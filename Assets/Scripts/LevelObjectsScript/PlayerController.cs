using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Скорость движения персонажа

    private CharacterController controller;
    
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