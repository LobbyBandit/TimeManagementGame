using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureIndicator : MonoBehaviour
{
    Lock _lock;

   public GameObject minPressureIndicator;
   public GameObject maxPressureIndicator;
   public GameObject currentPressureIndicator;

    Vector3 CurrentPressureRotation;
    Vector3 minPressureRotation;
    Vector3 maxPressureRotation;

    Vector3 absoluteMaximumRotation = new Vector3(0,0,-45);
    Vector3 absoluteMinimumRotation = new Vector3(0, 0, 90);

    // Start is called before the first frame update
    void Start()
    {
        _lock = GetComponentInParent<Lock>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateIndicatorValues();

        UpdateIndicators();


    }

    void CalculateIndicatorValues()
    {
        minPressureRotation.z = Mathf.Lerp(absoluteMinimumRotation.z, absoluteMaximumRotation.z, _lock.minimumPressure / 100);
        maxPressureRotation.z = Mathf.Lerp(absoluteMinimumRotation.z, absoluteMaximumRotation.z, _lock.maximumPressure / 100);
        CurrentPressureRotation.z = Mathf.Lerp(absoluteMinimumRotation.z, absoluteMaximumRotation.z, _lock.pressure / 100);
    }

    void UpdateIndicators()
    {
        minPressureIndicator.transform.localRotation = Quaternion.Euler(minPressureRotation);
        maxPressureIndicator.transform.localRotation = Quaternion.Euler(maxPressureRotation);

        currentPressureIndicator.transform.localRotation = Quaternion.Euler(CurrentPressureRotation);
    }

}
