// HealthManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;
    
    public int playerMaxHealth = 100;
    public int spiderMaxHealth = 50;
    
    [SerializeField] private string deathSceneName = "GameOver";
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void PlayerDied()
    {
        SceneManager.LoadScene(deathSceneName);
    }
}