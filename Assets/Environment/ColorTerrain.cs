/// <summary>
/// File for creating the terrain and handle all the color functions needed
///
/// Author: Renato Avellar Nobre - 984405
/// Class Project: Artificial Inteligence for Video Games 2021-2022
/// Version 1.0.0
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enumerate the terrain colored layers
/// </summary>
public enum TerrainLayers {
            Blue,
            Green,
            Orange,
            Red,
            Yellow,
            Purple
}

/// <summary>
/// Creates the terrain and handle all the color functions needed
/// </summary>
public class ColorTerrain : MonoBehaviour {

    /// <summary>
    /// Terrain Height Y
    /// </summary>
    public int height = 20;

    /// <summary>
    /// Terrain Width X
    /// </summary>
    public int width = 256;

    /// <summary>
    /// Terrain Depth Z
    /// </summary>
    public int depth = 256;

    /// <summary>
    /// Terrain Object in Game
    /// </summary>
    public Terrain terrain;

    /// <summary>
    /// Material for the environment glass floor
    /// </summary>
    public Material GlassFloor;

    /// <summary>
    /// Material for the environment walls
    /// </summary>
    public Material WallsMaterial;

    /// <summary>
    /// Player to be spawned
    /// </summary>
    public GameObject player;

    /// <summary>
    /// Data of the layer to get the proper color at a position
    /// </summary>
    float[,,] layer_data;

    /// <summary>
    /// Create the perlin noise terrain, create the walls and the flass floor
    /// and initialize the color datas
    /// </summary>
    void Awake() {
        Vector3 terrainSize = new Vector3(width, height, depth);
        TerrainData t_data = terrain.terrainData;
        t_data = PerlinNoise.GenerateTerrain(t_data, terrainSize);

        ScenarioStructures.CreateWalls(width, depth, height, WallsMaterial);
        ScenarioStructures.CreateGlassFloor(width, depth, height, GlassFloor);

        layer_data = t_data.GetAlphamaps(0, 0, t_data.alphamapWidth, t_data.alphamapHeight);
    }

    /// <summary>
    /// Return the color of a terrain at a certain point
    /// </summary>
    /// <param name="x">Terrain point in the width axis</param>
    /// <param name="z">Terrain point in the depth axis</param>
    /// <returns>Color of the terrain at point</returns>
    public Color GetTerrainColorAtPoint(int x, int z) {
        TerrainData t_data = terrain.terrainData;

        // Terrain width and depth have different sizes from its
        // resolution, need to normalize
        float res = t_data.alphamapResolution;
        int map_to_res_x = (int)(res / t_data.size[0]);
        int map_to_res_z = (int)(res / t_data.size[2]);
        x = x * map_to_res_x;
        z = z * map_to_res_z;

        try {
            if (layer_data[x, z, (int)TerrainLayers.Blue] == 1.0f) {
                return Color.blue;
            } else if (layer_data[x, z, (int)TerrainLayers.Green] == 1.0f) {
                return Color.green;
            } else if (layer_data[x, z, (int)TerrainLayers.Orange] == 1.0f) {
                return new Color(1.0f, (float)165.0/255.0f, 0.0f);
            } else if (layer_data[x, z, (int)TerrainLayers.Red] == 1.0f) {
                return Color.red;
            } else if (layer_data[x, z, (int)TerrainLayers.Yellow] == 1.0f) {
                return Color.yellow;
            } else if (layer_data[x, z, (int)TerrainLayers.Purple] == 1.0f) {
                return new Color((float)101.0/255.0f, (float)49.0/255.0f, (float)142.0/255.0f);
            } else {
                return Color.grey;
            }
        } catch {
            return Color.grey;
        }
    }
}
