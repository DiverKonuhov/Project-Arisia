using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleDoor : MonoBehaviour
{
    public string NextSceneName; // Имя сцены (укажешь в Unity)
    public GameObject Player;    // Перетащи сюда игрока в инспекторе!

    private void OnTriggerEnter(Collider other) 
    {
        // Проверяем, что вошёл именно игрок (без тегов!)
        if (other.gameObject == Player)
        {
            SceneManager.LoadScene(NextSceneName);
        }
    }
}