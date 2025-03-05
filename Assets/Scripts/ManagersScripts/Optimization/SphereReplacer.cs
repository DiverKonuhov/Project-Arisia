using UnityEngine;  

public class SphereReplacer : MonoBehaviour  
{  
    private GameObject _decor;  
    private Camera _playerCamera;  
    [SerializeField] private float _maxDistance = 50f;  

    void Start()  
    {  
        _playerCamera = Camera.main;  
        _decor = transform.GetChild(0).gameObject;  
    }  

    void Update()  
    {  
        Vector3 directionToPlayer = _playerCamera.transform.position - transform.position;  
        float angle = Vector3.Angle(_playerCamera.transform.forward, directionToPlayer);  

        if (angle < 15f && Vector3.Distance(_playerCamera.transform.position, transform.position) < _maxDistance)  
        {  
            gameObject.SetActive(false);  
            _decor.SetActive(true);  
        }  
        else  
        {  
            gameObject.SetActive(true);  
            _decor.SetActive(false);  
        }  
    }  
}  