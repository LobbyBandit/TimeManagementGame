using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Tooth : MonoBehaviour
{
    public int toothNumber;
    bool imNext;
    bool imUp;
    bool isClicked;

    Lock _lock;

    Vector3 startingPosition;
    Vector3 endPosition;
    [Range(0, 1)]
    float positionMultiplier;


    private void Start()
    {
        imNext = false;
        imUp = false;
        positionMultiplier = 0f;
        _lock = GetComponentInParent<Lock>();

        //References To Lockscripts tooth section
        topLimitY = _lock.TopLimit.localPosition.y;
        bottomLimitY = _lock.BottomLimit.localPosition.y;
        targetPositionY = _lock.TargetPosition.localPosition.y;

        //setting the tooth starting position
        startingPosition = gameObject.transform.localPosition;
        Vector2 startingYvalue;
        startingYvalue = CalculateStartingYvalue();
        startingPosition.y = Random.Range(startingYvalue.x, startingYvalue.y);

        endPosition = gameObject.transform.localPosition;
        endPosition.y = targetPositionY;

        //Graphics
        SetToothGraphics();
        UpdatePosition();

    }

    private void OnMouseDown()
    {
        if (!imUp)
            isClicked = true;
    }

    private void OnMouseUp()
    {
        isClicked = false;
    }



    private void Update()
    {
        if (!_lock.lockpicking)
        {
            imUp = false;
            if (positionMultiplier > 0)
            {
                positionMultiplier -= 0.5f * Time.deltaTime;
                UpdatePosition();
            }

            return;
        }

        if (positionMultiplier >= 0.99f)
        {
            if (imNext)
            {

                imUp = true;
                imNext = false;
                _lock.currentTooth++;
                _lock.PivotPressure();
            }
            else
            {
                if (!imUp)
                    _lock.PivotPressureWithTooth();
            }

            //Debug.Log("ImUp");
        }

        if (!imUp && positionMultiplier > 0 && !isClicked)
            positionMultiplier -= 0.5f * Time.deltaTime;


        if (isClicked)
        {
            if (toothNumber == _lock.currentTooth)
            {
                imNext = true;
            }

            MoveTooth();
        }


        UpdatePosition();

    }

    void MoveTooth()
    {
        if (positionMultiplier >= 1)
            return;

        if (imNext)
        {
            if (positionMultiplier > 0.8f)
                positionMultiplier += 0.5f * Time.deltaTime;
            else
                positionMultiplier += 1f * Time.deltaTime;
        }

        else
            positionMultiplier += 1f * Time.deltaTime;

    }



    void UpdatePosition()
    {
        gameObject.transform.localPosition = Vector3.Lerp(startingPosition, endPosition, positionMultiplier);
    }

    [HideInInspector]
    public float bottomLimitY;
    [HideInInspector]
    public float topLimitY;
    [HideInInspector]
    public float targetPositionY;

    Vector2 CalculateStartingYvalue()
    {
        Vector2 startingValue;
        float difference;
        float minimumValue;

        minimumValue = Mathf.Min(bottomLimitY, targetPositionY);
        difference = Mathf.Max(bottomLimitY, targetPositionY) - minimumValue;

        startingValue.x = minimumValue + difference * 0.2f;
        startingValue.y = minimumValue + difference * 0.6f;



        return startingValue;
    }

    public GameObject TopBone;
    public GameObject BottomBone;
    public GameObject Spring;

    void SetToothGraphics()
    {
        // teeth
        Vector3 TopPosition;

        TopPosition = TopBone.transform.position;
        TopPosition.y = Random.Range(Mathf.Abs(targetPositionY - ((targetPositionY - topLimitY) * 0.2f)), Mathf.Abs(startingPosition.y - topLimitY));
        TopBone.transform.position = TopPosition;

        Vector3 BottomPosition;

        BottomPosition = BottomBone.transform.position;
        BottomPosition.y = (Mathf.Abs(startingPosition.y - bottomLimitY)) * -1;
        BottomBone.transform.position = BottomPosition;

        Spring.transform.parent = null;
        Spring.transform.position = new Vector3(Spring.transform.position.x, topLimitY * 2, Spring.transform.position.z);
    }

    //topLimitY + (topLimitY* 0.5f)
}


