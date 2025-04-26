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
        // Проверка, находится ли персонаж на земле и движется ли
        isGrounded = characterController.isGrounded;
        if (isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            PlayFootstepSound();

            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                PlayFootstepSound();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = stepInterval; // Сброс таймера
        }
    }

    void PlayFootstepSound()
    {
        // Бросаем луч вниз, чтобы определить поверхность
        RaycastHit hit;
        Debug.DrawLine(transform.position, Vector3.down,Color.blue, 1.5f);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
        {

            // Проверка слоя поверхности
            int hitLayer = hit.collider.gameObject.layer;
            if (hitLayer == woodLayer)
            {
                clip = woodClips[Random.Range(0, woodClips.Length)];
                audioSource.PlayOneShot(clip);

            }
            else if (hitLayer == grassLayer)
            {
                clip = grassClips[Random.Range(0, grassClips.Length)];
                audioSource.PlayOneShot(clip);

            }
            else if (hitLayer == concreteLayer)
            {
                clip = StrongClips[Random.Range(0, StrongClips.Length)];
                audioSource.PlayOneShot(clip);

            }
            else if (hitLayer == metalLayer)
            {
                clip = GravelClips[Random.Range(0, GravelClips.Length)];
                audioSource.PlayOneShot(clip);

            }
            else
            {
                clip = StrongClips[Random.Range(0, StrongClips.Length)]; // По умолчанию
                audioSource.PlayOneShot(clip);

            }

            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }

}
