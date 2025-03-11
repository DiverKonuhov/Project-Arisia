using UnityEngine;
using UnityEngine.SceneManagement; // Для смены сцен
using UnityEngine.UI; // Для UI-элементов

public class DoorInteraction : MonoBehaviour
{
    public string sceneToLoad; // Название сцены для загрузки
    public GameObject interactionUI; // UI-объект, например, текст "Нажмите E"

    private bool isPlayerNear = false;

    void Start()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false); // Скрываем UI при старте
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(sceneToLoad); // Загружаем сцену
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Проверяем, что вошел игрок
        {
            isPlayerNear = true;
            if (interactionUI != null)
                interactionUI.SetActive(true); // Показываем UI
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Если игрок уходит
        {
            isPlayerNear = false;
            if (interactionUI != null)
                interactionUI.SetActive(false); // Скрываем UI
        }
    }
}
