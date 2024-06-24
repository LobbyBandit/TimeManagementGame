using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock_SpringManipulator : MonoBehaviour
{

    public GameObject SpringContractedPosition;
    public GameObject SpringExpandedTargetPosition;

    public Transform followTransformTarget;

    public SkinnedMeshRenderer SpringMeshRenderer;

    public int shapeKeyIndex = 1;

    float distanceRange;

    float shapeKeyValue;

    public bool followTransform;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.parent = null;

        float minDistance = Vector3.Distance(gameObject.transform.position, SpringContractedPosition.transform.position);
        float maxDistance = Vector3.Distance(gameObject.transform.position,SpringExpandedTargetPosition.transform.position);

        distanceRange = Mathf.Abs(maxDistance - minDistance);

    }

    // Update is called once per frame
    void Update()
    {
        if (followTransform)
            FollowTransform();

        CalculateShapekeyValue();

        SpringMeshRenderer.SetBlendShapeWeight(shapeKeyIndex, shapeKeyValue);
    }

    void CalculateShapekeyValue()
    {
        float distanceFromTarget;

        distanceFromTarget = Vector3.Distance(SpringExpandedTargetPosition.transform.position, gameObject.transform.position);

        shapeKeyValue = 100 - (Mathf.Clamp(distanceFromTarget / distanceRange, 0, 100) * 100);

    }

    void FollowTransform() 
    {
        SpringExpandedTargetPosition.transform.position = followTransformTarget.transform.position;
    }


}
