using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public Vector3 lastDoorPosition; // Позиция у последней двери
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ищем дверь, соответствующую предыдущей сцене
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject door in doors)
        {
            SimpleDoor doorScript = door.GetComponent<SimpleDoor>();
            if (doorScript != null && doorScript.NextSceneName == SceneManager.GetActiveScene().name)
            {
                // Телепортируем игрока к двери
                transform.position = door.transform.position + door.transform.forward * 2f;
                break;
            }
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}