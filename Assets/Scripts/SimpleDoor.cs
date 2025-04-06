using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleDoor : MonoBehaviour
{
    public string NextSceneName;
    
    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            // Сохраняем позицию игрока у этой двери
            PlayerManager.Instance.lastDoorPosition = transform.position;
            
            // Загружаем новую сцену
            SceneManager.LoadScene(NextSceneName);
        }
    }
}