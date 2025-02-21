using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 2.0f; // Чувствительность мыши
    public float maxYAngle = 80.0f; // Максимальный угол вращения по вертикали

    private float rotationX = 0.0f;

    public Transform player; // Ссылка на объект игрока
    public Vector3 thirdPersonOffset = new Vector3(0, 2, -5); // Смещение для третьего лица
    public Vector3 firstPersonOffset = new Vector3(0, 1.5f, 0); // Смещение для первого лица

    private void LateUpdate()
    {
        // Получаем ввод от мыши
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Вращаем персонажа в горизонтальной плоскости
        player.Rotate(Vector3.up * mouseX * sensitivity);

        // Вращаем камеру в вертикальной плоскости
        rotationX -= mouseY * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -maxYAngle, maxYAngle);

        // Устанавливаем позицию камеры в зависимости от режима
        if (player.GetComponent<PlayerController>().cameraMode == PlayerController.CameraMode.FirstPerson)
        {
            transform.localPosition = firstPersonOffset;
            transform.localRotation = Quaternion.Euler(rotationX, player.eulerAngles.y, 0);
        }
        else if (player.GetComponent<PlayerController>().cameraMode == PlayerController.CameraMode.ThirdPerson)
        {
            transform.position = player.position + player.TransformDirection(thirdPersonOffset);
            transform.LookAt(player.position + Vector3.up * 1.5f); // Смотрим на игрока с небольшим смещением вверх
            transform.localRotation = Quaternion.Euler(rotationX, player.eulerAngles.y, 0);
        }
    }
}
