using UnityEngine;

public class PerlinNoiseMatrix : MonoBehaviour 
{
    public int width = 200;
    public int height = 200;
    public float scale = 0.1f;
    public float offsetX = -100f;
    public float offsetY = -100f;

    public Material material;
    Mesh mesh;

    public RoadGenerator roadGenerator;

    public float[,] noisemap;

    void Start()
    {
        noisemap = GenerateNoiseMap();
        roadGenerator = GetComponent<RoadGenerator>();
        CalculateRoad();
        addMesh(noisemap);

        // Now you can use the noiseMap for whatever purpose you need
    }

   public float[,] GenerateNoiseMap()
    {
        float[,] noiseMap = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float sampleX = (x + offsetX) * scale;
                float sampleY = (y + offsetY) * scale;

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                float heightValue = perlinValue * 10f; // Adjusting the range to be between 0 and 10

                noiseMap[x, y] = heightValue;
            }
        }

        return noiseMap;
    }

    void addMesh(float[,] noisemap)
    {
        GameObject point = new GameObject();
        MeshFilter meshFilter = point.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = point.AddComponent<MeshRenderer>();
        point.transform.position = new Vector3(-100, 0, -100);
        point.transform.localScale = Vector3.one;
        meshFilter.mesh = GenerateMesh(noisemap);
        meshRenderer.material = material;
        meshRenderer.material.color = Color.grey;
    }

    Mesh GenerateMesh(float[,] noiseMap)
    {
        int verticesPerLine = width;
        int verticesCount = verticesPerLine * verticesPerLine;
        Vector3[] vertices = new Vector3[verticesCount];
        int[] triangles = new int[(verticesPerLine - 1) * (verticesPerLine - 1) * 6];
        int triIndex = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                vertices[y * verticesPerLine + x] = new Vector3(x, noiseMap[x, y], y);

                if (x < width - 1 && y < height - 1)
                {
                    triangles[triIndex] = y * verticesPerLine + x;
                    triangles[triIndex + 1] = (y + 1) * verticesPerLine + x;
                    triangles[triIndex + 2] = y * verticesPerLine + x + 1;
                    triangles[triIndex + 3] = y * verticesPerLine + x + 1;
                    triangles[triIndex + 4] = (y + 1) * verticesPerLine + x;
                    triangles[triIndex + 5] = (y + 1) * verticesPerLine + x + 1;
                    triIndex += 6;
                }
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    public void CalculateRoad()
    {
        roadGenerator.GenerateRoads(noisemap, (int)offsetX, (int)offsetY);
    }
}
