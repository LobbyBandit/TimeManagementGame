using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, 360, 1 - TimeContainer.convertedMinutesInSeconds / 24));
    }
}
