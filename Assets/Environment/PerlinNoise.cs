/// <summary>
/// File for generating a Terrain with perlin noise
///
/// Author: Renato Avellar Nobre - 984405
/// Class Project: Artificial Inteligence for Video Games 2021-2022
/// Version 1.0.0
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates a Terrain with perlin noise
/// </summary>
public class PerlinNoise {

    /// <summary>
    /// Scale of the X and Z matrix to be generated on top of the terrain
    /// </summary>
    public static int scale = 10;

    /// <summary>
    /// Returns the Created terrain data, with a proper size and resolution and
    /// its perlin noise heights.
    /// </summary>
    /// <param name="terrainData">An instantiated terrain data to be populated</param>
    /// <param name="terrainSize">The desired terrain size</param>
    /// <returns>The perlin terrain data</returns>
    public static TerrainData GenerateTerrain (TerrainData terrainData, Vector3 terrainSize) {

        Debug.Log(terrainSize.x);
        terrainData.heightmapResolution = (int)terrainSize.x+1;
        Debug.Log(terrainSize.x);
        // Set terrain size
        terrainData.size = new Vector3(terrainSize.x, terrainSize.y, terrainSize.z);

        // Attribute the perlin noise heights
        terrainData.SetHeights(0, 0, GenerateHeights(terrainSize));

        return terrainData;
    }


    /// <summary>
    /// Generate heights for a terrain considering a x,y matrix with an scale factor
    /// </summary>
    /// <param name="terrainSize">The size of the terrain</param>
    /// <returns>The heights for the terrain</returns>
    private static float[,] GenerateHeights(Vector3 terrainSize) {
        // This is a Height Matrix of size X, Z where the value at a certain position
        // represent the heigh Y in that terrain position
        float [,] heights = new float[(int)terrainSize.x, (int)terrainSize.z];
        for (int i = 0; i < terrainSize.x; i++) {
            for (int j = 0; j < terrainSize.z; j++) {
                heights[i, j] = CalculateHeight(i, j, terrainSize);
            }
        }

        return heights;
    }

    /// <summary>
    /// Calculate the perlin noise height
    /// </summary>
    /// <param name="x">Position x on the terrain matrix</param>
    /// <param name="z">Position z in terrain matrix</param>
    /// <param name="terrainSize">Terrain size</param>
    /// <returns>The generated perlin noise height value for this coordinate</returns>
    private static float CalculateHeight (int x, int z, Vector3 terrainSize) {
        float xCoord = (float) x / terrainSize.x * scale;
        float zCoord = (float) z / terrainSize.z * scale;

        return Mathf.PerlinNoise(xCoord, zCoord);
    }


}
