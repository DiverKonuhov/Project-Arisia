using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private RectTransform pauseBoard; // Ваша панель с кнопками
    [SerializeField] private Button pauseButton; // Кнопка в углу экрана
    [SerializeField] private float moveSpeed = 5f; // Скорость анимации

    [Header("аудио настройки")]
     private AudioSource AudioSo;
    [SerializeField] private AudioClip SongOpen;
    [SerializeField] private AudioClip SongClick;



    [Header("меню обьекты")]
    [SerializeField] private GameObject panelPauseMenu;
    [SerializeField] private GameObject panelSetting;

    private bool isPaused;
    private bool isOpenSetting;

    private Vector2 hiddenPosition; // Стартовая позиция
    private Vector2 targetPosition; // Целевая позиция
    [SerializeField] private GameObject Plauer;


    void Start()
    {
        AudioSo = GetComponent<AudioSource>(); 
        Cursor.visible = false;
        // Запоминаем начальную позицию
        hiddenPosition = pauseBoard.anchoredPosition;

        // Рассчитываем позицию при паузе
        targetPosition = hiddenPosition + new Vector2(0, 600f);
    }

    void Update()
    {
        panelSetting.SetActive(isOpenSetting);

        // Проверяем нажатие клавиши Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                AudioSo.PlayOneShot(SongOpen);
                panelPauseMenu.SetActive(isPaused);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
                MenuPanel();
            }
        }
       
    }
    #region панели пазуы

   public void SettingPanel()
    {
        AudioSo.PlayOneShot(SongClick);

        if (isOpenSetting)
        {
            isOpenSetting = false;
        }
        else
        {
            isOpenSetting = true;
        }
    }

   public void MenuPanel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        isOpenSetting = false;
        panelPauseMenu.SetActive(false);
    }

    public void ContinumBatton()
    {
        Time.timeScale = 1;
        MenuPanel();
        AudioSo.PlayOneShot(SongClick);
    }
    public void Exit()
    {
        SceneManager.GetSceneByBuildIndex(0);
        Destroy(Plauer);
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
    #endregion
}
