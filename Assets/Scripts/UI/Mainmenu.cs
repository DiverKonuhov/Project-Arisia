using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    [SerializeField] private GameObject PanelSettng;
    [SerializeField] private GameObject PanelCreate;
    private bool IsSetting;
    private bool IsCrteate;


    private AudioSource AudioSo;
    [SerializeField] private AudioClip SongClick;

    private void Start()
    {
        AudioSo = GetComponent<AudioSource>();

    }


    private void Update()
    {
        PanelSettng.SetActive(IsSetting);
        PanelCreate.SetActive(IsCrteate);

    }

    public void LoadSceneWithName(string sceneName)
    {
        AudioSo.PlayOneShot(SongClick);
        SceneManager.LoadScene(sceneName);
    }
    public void Exit()
    {
        AudioSo.PlayOneShot(SongClick);
        Application.Quit();
    }


    public void setting()
    {
        AudioSo.PlayOneShot(SongClick);

        if (!IsSetting)
        {
            IsSetting = true;
            IsCrteate = false;

        }
        else
       {

            IsSetting = false;
        }

    }

    public void create()
    {
        AudioSo.PlayOneShot(SongClick);

        if (!IsCrteate)
        {
            IsCrteate = true;
            IsSetting = false;

        }
        else
        {
            IsCrteate = false;
        }

    }
}

