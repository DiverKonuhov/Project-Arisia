using UnityEngine;
using UnityEngine.UI;

public class SupportManager : MonoBehaviour
{
    public GameObject supportPanel; // Панель поддержки
    public InputField issueInputField; // Поле для ввода текста
    public string supportEmail = "ваша_почта@example.com"; // Замените на свою почту

    private void Start()
    {
        supportPanel.SetActive(false);
    }

    public void ToggleSupportPanel()
    {
        supportPanel.SetActive(!supportPanel.activeSelf);
    }

    public void SendEmail()
    {
        string subject = "Проблема в Project Arisia";
        string body = issueInputField.text;
        
        // Форматирование URL для почты
        string emailUrl = $"mailto:{supportEmail}?subject={subject}&body={body}";
        Application.OpenURL(emailUrl);
        
        supportPanel.SetActive(false); // Закрыть панель после отправки
    }
}