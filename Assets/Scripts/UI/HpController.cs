using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
public class HpController : MonoBehaviour
{
    public UnityEngine.UI.Image hp_bar;
    void Update()
    {
        hp_bar.fillAmount = (float)GetComponent<PlayerStats>().health /100f;
    }
}
