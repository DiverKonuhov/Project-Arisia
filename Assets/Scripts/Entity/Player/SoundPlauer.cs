using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlauer: MonoBehaviour
{
    [SerializeField] private AudioClip[] woodClips; // Звуки для деревянной поверхности
    [SerializeField] private AudioClip[] grassClips; // Звуки для травы
    [SerializeField] private AudioClip[] GravelClips; // Звуки для бетона
    [SerializeField] private AudioClip[] StrongClips; // Звуки для металла
    [SerializeField] private float stepInterval = 0.5f; // Интервал между шагами
    public AudioClip clip;

    private AudioSource audioSource;
    private CharacterController characterController;
    private float stepTimer;
    public bool isGrounded;
    public Ray ray;

    // Имена слоев (должны быть настроены в Unity)
    private int woodLayer;
    private int grassLayer;
    private int concreteLayer;
    private int metalLayer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
        stepTimer = 0f;

        // Получаем индексы слоев
        woodLayer = LayerMask.NameToLayer("Wood");
        grassLayer = LayerMask.NameToLayer("Grass");
        concreteLayer = LayerMask.NameToLayer("Gravel");
        metalLayer = LayerMask.NameToLayer("Strong");
    }

    void Update()
    {
        bool isMoving = Input.GetAxis("Vertical") !=0 || Input.GetAxis("Horizontal") != 0;
        // Проверка, находится ли персонаж на земле и движется ли
        isGrounded = characterController.isGrounded;
         ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(ray.origin,ray.direction*20,Color.blue);

        if (isGrounded && isMoving)
        {

            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                FastMove();

                PlayFootstepSound();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = stepInterval; // Сброс таймера
        }
    }
     
    void FastMove()
    {
        if(PlayerController.isRunning)
        {

            audioSource.pitch = 2;
            stepInterval = 0.2f;
        }
        else
        {
            audioSource.pitch = 1;
            stepInterval = 0.5f;
        }
    }
    void PlayFootstepSound()
    {
        // Бросаем луч вниз, чтобы определить поверхность
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            // Проверка слоя поверхности
            int hitLayer = hit.collider.gameObject.layer;
            if (hitLayer == woodLayer)
            {
                clip = woodClips[Random.Range(0, woodClips.Length)];

            }
            else if (hitLayer == grassLayer)
            {
                clip = grassClips[Random.Range(0, grassClips.Length)];


            }
            else if (hitLayer == concreteLayer)
            {
                clip = StrongClips[Random.Range(0, StrongClips.Length)];

            }
            else if (hitLayer == metalLayer)
            {
                clip = GravelClips[Random.Range(0, GravelClips.Length)];

            }
            else
            {
                clip = StrongClips[Random.Range(0, StrongClips.Length)]; // По умолчанию

            }

            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }

}
