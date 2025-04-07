using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    
    private Vector3 _targetPosition;
    private string _targetScene;

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
        if (scene.name == _targetScene)
        {
            transform.position = _targetPosition;
            Debug.Log($"Игрок перенесён в {scene.name} на позицию: {_targetPosition}");
        }
    }

    public void SetTeleportData(Vector3 position, string sceneName)
    {
        _targetPosition = position;
        _targetScene = sceneName;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}