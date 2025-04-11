using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private RectTransform pauseBoard; // Ваша панель с кнопками
    [SerializeField] private Button pauseButton; // Кнопка в углу экрана
    [SerializeField] private float moveSpeed = 5f; // Скорость анимации

    private Vector2 hiddenPosition; // Стартовая позиция
    private Vector2 targetPosition; // Целевая позиция
    private bool isPaused;

    void Start()
    {
        // Запоминаем начальную позицию
        hiddenPosition = pauseBoard.anchoredPosition;

        // Рассчитываем позицию при паузе
        targetPosition = hiddenPosition + new Vector2(0, 600f);

        // Настраиваем кнопку
        pauseButton.onClick.AddListener(TogglePause);
    }

    void Update()
    {
        // Проверяем нажатие клавиши Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        StartCoroutine(MovePanel());
    }

    IEnumerator MovePanel()
    {
        Vector2 startPos = pauseBoard.anchoredPosition;
        Vector2 endPos = isPaused ? targetPosition : hiddenPosition;

        float progress = 0;

        while (progress < 1f)
        {
            progress += Time.deltaTime * moveSpeed;
            pauseBoard.anchoredPosition = Vector2.Lerp(startPos, endPos, progress);
            yield return null;
        }
    }
}
