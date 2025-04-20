using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mainmenu : MonoBehaviour
{
    [SerializeField] private GameObject PanelSettng;
    [SerializeField] private GameObject PanelCreate;
    private bool IsSetting;
    private bool IsCrteate;

    private void Update()
    {
        PanelSettng.SetActive(IsSetting);
        PanelCreate.SetActive(IsCrteate);

    }


    public void setting()
    {

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

