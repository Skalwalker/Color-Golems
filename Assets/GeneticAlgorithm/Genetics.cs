/// <summary>
/// Handles all the genetics related to a golem individual in the genetic algorithm
///
/// Author: Renato Avellar Nobre - 984405
/// Class Project: Artificial Inteligence for Video Games 2021-2022
/// Version 1.0.0
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all the genetics related to a golem individual in the genetic algorithm
/// </summary>
public class Genetics {

    /// <summary>
    /// GameObject for golem with no stripes
    /// </summary>
    public GameObject Golem0;

    /// <summary>
    /// GameObject for golem with 1 stripe
    /// </summary>
    public GameObject Golem1;

    /// <summary>
    /// GameObject for golem with 2 stripes
    /// </summary>
    public GameObject Golem2;

    /// <summary>
    /// GameObject for golem with 3 stripes
    /// </summary>
    public GameObject Golem3;

    /// <summary>
    /// The environments terrain
    /// </summary>
    Terrain env;

    /// <summary>
    /// The environments terrain data
    /// </summary>
    TerrainData t_data;

    /// <summary>
    /// The environments terrain boundaries
    /// </summary>
    Bounds boundaries;

    /// <summary>
    /// The environments terrain colors
    /// </summary>
    ColorTerrain colorTerrain;


    /// <summary>
    /// Constructor of the genetics class
    /// </summary>
    /// <param name="env">The environments terrain</param>
    /// <param name="Golem0">GameObject for golem with no stripes</param>
    /// <param name="Golem1">GameObject for golem with 1 stripes</param>
    /// <param name="Golem2">GameObject for golem with 2 stripes</param>
    /// <param name="Golem3">GameObject for golem with 3 stripes</param>
    public Genetics(Terrain env, GameObject Golem0, GameObject Golem1, GameObject Golem2, GameObject Golem3) {
        this.t_data = env.terrainData;
        this.colorTerrain = env.GetComponent<ColorTerrain>();
        this.env = env;
        this.boundaries = t_data.bounds;
        this.Golem0 = Golem0;
        this.Golem1 = Golem1;
        this.Golem2 = Golem2;
        this.Golem3 = Golem3;
    }

    /// <summary>
    /// Create the Initial Golem
    /// </summary>
    /// <returns>A golem from the generation 0</returns>
    public Golem CreateGolem() {
        Vector3 randomPosition = new Vector3(Random.Range(0.0f, 1.0f) * boundaries.size.x,
                                             Random.Range(0.0f, 1.0f) * boundaries.size.y,
                                             Random.Range(0.0f, 1.0f) * boundaries.size.z);
        Vector3 worldPosition = this.env.transform.position + randomPosition;


        Color golem_color = this.colorTerrain.GetTerrainColorAtPoint((int) Mathf.Floor(randomPosition.z),
                                                                     (int) Mathf.Floor(randomPosition.x));
        Chromosome chromosome = CreateChromosome(golem_color);
        GameObject temp = Object.Instantiate(Golem0);
        Golem golem = temp.GetComponent<Golem>();

        float terrainHeight = this.env.SampleHeight(randomPosition);
        float golemHeight = temp.GetComponent<MeshFilter>().mesh.bounds.size.y;
        worldPosition.y = terrainHeight + (golemHeight/2.0f);
        temp.transform.position = worldPosition;

        golem.GetComponent<Golem>().SetChromosome(chromosome);
        golem.GetComponent<Golem>().SetBirthgen(0);

        return golem;
    }

    /// <summary>
    /// Create a Chromosome
    /// </summary>
    /// <param name="c">Color RGBA of the chromosome</param>
    /// <param name="size">Size of the golem, defaults to 1</param>
    /// <param name="metalic">Metalic Aspect of the golem, defaults to 0</param>
    /// <param name="smooth">Smooth Aspect of the golem, defaults to 0</param>
    /// <param name="life">Life of the golem, defaults to 2</param>
    /// <param name="health">Health of the golem, defaults to 1</param>
    /// <param name="stripes">Stripes Aspect of the golem, defaults to 0</param>
    /// <returns></returns>
    public Chromosome CreateChromosome(Color c, int size=1, float metalic=0.0f, float smooth=0.0f,
                                       float life=2.0f, float health=1.0f, int stripes=0) {
        return new Chromosome(c, size, metalic, smooth, life, health, stripes);
    }


    /// <summary>
    /// Calculate the fitness between a Golem and a terrain X, and Z point
    /// </summary>
    /// <param name="golem_color">The color of the Golem</param>
    /// <param name="pos_x">The X position</param>
    /// <param name="pos_z">The Z position</param>
    /// <returns>The fitness value using the DeltaE2000</returns>
    public float CalculateFitness(Color golem_color, int pos_x, int pos_z) {
        Color terrain_color = this.colorTerrain.GetTerrainColorAtPoint(pos_z, pos_x);
        float fitness = DeltaE2000.EvaluateFitness(terrain_color, golem_color);
        return fitness;
    }

    /// <summary>
    /// Perform the crossover between two golems and creates the child golem
    /// </summary>
    /// <param name="p1">First Parent</param>
    /// <param name="p2">Second Parent</param>
    /// <param name="gen">Generation of the Golem</param>
    /// <param name="mutationRate">Mutation rate for the child genes</param>
    /// <returns>A child golem</returns>
    public Golem Crossover (Golem p1, Golem p2, int gen, float mutationRate) {
        Chromosome c1 = p1.GetChromosome();
        Chromosome c2 = p2.GetChromosome();

        Chromosome child = Crossovers.Uniform(c1, c2);

        child = Mutation(child, mutationRate);

        GameObject temp = GetProperGolem((int)child.Genes[(int)GeneIndex.Stripes]);
        Golem golem = temp.GetComponent<Golem>();

        Vector3 spawnPos = GetMidPoint(p1.transform.position, p2.transform.position);

        golem.GetComponent<Golem>().SetChromosome(child);
        golem.GetComponent<Golem>().SetBirthgen(gen);

        float terrainHeight = this.env.SampleHeight(spawnPos);
        float golemHeight = child.Genes[(int)GeneIndex.Size];
        spawnPos.y = terrainHeight + (golemHeight);
        temp.transform.position = spawnPos;

        return golem;
    }


    /// <summary>
    /// Evaluate for each chromosome in a gene if a mutation happened.
    /// If so, modify the chromosome to have a new random mutation
    /// </summary>
    /// <param name="c">The chromosome to evaluate for mutations</param>
    /// <param name="rateOfMutation">The probability of occuring a mutation at a chromosome</param>
    /// <returns> The mutated (or not) chromosome</returns>
    public Chromosome Mutation(Chromosome c, float rateOfMutation) {

        for(int i = 0; i < c.ChromosomeSize(); i++) {
            if(Random.Range(0.0f, 1.0f) < rateOfMutation) {
                if(i >= (int)GeneIndex.Red && i <= (int)GeneIndex.Blue) {
                    c.Genes[i] = Random.Range(0.0f, 1.0f);
                } else if (i == (int)GeneIndex.Alpha) {
                    c.Genes[i] = Random.Range(0.3f, 1.0f);
                } else if(i >= (int)GeneIndex.Metalic && i <= (int)GeneIndex.Smooth) {
                    c.Genes[i] = Random.Range(0.0f, 1.0f);
                } else if (i == (int)GeneIndex.Size) {
                    c.Genes[i] = Mathf.Round(Random.Range(1.0f, 3.0f));
                } else if (i >= (int)GeneIndex.Lifespan && i <= (int)GeneIndex.HP) {
                    c.Genes[i] = Mathf.Round(Random.Range(1.0f, 3.0f));
                } else if (i == (int)GeneIndex.Stripes) {
                    c.Genes[i] = Mathf.Round(Random.Range(0.0f, 3.0f));
                }
            }
        }

        return c;
    }

    /// <summary>
    /// Instantiate the proper golem prefab based on the ammount of stripes
    /// </summary>
    /// <param name="stripes">Ammount of stripes of the Golem</param>
    /// <returns>The proper golem object instantiated</returns>
    private GameObject GetProperGolem(int stripes){
        if (stripes == 1){
            return Object.Instantiate(Golem1);
        } else if (stripes == 2){
            return Object.Instantiate(Golem2);
        } else if (stripes == 3){
            return Object.Instantiate(Golem3);
        } else {
            return Object.Instantiate(Golem0);
        }
    }

    /// <summary>
    /// Get the 2D middle point of two points in 3D space
    /// </summary>
    /// <param name="pos1">Point 1</param>
    /// <param name="pos2">Point 2</param>
    /// <returns>The middle between x and z, ignoring y</returns>
    private Vector3 GetMidPoint(Vector3 pos1, Vector3 pos2) {
        float mid_x = (float)(pos1.x + pos2.x)/2.0f;
        float mid_z = (float)(pos1.z + pos2.z)/2.0f;

        return new Vector3(mid_x, 0, mid_z);
    }
}
