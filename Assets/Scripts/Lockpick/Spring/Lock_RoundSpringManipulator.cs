using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;

public class Lock_RoundSpringManipulator : MonoBehaviour
{
    /*
        This code only manipulates blendshapes of a specific spring in the library. 
        It supports 11 blendshapes if you find use for such a thing.
        But I would advise not to use this code for anything else.
    */
    public bool inversed;

    [SerializeField]
    [Range(0,100)]
    float ReferenceNumber;

    SkinnedMeshRenderer skinnedMeshRenderer;
    Lock _lock;

    int blendShapeCount;

    int previousPrimaryIndex;
    int currentPrimaryIndex;

    float difference;


    private void Start()
    {
        ReferenceNumber = 0;
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        _lock = GetComponentInParent<Lock>();
        blendShapeCount = skinnedMeshRenderer.sharedMesh.blendShapeCount;
    }

    private void Update()
    {
        GetReferenceNumberFromLock();

        CalculateIndex(ReferenceNumber);
        UpdateBlendShapes();
    }



    void CalculateIndex(float ReferenceNumber)
    {
        if (inversed)
        {
            ReferenceNumber = 100 - ReferenceNumber;
        }
 

        float dividedIndex = ReferenceNumber / 10;

        currentPrimaryIndex = Mathf.FloorToInt(dividedIndex);

        difference = dividedIndex - currentPrimaryIndex;
    }

    void UpdateBlendShapes()
    {

        /*
        //Check if new primary index is different from old one.
        if(currentPrimaryIndex != previousPrimaryIndex)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(previousPrimaryIndex, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(previousPrimaryIndex + 1, 0);
        }
        */

        for (int i = 0; i < blendShapeCount; i++) {
            skinnedMeshRenderer.SetBlendShapeWeight(i, 0);
        }

        //Set PrimaryIndex
        skinnedMeshRenderer.SetBlendShapeWeight(currentPrimaryIndex, (1 - difference) * 100);

        //Set Secondary Index but also check if it is outside of the bounds of the array
        if(currentPrimaryIndex + 1 <  blendShapeCount)
        skinnedMeshRenderer.SetBlendShapeWeight(currentPrimaryIndex + 1, difference * 100);

    }


    // no fancy way to switch between modes. Just add this to update at the top to make it get the reference number from the lock script in a parent
    void GetReferenceNumberFromLock()
    {
        if(!inversed)
        ReferenceNumber = _lock.minimumPressure;
        else 
        ReferenceNumber = _lock.maximumPressure;
    }
}
