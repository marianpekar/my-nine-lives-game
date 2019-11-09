#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading;

[ExecuteInEditMode]
public class HeightmapsLoader : MonoBehaviour
{

    Terrain terrain;
    TerrainData terrainData;

    public Texture2D heightMapTexture;
    public int smoothPasses = 7;

    void OnEnable()
    {
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
    }

    public void LoadTexture()
    {
        float[,] heightMap = new float[terrainData.heightmapWidth, terrainData.heightmapHeight];

        for (int x = 0; x < terrainData.heightmapWidth; x++)
            for (int y = 0; y < terrainData.heightmapHeight; y++)
                heightMap[x, y] = heightMapTexture.GetPixel(x, y).grayscale;

        terrainData.SetHeights(0, 0, heightMap);
        SmoothTerrain();
    }

    void SmoothTerrain()
    {
        if (smoothPasses <= 0)
            return;

        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);

        float smoothProgress = 0;
        EditorUtility.DisplayProgressBar("Smoothing Terrain","Progress", smoothProgress);

        for (int i = 0; i < smoothPasses; i++)
        {
            for (int y = 0; y < terrainData.heightmapHeight; y++)
                for (int x = 0; x < terrainData.heightmapWidth; x++)
                {
                    float avgHeight = heightMap[x, y];
                    List<Vector2> neighbours = CreateListOfNeighbours(new Vector2(x, y), terrainData.heightmapWidth, terrainData.heightmapHeight);

                    foreach (Vector2 n in neighbours)
                        avgHeight += heightMap[(int)n.x, (int)n.y];

                    heightMap[x, y] = avgHeight / ((float)neighbours.Count + 1);
                }
            smoothProgress++;
            EditorUtility.DisplayProgressBar("Smoothing Terrain", "Progress", smoothProgress/smoothPasses);
        }

        terrainData.SetHeights(0, 0, heightMap);
        EditorUtility.ClearProgressBar();
    }

    List<Vector2> CreateListOfNeighbours(Vector2 position, int width, int height)
    {
        List<Vector2> neighbours = new List<Vector2>();
        for (int y = -1; y < 2; y++)
            for (int x = -1; x < 2; x++)
                if (!(x == 0 && y == 0))
                {
                    Vector2 nPos = new Vector2(Mathf.Clamp(position.x + x, 0, width - 1),
                                                Mathf.Clamp(position.y + y, 0, height - 1));
                    if (!neighbours.Contains(nPos))
                        neighbours.Add(nPos);
                }

        return neighbours;
    }

}

#endif
