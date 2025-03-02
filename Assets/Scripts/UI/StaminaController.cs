using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
public class StaminaController : MonoBehaviour
{
    public UnityEngine.UI.Image stamina_bar;
    void Update()
    {
        stamina_bar.fillAmount = (float)GetComponent<PlayerStats>().stamina /100f;
    }
}
