using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RoadGenerator : MonoBehaviour
{
    Vector3[] mainInterestPoints = new Vector3[5];
    Vector3[] secondaryInterestPoints = new Vector3[8];
    Vector2Int[] grid = new Vector2Int[8];
    float[] stepCost = new float[8];

    int noiseMapX;
    int noiseMapY;

    public GameObject IndicatorPoint;
    public GameObject indicatorLine;
    GameObject PointsContainer;

    public List<Vector3> MainPointList = new List<Vector3>();
    public List<Vector3> SecondaryPointList = new List<Vector3>();

    public void GenerateRoads(float[,] noisemap, int OffsetX, int OffsetY)
    {
        Destroy(PointsContainer);
        PointsContainer = new GameObject("VisualRep");
        noiseMapX = noisemap.GetLength(0);
        noiseMapY = noisemap.GetLength(1);

        GenerateMainPoints(noisemap, OffsetX, OffsetY);
        GenerateSecondaryPoints(noisemap, OffsetX, OffsetY);

        ConnectSecondaryPoints(noisemap, OffsetX, OffsetY);
    }
    
    private void GenerateMainPoints(float[,] noisemap, int OffsetX, int OffsetY)
    {
       

        mainInterestPoints[0] = GenerateRandomCoordinate(noisemap, noiseMapX, noiseMapY, OffsetX, OffsetY);
        mainInterestPoints[1] = new Vector3(noiseMapX + OffsetX, noisemap[noiseMapX - 1, noiseMapY / 2], noiseMapY / 2 + OffsetY);
        mainInterestPoints[2] = new Vector3(noiseMapX / 2 + OffsetX, noisemap[noiseMapX / 2, noiseMapY - 1], noiseMapY + OffsetY);
        mainInterestPoints[3] = new Vector3(0 + OffsetX, noisemap[0, noiseMapY / 2], noiseMapY / 2 + OffsetY);
        mainInterestPoints[4] = new Vector3(noiseMapX / 2 + OffsetX, noisemap[noiseMapX / 2, 0], 0 + OffsetY);

        Color IndicatorColor = Color.red;

        for (int i = 0; i < mainInterestPoints.Length; i++)
        {
            GameObject mainSphere = Instantiate(IndicatorPoint, PointsContainer.transform);
            mainSphere.transform.position = mainInterestPoints[i];
            mainSphere.transform.localScale *= 2;
            mainSphere.GetComponent<MeshRenderer>().material.color = IndicatorColor;
        }

        /* LineRenderer
        for (int i = 1; i < mainInterestPoints.Length; i++)
        {
            GameObject inbetweenLine = Instantiate(indicatorLine, PointsContainer.transform);
            LineRenderer lineRenderer = inbetweenLine.GetComponent<LineRenderer>();

            lineRenderer.SetPosition(0, mainInterestPoints[i]);
            lineRenderer.SetPosition(1, mainInterestPoints[0]);
            lineRenderer.startColor = IndicatorColor;
            lineRenderer.endColor = IndicatorColor;
        }
        */

        Vector2Int targetPosition = new Vector2Int((int)mainInterestPoints[0].x - OffsetX - 1, (int)mainInterestPoints[0].z - OffsetY - 1);
        targetPosition = new Vector2Int(Mathf.Clamp(targetPosition.x, 0, 199), Mathf.Clamp(targetPosition.y, 0, 199));

        for (int i = 1; i < mainInterestPoints.Length; i++)
        {
            Vector2Int currentPosition = new Vector2Int((int)mainInterestPoints[i].x - OffsetX - 1, (int)mainInterestPoints[i].z - OffsetY - 1);
            currentPosition = new Vector2Int(Mathf.Clamp(currentPosition.x, 0, 199), Mathf.Clamp(currentPosition.y, 0, 199));
            MarchToTarget(currentPosition, OffsetX, OffsetY, noiseMapX, noiseMapY, targetPosition, noisemap, MainPointList);
        }
    }

    void GenerateSecondaryPoints(float[,] noisemap, int OffsetX, int OffsetY)
    {
        for (int i = 0; i < secondaryInterestPoints.Length; i++)
        {
            secondaryInterestPoints[i] = GenerateRandomCoordinate(noisemap, noiseMapX, noiseMapY, OffsetX, OffsetY);

            GameObject TestSphere = Instantiate(IndicatorPoint, PointsContainer.transform);
            TestSphere.transform.position = secondaryInterestPoints[i];
            TestSphere.transform.localScale *= 2;
        }
    }

    void ConnectSecondaryPoints(float[,] noisemap, int OffsetX, int OffsetY)
    {
        int[] connected = new int[secondaryInterestPoints.Length];

        for (int i = 0; i < secondaryInterestPoints.Length; i++)
        {
            float distanceToPoint = 10000;
            Vector3 closestPointInterestPoint = Vector3.zero;

            for (int j = 0; j < secondaryInterestPoints.Length; j++)
            {
                if (secondaryInterestPoints[j] != secondaryInterestPoints[i])
                {
                    if (connected[j] < 2)
                    {
                        if (distanceToPoint > Vector3.Distance(secondaryInterestPoints[i], secondaryInterestPoints[j]))
                        {
                            closestPointInterestPoint = secondaryInterestPoints[j];
                            distanceToPoint = Vector3.Distance(secondaryInterestPoints[i], secondaryInterestPoints[j]);
                            connected[j]++;
                        }
                    }
                }
            }

            //if(distanceToPoint > MainPointOffset * 0.4f)


            if (closestPointInterestPoint != Vector3.zero)
            {
                Vector2Int targetPosition = new Vector2Int((int)closestPointInterestPoint.x - OffsetX - 1, (int)closestPointInterestPoint.z - OffsetY - 1);
                targetPosition = new Vector2Int(Mathf.Clamp(targetPosition.x, 0, 199), Mathf.Clamp(targetPosition.y, 0, 199));

                Vector2Int currentPosition = new Vector2Int((int)secondaryInterestPoints[i].x - OffsetX - 1, (int)secondaryInterestPoints[i].z - OffsetY - 1);
                currentPosition = new Vector2Int(Mathf.Clamp(currentPosition.x, 0, 199), Mathf.Clamp(currentPosition.y, 0, 199));

                MarchToTarget(currentPosition, OffsetX, OffsetY, noiseMapX, noiseMapY, targetPosition, noisemap, SecondaryPointList);
                /*
                GameObject inbetweenLine = Instantiate(indicatorLine, PointsContainer.transform);
                LineRenderer lineRenderer = inbetweenLine.GetComponent<LineRenderer>();

                lineRenderer.SetPosition(0, secondaryInterestPoints[i]);
                lineRenderer.SetPosition(1, closestPointInterestPoint);
                lineRenderer.startColor = Color.blue;
                lineRenderer.endColor = Color.blue;
                */
            }
        }
    }



    private void MarchToTarget(Vector2Int currentPosition, int OffsetX, int OffsetY, int noiseMapX, int noiseMapY, Vector2Int targetPosition, float[,] noisemap , List<Vector3> pointList)
    {
        
        int maxIterations = 1000; // Limit the maximum number of iterations to prevent freezing

        
        // Continue marching until the distance between currentPosition and targetPosition is less than or equal to 10
        while (Vector2Int.Distance(currentPosition, targetPosition) > 1 && maxIterations > 0)
        {
            CalculateGridAroundPoint(currentPosition);

            for (int j = 0; j < grid.Length; j++)
            {
                if (grid[j].x >= 0 && grid[j].x < noiseMapX && grid[j].y >= 0 && grid[j].y < noiseMapY)
                {
                    CalculateCost(noisemap, j, currentPosition, targetPosition);
                }
                else
                {
                    stepCost[j] = 100;
                }
            }

            int minIndex = -1;
            float minCost = float.MaxValue;

            for (int k = 0; k < stepCost.Length; k++)
            {
                if (stepCost[k] < minCost)
                {
                    minCost = stepCost[k];
                    minIndex = k;
                }
            }

            if (minIndex != -1)
            {
                currentPosition = grid[minIndex];
            }

            maxIterations--; // Decrement the iteration counter

            //Debug.Log("Point was Added");
            pointList.Add(new Vector3(currentPosition.x + OffsetX, noisemap[currentPosition.x, currentPosition.y], currentPosition.y + OffsetY));

        }
    }
    void CalculateGridAroundPoint(Vector2Int middlePoint)
    {
        grid[0] = middlePoint + new Vector2Int(-1, 1);
        grid[1] = middlePoint + new Vector2Int(0, 1);
        grid[2] = middlePoint + new Vector2Int(1, 1);
        grid[3] = middlePoint + new Vector2Int(1, 0);
        grid[4] = middlePoint + new Vector2Int(1, -1);
        grid[5] = middlePoint + new Vector2Int(0, -1);
        grid[6] = middlePoint + new Vector2Int(-1, -1);
        grid[7] = middlePoint + new Vector2Int(-1, 0);
    }
    void CalculateCost(float[,] noiseMap, int gridPosition, Vector2Int currentPosition, Vector2Int targetPosition)
    {
        stepCost[gridPosition] = (Vector2Int.Distance(currentPosition, targetPosition) - Vector2Int.Distance(grid[gridPosition], targetPosition)) * (-1);
        float slopeCost = Mathf.Abs(noiseMap[currentPosition.x, currentPosition.y] - noiseMap[grid[gridPosition].x, grid[gridPosition].y]); 
        if (slopeCost <= 0.3f) 
            slopeCost = 0;
        stepCost[gridPosition] += slopeCost;
      
    }

    private Vector3 GenerateRandomCoordinate(float[,] noisemap, int x, int y, int OffsetX, int OffsetY)
    {
        Vector3 randomCoordinate;
        randomCoordinate = new Vector3(Mathf.RoundToInt(Random.Range(x * 0.2f, x * 0.8f)), 0, Mathf.RoundToInt(Random.Range(y * 0.2f, y * 0.8f)));

        // Ensure that the generated coordinates fall within the valid range
        randomCoordinate.x = Mathf.Clamp(randomCoordinate.x, 0, x - 1);
        randomCoordinate.z = Mathf.Clamp(randomCoordinate.z, 0, y - 1);

        // Retrieve the height from the noise map
        randomCoordinate.y = noisemap[(int)randomCoordinate.x, (int)randomCoordinate.z];

        // Apply offsets
        randomCoordinate += new Vector3(OffsetX, 0, OffsetY);

        return randomCoordinate;
    }
    
    private void OnDrawGizmos()
    {
        // Set the color for the Gizmo
        Gizmos.color = Color.red;

        // Draw a sphere Gizmo at the specified point position
        foreach (Vector3 pointPosition in MainPointList)
        {
            // Draw a sphere Gizmo at the specified point position
            Gizmos.DrawSphere(pointPosition, 0.5f);
        }

        Gizmos.color = Color.blue;

        foreach (Vector3 pointPosition in SecondaryPointList)
        {
            // Draw a sphere Gizmo at the specified point position
            Gizmos.DrawSphere(pointPosition, 0.5f);
        }

    }
    
}



