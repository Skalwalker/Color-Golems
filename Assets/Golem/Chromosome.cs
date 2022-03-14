/// <summary>
/// This file keeps track and handle the golems chromosome.
///
/// Author: Renato Avellar Nobre - 984405
/// Class Project: Artificial Inteligence for Video Games 2021-2022
/// Version 1.0.0
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gene Index on the Chromosome for reference in code.
/// </summary>
enum GeneIndex {
    Red,
    Green,
    Blue,
    Alpha,
    Metalic,
    Smooth,
    Size,
    Stripes,
    Lifespan,
    HP
}

/// <summary>
/// Each golem has a unique chromosome. This class keeps track and handle the golems chromosome.
/// Also initializing it with values with needed. Each Golem has one chromosome.
/// </summary>
public class Chromosome {

    /// <summary>
    /// Chromosome struct to keep track of the Genes
    /// </summary>
    public List<float> Genes;

    /// <summary>
    /// Constructor for an empty chromosome
    /// </summary>
    public Chromosome() {
        this.Genes = new List<float>();
    }


    /// <summary>
    /// Constructor for a chromosome with genes information
    /// </summary>
    /// <param name="color">Color of the golem</param>
    /// <param name="size">Size (1, 2, 3) of the Golem</param>
    /// <param name="metalic">Metalic Value (between 0 and 1) of the Golem</param>
    /// <param name="smooth">Smooth Value (between 0 and 1) of the Golem</param>
    /// <param name="lifespan">Lifespan value (1, 2, or 3) of the Golem</param>
    /// <param name="health">Health value (1, 2, or 3) of the Golem</param>
    /// <param name="stripes">Ammount of stripes (0, 1, 2, or 3) of the Golem</param>
    /// <returns></returns>
    public Chromosome(Color color, int size, float metalic, float smooth, float lifespan, float health, float stripes): this(){

        Genes.Add(color.r);
        Genes.Add(color.g);
        Genes.Add(color.b);
        Genes.Add(color.a);
        Genes.Add(metalic);
        Genes.Add(smooth);
        Genes.Add(size);
        Genes.Add(stripes);
        Genes.Add(lifespan);
        Genes.Add(health);
    }

    /// <summary>
    /// Get the ammount of genes in the chromosome
    /// </summary>
    /// <returns>The ammount of genes in the chromosome</returns>
    public int ChromosomeSize(){
        return Genes.Count;
    }
}
