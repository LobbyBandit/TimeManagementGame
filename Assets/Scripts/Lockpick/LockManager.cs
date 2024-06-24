using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockManager
{
    public static LockStats[] DifficultyArray;
    public static Vector3[] toothPositions;

    static LockManager()
    {
        // Initialize DifficultyArray and toothPositions here
        DifficultyArray = new LockStats[]
        {
            new LockStats(30, 80, 10, 3, 5),
            new LockStats(20, 60, 10, 4, 7),
            new LockStats(10, 50, 10, 5, 7)
        };

        //toothPositions
        toothPositions = new Vector3[]
        {
            new Vector3(-1.5f, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(-0.5f, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(0.5f, 0, 0),
            new Vector3(1f, 0, 0),
            new Vector3(1.5f, 0, 0)
        };
    }
}

public class LockStats
{

    public float _minPressure;
    public float _maxPressure;
    public float _PressureChangePerTooth;
    public int _minTeethAmount;
    public int _maxTeethAmount;
    public int _teethAmount;

    public LockStats(float minPressure, float maxPressure, float PressureChangePerTooth, int minTeethAmount, int maxTeethAmount)
    {
        _minPressure = minPressure;
        _maxPressure = maxPressure;
        _PressureChangePerTooth = PressureChangePerTooth;
        _minTeethAmount = minTeethAmount;
        _maxTeethAmount = maxTeethAmount;
        _teethAmount = Random.Range(minTeethAmount, maxTeethAmount + 1);
    }






}
