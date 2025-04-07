using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDoor : MonoBehaviour
{
    [System.Serializable]
    public class SceneDestination
    {
        public string sceneName;
        public Vector3 spawnPosition;
    }

    [Header("Настройки перехода")]
    public SceneDestination destination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerManager.Instance?.gameObject)
        {
            PlayerManager.Instance.SetTeleportData(
                destination.spawnPosition, 
                destination.sceneName
            );
            
            SceneManager.LoadScene(destination.sceneName);
        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        
        if (!string.IsNullOrEmpty(destination.sceneName))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(destination.spawnPosition, Vector3.one * 0.5f);
            Gizmos.DrawLine(transform.position, destination.spawnPosition);
        }
    }
    #endif
}