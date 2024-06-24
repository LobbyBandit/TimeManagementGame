using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;

public class Lock : MonoBehaviour
{
    enum Difficulty
    {
        Easy = 0,
        Medium = 1,
        Hard = 2,
    }

    // Initializing
    [SerializeField]
    Difficulty difficulty;
    bool[] teeth;

    // Publics
    [Header("Objects")]
    public GameObject toothPrefab;

    // Tracked
    [Range(0, 100)]
    public float minimumPressure;
    [Range(0, 100)]
    public float maximumPressure;
    public float pressureMinChangePerTooth;
    [Range(0, 100)]
    public float targetMinimumPressure;
    [Range(0, 100)]
    public float targetMaximumPressure;

    float AnchoredMinP;
    float AnchoredMaxP;

    [SerializeField]
    public float pressure = 0;
    static float pressureChangePlayer = 20;
    public bool lockpicking;
    bool lockIsBroken;
    bool Unlocked;
    float lockHealth = 100;
    public int currentTooth;
    public int UnlockingTreshold;

    bool initialized;

    // Initializing the Lock
    private void Start()
    {
        currentTooth = 0;

        //References the Lock Manager Constructors to get the teeht amount
        teeth = new bool[LockManager.DifficultyArray[(int)difficulty]._teethAmount];

        minimumPressure = LockManager.DifficultyArray[(int)difficulty]._minPressure;
        maximumPressure = LockManager.DifficultyArray[(int)difficulty]._maxPressure;
        pressureMinChangePerTooth = LockManager.DifficultyArray[(int)difficulty]._PressureChangePerTooth;

        targetMaximumPressure = LockManager.DifficultyArray[(int)difficulty]._teethAmount * 10 + 20;
        targetMinimumPressure = 0 + 20;

        AnchoredMaxP = targetMaximumPressure;
        AnchoredMinP = targetMinimumPressure;

        minimumPressure = targetMinimumPressure - 10;
        maximumPressure = targetMaximumPressure + 10;

        _lockPressureChange = pressureChangePlayer * 0.5f;

        Debug.Log("targetMaximumAtStart" + targetMaximumPressure);

        bool[] unavailableToothPosition = new bool[7];
        int randomPosition;
        int toothNumber = 0;

        for (int i = 0; i < teeth.Length; i++)
        {
            GameObject newTooth = Instantiate(toothPrefab); // Instantiate the tooth prefab
            newTooth.transform.parent = transform;

            //Assighns a random nuber to chack if that position is available
            randomPosition = Random.Range(0, LockManager.toothPositions.Length);
            int iterations = 0;

            //if it is unavailable then find a new random number and check again.
            while (unavailableToothPosition[randomPosition] || iterations < 100)
            {
                randomPosition = Random.Range(0, LockManager.toothPositions.Length);
                iterations++;
            }

            /*
            if (iterations >= 100)
            {
                for (int j = 0; j < teeth.Length; j++)
                {
                    if (unavailableToothPosition[j])
                        return;
                    else
                    {
                        randomPosition = j;
                        unavailableToothPosition[j] = true;
                        break;
                    }
                }
            }
            */
            //if it is available then mark that position as unavailable
            unavailableToothPosition[randomPosition] = true;

            newTooth.GetComponent<Tooth>().toothNumber = toothNumber;
            toothNumber++;

            newTooth.transform.localPosition = LockManager.toothPositions[randomPosition];
            //Debug.Log(randomPosition);

        }

        UnlockingTreshold = toothNumber;

        initialized = true;
        Debug.Log("Initialized");
    }



    private void Update()
    {
        if (!initialized)
            return;

        if (!Unlocked)
        {


            RaisePressure();

            CheckPivotingPressure();

            CheckPressure();

            CheckLockHealth();
        }
        else
        pressure += pressureChangePlayer * 2.5f * Time.deltaTime;

    }

    private void RaisePressure()
    {
        if (lockpicking)
            pressureChangePlayer = 20;
        else
            pressureChangePlayer = 40;

        if (Input.GetKey(KeyCode.Space) && !lockIsBroken)
        {
            if (pressure <= 100)
            {
                pressure += pressureChangePlayer * Time.deltaTime;
            }

        }
        else
        {
            if (pressure > 0)
                pressure -= pressureChangePlayer * Time.deltaTime;
        }

    }

    void CheckPressure()
    {
        if (pressure <= minimumPressure)
        {
            lockpicking = false;
            targetMinimumPressure = AnchoredMinP;
            targetMaximumPressure = AnchoredMaxP;
            currentTooth = 0;
        }
        else
            lockpicking = true;

        if (pressure >= maximumPressure)
        {
            if (UnlockingTreshold > currentTooth)
                lockHealth -= 20 * Time.deltaTime;
            else
            {
                Unlocked = true;
                Debug.Log("You Lockpicked Succesfully");
            }

        }

        //Debug.Log("MinimumPressure" + minimumPressure);
    }

    private void CheckLockHealth()
    {
        if (lockHealth <= 0)
        {
            lockpicking = false;
            lockIsBroken = true;
            Debug.Log("You Broke the Lock");
        }
    }

    float _lockPressureChange;

    void CheckPivotingPressure()
    {

            if (minimumPressure > targetMinimumPressure)
            minimumPressure -= _lockPressureChange * Time.deltaTime;
        if (minimumPressure < targetMinimumPressure)
            minimumPressure += _lockPressureChange * Time.deltaTime;

        if (maximumPressure > targetMaximumPressure)
            maximumPressure -= _lockPressureChange * Time.deltaTime;
        if (maximumPressure < targetMaximumPressure)
            maximumPressure += _lockPressureChange * Time.deltaTime;
    }

    public void PivotPressure()
    {
        float DistanceBetweenMinMAx;
        float ClampedPressureDistance;

        //Check what is the maximum distace clamps can be from eachother
        ClampedPressureDistance = (-10 * ((float)difficulty)) + 30;
       
        //Check the distance between clamps
        DistanceBetweenMinMAx = targetMaximumPressure - targetMinimumPressure;

        //Lower the Distance between clamps
        DistanceBetweenMinMAx -= pressureMinChangePerTooth;

        if (DistanceBetweenMinMAx <= ClampedPressureDistance)
        {
            DistanceBetweenMinMAx = ClampedPressureDistance;

        }


        float ChangeRate = Mathf.Floor(Random.Range(DistanceBetweenMinMAx, 101));

        float difficultyMultiplier;
        difficultyMultiplier = ((float)difficulty + 1) * 10;



        if (Mathf.Abs(ChangeRate - targetMaximumPressure) < difficultyMultiplier)
        {
            if (targetMaximumPressure >= 100 - difficultyMultiplier)
            {
                ChangeRate = targetMaximumPressure - difficultyMultiplier;
            }
            else
            {
                ChangeRate = targetMinimumPressure + difficultyMultiplier;
            }
        }

        if( DistanceBetweenMinMAx <= ClampedPressureDistance)
        {
            DistanceBetweenMinMAx = ClampedPressureDistance;
        }

        targetMaximumPressure = ChangeRate;

        if(targetMaximumPressure < DistanceBetweenMinMAx)
        {
            targetMaximumPressure = DistanceBetweenMinMAx;
        }

        targetMinimumPressure = targetMaximumPressure - DistanceBetweenMinMAx;






        //Debug.Log("ClampedPressure " + ClampedPressureDistance);

        //Debug.Log("DistanceBetweenMinMAx " + DistanceBetweenMinMAx);

        //Debug.Log("MinPressure " + targetMinimumPressure + " ||" + " MaxPressure " + targetMaximumPressure);


    }

    public void PivotPressureWithTooth()
    {

        //if(targetMaximumPressure - targetMinimumPressure > (-10 * ((float)difficulty)) + 30)
        targetMinimumPressure += 10 * Time.deltaTime;



    }


    // Tooth Transform positions for the teeth to reference
    public Transform TopLimit;
    public Transform BottomLimit;
    public Transform TargetPosition;

   

}
