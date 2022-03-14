/// <summary>
/// Handles the operations for the genetic algortihm simulation.
///
/// Author: Renato Avellar Nobre - 984405
/// Class Project: Artificial Inteligence for Video Games 2021-2022
/// Version 1.0.0
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// Handles the operations for the genetic algortihm simulation, this usualy performs
/// population level operations. If the operation will happen on an individual golem it
/// will be handled by the genetics class
/// </summary>
public class Simulation : MonoBehaviour {

    /// <summary>
    /// Time of a single step in seconds
    /// </summary>
    public float stepTime = 10f;

    /// <summary>
    /// Population Initial Size
    /// </summary>
    public int populationInitialSize = 100;

    /// <summary>
    /// Population Max Size
    /// </summary>
    public int populationMaxSize = 0;

    /// <summary>
    ///  Number of childrens resulting from the mating process
    /// </summary>
    public float numberOfChilds = 2;

    /// <summary>
    /// Golem prefab with 0 stripes
    /// </summary>
    public GameObject Golem0;

    /// <summary>
    /// Golem prefab with 1 stripe
    /// </summary>
    public GameObject Golem1;

    /// <summary>
    /// Golem prefab with 2 stripes
    /// </summary>
    public GameObject Golem2;

    /// <summary>
    /// Golem prefab with 3 stripes
    /// </summary>
    public GameObject Golem3;

    /// <summary>
    /// UI Top Text
    /// </summary>
    public Text top_text;

    /// <summary>
    /// UI Bottom Text
    /// </summary>
    public Text bottom_text;

    /// <summary>
    /// Terrain environment of the simulation
    /// </summary>
    public Terrain environment;

    /// <summary>
    /// Rate in which a mutation occurs
    /// </summary>
    public float mutationRate = 0.1f;

    /// <summary>
    /// List of Golems that keeps track of the population
    /// </summary>
    /// <typeparam name="Golem">Golem in the population</typeparam>
    protected List<Golem> population = new List<Golem>();

    /// <summary>
    /// Genetics class to handle crossovers and other genetic operations
    /// </summary>
    private Genetics genetics;

    /// <summary>
    /// Current Generation number
    /// </summary>
    private int currentGen = 0;

    /// <summary>
    /// Ammount of gollems killed this generation
    /// </summary>
    private int golemKilled = 0;

    /// <summary>
    /// Ammount of gollems borned this generation
    /// </summary>
    private int bornedThisGen = 0;

    /// <summary>
    /// Vector of ammount of golems for each size
    /// </summary>
    private int[] g_size = { 0, 0, 0 };

    /// <summary>
    /// Vector of ammount of golems for each stripes
    /// </summary>
    private int[] strp_amt = { 0, 0, 0, 0 };

    /// <summary>
    /// Vector of ammount of golems for each life type
    /// </summary>
    private int[] life_amt = { 0, 0, 0 };

    /// <summary>
    /// Vector of ammount of golems for each hp type
    /// </summary>
    private int[] hp_amt = { 0, 0, 0 };

    /// <summary>
    /// Vector of ammount of golems for each metal interval
    /// </summary>
    private int[] metal_amt = {0, 0, 0};

    /// <summary>
    /// Vector of ammount of golems for each smooth interval
    /// </summary>
    private int[] smt_amt = {0, 0, 0};

    /// <summary>
    /// Vector of ammount of golems for each transparancy interval
    /// </summary>
    private int[] alpha_amt = { 0, 0, 0, 0 };

    /// <summary>
    /// Start the simulation class, initializing the genetics class,
    /// creating the initial population and starting the coroutine to execute
    /// the simulation steps
    /// </summary>
    void Start() {
        genetics = new Genetics(environment, Golem0, Golem1, Golem2, Golem3);
        InitialPopulation();
        StartCoroutine(StepSimulation());
    }

    /// <summary>
    /// The only prupose for the update function in the simulations is to update the
    /// UI information
    /// </summary>
    void Update() {
        top_text.text = "Generation: " + currentGen + Environment.NewLine + "Population Size: " + population.Count
                        + Environment.NewLine + "Died Last Gen: " + golemKilled + Environment.NewLine
                        + "Borned This Gen: " + bornedThisGen;


        bottom_text.text = "Amounts:" + Environment.NewLine + "Size 1: " + g_size[0] + "      Size 2: " + g_size[1] +
                          "      Size 3: "  + g_size[2]
                          + Environment.NewLine + "Stripes 0: " + strp_amt[0] + "      Stripes 1: " + strp_amt[1] + "      Stripes 2: " + strp_amt[2] + "      Stripes 3: " + strp_amt[3]
                          + Environment.NewLine + "LifeSpan Low: " + life_amt[0] + "      LifeSpan Medium: " + life_amt[1] + "      LifeSpan High: " + life_amt[2]
                          + Environment.NewLine + "HP Low: " + hp_amt[0] + "      HP Medium: " + hp_amt[1] + "      HP High: " + hp_amt[2]
                          + Environment.NewLine + "Metalic Zero: " + metal_amt[0] + "      Metalic Low: " + metal_amt[1] + "      Metalic High: " + metal_amt[2]
                          + Environment.NewLine + "Smooth Zero: " + smt_amt[0] + "      Smooth Low: " + smt_amt[1] + "      Smooth High: " + smt_amt[2]
                          + Environment.NewLine + "Tranparency Zero: " + alpha_amt[0] + "      Tranparency Low: " + alpha_amt[1] + "      Tranparency High: " + alpha_amt[2];

    }

    /// <summary>
    /// Create the Initial Population
    /// </summary>
    void InitialPopulation() {
        for (int i = 0; i < populationInitialSize; i++) {
            Golem golem = genetics.CreateGolem();
            population.Add(golem);
        }
        // Count initial population traits
        CountPopulationTraits();
    }

    /// <summary>
    /// Execute the whole genetic algorithm process of evaluating the
    /// population.
    /// </summary>
    void EvaluatePopulation() {
        Kill();
        Reproduce();
        CountPopulationTraits();
    }

    /// <summary>
    /// Execute the reproducing routine
    /// </summary>
    void Reproduce() {
        // Creat at temporary list for the new Golems
        List<Golem> tempList = new List<Golem>();
        // Variable for traking amount of golem borns
        bornedThisGen = 0;
        // Loop through the population
        for(int i = 0; i < population.Count; i += 1) {
            // Select the first parent
            Golem parent1 = population[i];
            Golem parent2 = null;

            // This logic will look for the closest golem that
            // has not yet reproduced
            float dist;
            float minDist = Mathf.Infinity;
            Vector3 p1_pos = parent1.transform.position;

            if(!parent1.reproduced){
                // Look for an avaiable partner
                for(int j = 0; j < population.Count; j++){
                    Golem candidate = population[j];
                    if(candidate.reproduced || j == i ){
                        continue;
                    } else {
                        // Save the closest partner by far
                        dist = (p1_pos - candidate.transform.position).magnitude;
                        if (dist < minDist) {
                            minDist = dist;
                            parent2 = candidate;
                        }
                    }
                }
                parent1.reproduced = true;

                // This condition check if there is an elegible second parent
                // it might not have if the list is odd
                if (parent2 != null) {
                    parent2.reproduced = true;
                    // Create kids and add to the list
                    for (int j = 0; j < numberOfChilds; j++) {
                        // Call the genetics class to handle the child genetics
                        Golem child = genetics.Crossover(parent1, parent2, currentGen, mutationRate);
                        tempList.Add(child);
                    }
                }
            }
        }


        bornedThisGen = tempList.Count;
        population.AddRange(tempList);
    }

    /// <summary>
    /// Perform all actions related to the process of killing the unfitted golems in
    /// the populatiom.
    /// </summary>
    void Kill(){

        // Control purge mode if the max population size is reached
        bool purge = false;
        if(population.Count >= populationMaxSize) {
            purge = true;
        }

        // Zero the kill count variable
        golemKilled = 0;

        // Iterate through population killing unfited golems
        for(int i = 0; i < population.Count; i++) {
            // This is here so we dont need to add another for loop just to reset
            // the reprodution status
            population[i].reproduced = false;
            // Get Golem Color
            Color golem_color = population[i].GetComponent<MeshRenderer>().material.color;
            // Get Colem Position
            int golem_x = (int)population[i].transform.position.x;
            int golem_z = (int)population[i].transform.position.z;
            // Call the genetics class to evaluate the fitness
            float fitness = genetics.CalculateFitness(golem_color, golem_x, golem_z);
            //Select the future of a single golem
            Selection(population[i], fitness, i, purge);
        }

        // Remove all dead golems remains from population list
        population.RemoveAll(x => x == null);
    }


    /// <summary>
    /// Decides if a golem will die in this generation.
    /// </summary>
    /// <param name="golem">The golem to be evaluated</param>
    /// <param name="fitness">The DeltaE2000 fitness of the Golem with its terrain</param>
    /// <param name="i">The Golem index in the population list</param>
    /// <param name="purge">Flag to control maximum population and kill old golems</param>
    void Selection(Golem golem, float fitness, int i, bool purge) {

        // Descrease Golem Health
        golem.health -= CalculateDamage(fitness);

        // Kill Golem if health below 0
        if(golem.health <= 0) {
            Destroy(golem.gameObject);
            population[i] = null;
            golemKilled += 1;
        }

        // Kill Golem if Purge Mode is On and Golem is older than its lifespan
        if(currentGen - golem.genBorn >= golem.lifespan && purge) {
            Destroy(golem.gameObject);
            population[i] = null;
            golemKilled += 1;
        }

    }

    /// <summary>
    /// Calculates the damage to give to the a golem based on its
    /// fitness score. This is needed to fine tune the simulation.
    /// </summary>
    /// <param name="fitness">The DeltaE2000 fitness value</param>
    /// <returns>The damage to apply to the golem</returns>
    private float CalculateDamage(float fitness) {
        if (fitness < 20) {
            return 0.2f;
        } else if (fitness >= 20 && fitness < 50) {
            return (fitness/50.0f);
        } else {
            return fitness;
        }
    }

    /// <summary>
    /// This function retrieve information about the population to
    /// display in the user interfac
    /// </summary>
    void CountPopulationTraits() {
        for(int i = 0; i < 4; i ++) {
            if (i < 3) {
                hp_amt[i] = 0;
                life_amt[i] = 0;
                metal_amt[i] = 0;
                smt_amt[i] = 0;
                alpha_amt[i] = 0;
                g_size[i] = 0;
            }
            strp_amt[i] = 0;
        }

        for(int i = 0; i < population.Count; i++) {
            Chromosome c = population[i].GetChromosome();
            g_size[(int)c.Genes[(int)GeneIndex.Size]-1] += 1;
            strp_amt[(int)c.Genes[(int)GeneIndex.Stripes]] += 1;
            life_amt[(int)c.Genes[(int)GeneIndex.Lifespan]-1] += 1;
            hp_amt[(int)c.Genes[(int)GeneIndex.HP]-1] += 1;

            if (c.Genes[(int)GeneIndex.Metalic] == 0.0f) {
                metal_amt[0] += 1;
            } else if (c.Genes[(int)GeneIndex.Metalic] < 0.5) {
                metal_amt[1] += 1;
            } else {
                metal_amt[2] += 1;
            }

            if (c.Genes[(int)GeneIndex.Smooth] == 0.0f) {
                smt_amt[0] += 1;
            } else if (c.Genes[(int)GeneIndex.Smooth] < 0.5) {
                smt_amt[1] += 1;
            } else {
                smt_amt[2] += 1;
            }

            if (c.Genes[(int)GeneIndex.Alpha] == 1.0f) {
                alpha_amt[0] += 1;
            } else if (c.Genes[(int)GeneIndex.Alpha] > 0.6) {
                alpha_amt[1] += 1;
            } else {
                alpha_amt[2] += 1;
            }
        }
    }

    /// <summary>
    /// Coroutine to run the simulation.
    ///
    /// This function perform forever and run each step of the simulation
    /// after a prefixed time.
    /// </summary>
    /// <returns>A wait time in seconds, for the simulation step</returns>
    IEnumerator StepSimulation() {
        while(true) {
            yield return new WaitForSeconds(stepTime);
            EvaluatePopulation();
            currentGen += 1;
        }
    }
}
