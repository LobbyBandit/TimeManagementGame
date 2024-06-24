using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_WorldTime : MonoBehaviour
{
    TextMeshProUGUI textMeshProUGUI;
    private void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        textMeshProUGUI.text = new string((24 - TimeContainer.convertedMinutes).ToString("00") + ":" + (60 - TimeContainer.convertedSeconds).ToString("00"));
    }
}
