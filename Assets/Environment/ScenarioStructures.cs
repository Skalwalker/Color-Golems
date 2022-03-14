/// <summary>
/// File for creating terrain supporting structures
///
/// Author: Renato Avellar Nobre - 984405
/// Class Project: Artificial Inteligence for Video Games 2021-2022
/// Version 1.0.0
/// </summary>

using UnityEngine;

/// <summary>
/// Creates terrain supporting structures
/// </summary>
public class ScenarioStructures {

    /// <summary>
    /// Distance applied to the height of the possible maximum point of the terrain
    /// </summary>
    private static int heightOffset = 20;

    /// <summary>
    /// Create Glass for at proper position
    ///
    /// Since terrain size can change this needs to be created at runtime
    /// </summary>
    /// <param name="width">Terrain X</param>
    /// <param name="depth">Terrain Z</param>
    /// <param name="height">Terrain Y</param>
    /// <param name="GlassFloor">Invisible material to be applied at the glass floor</param>
    public static void CreateGlassFloor(float width, float depth, float height, Material GlassFloor){
        // There is a scale 1 for 10 in terrain and planes
        // Need to create a plane with 1/10 of the terrain scale
        float terrainPlaneDif = 10f;

        // Set new width
        float floorWidth = (float)width/terrainPlaneDif;
        // Set new depth
        float floorDepth = (float)depth/terrainPlaneDif;

        // Create plane primitive
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        // Set its scale and height
        floor.transform.localScale = new Vector3(floorWidth, 1, floorDepth);
        // Position on top of the terrain
        // Divisions by to to get the center
        floor.transform.position = new Vector3(width/2, height+heightOffset, depth/2);

        // Set the material of the plane to transparent
        floor.GetComponent<MeshRenderer>().material = GlassFloor;
    }

    /// <summary>
    /// Create the four walls sorounding the terrain
    /// </summary>
    /// <param name="width">Terrain X</param>
    /// <param name="depth">Terrain Z</param>
    /// <param name="height">Terrain Y</param>
    /// <param name="WallsMaterial">Material to be applied at the walls</param>
    public static void CreateWalls(float width, float depth, float height, Material WallsMaterial) {

        int wallTickness = 3;
        float offset = (float)wallTickness/2.0f;

        CreateWall(width, wallTickness, width/2.0f, -offset, WallsMaterial, height);
        CreateWall(width, wallTickness, width/2.0f, depth+offset, WallsMaterial, height);


        CreateWall(wallTickness, depth+(2*wallTickness), -offset, depth/2.0f, WallsMaterial, height);
        CreateWall(wallTickness, depth+(2*wallTickness), width+offset, depth/2.0f, WallsMaterial, height);
    }

    /// <summary>
    /// Create a Single Wall
    /// </summary>
    /// <param name="x_size">Wall X Size</param>
    /// <param name="z_size">Wall Z Size</param>
    /// <param name="x_pos">Wall X Position</param>
    /// <param name="z_pos">Wall Z Position</param>
    /// <param name="WallsMaterial">Wall Material</param>
    /// <param name="height">Wall Height</param>
    private static void CreateWall(float x_size, float z_size, float x_pos, float z_pos, Material WallsMaterial, float height){
        float wall_height = height + heightOffset;

        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.transform.localScale = new Vector3(x_size, wall_height, z_size);
        wall.transform.position = new Vector3(x_pos, (float)wall_height/2.0f, z_pos);

        wall.GetComponent<MeshRenderer>().material = WallsMaterial;
    }
}
