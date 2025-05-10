using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DescriptionManager : MonoBehaviour
{
    public GameObject descriptionPanel; // Панель с ScrollView
    public Text descriptionText; // Текстовый элемент в ScrollView

    private void Start()
    {
        descriptionPanel.SetActive(false); // Скрыть панель при старте
        LoadDescription();
    }

    void LoadDescription()
    {
        // Загрузка текста из файла в Resources
        TextAsset textAsset = Resources.Load<TextAsset>("UpdateDescription");
        if (textAsset != null)
        {
            descriptionText.text = textAsset.text;
        }
        else
        {
            descriptionText.text = "Файл не найден!";
        }
    }

    public void ToggleDescription()
    {
        descriptionPanel.SetActive(!descriptionPanel.activeSelf);
    }
}