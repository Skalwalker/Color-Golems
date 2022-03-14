/// <summary>
/// Class of possible crossovers functions for chormosomes reproduction
///
/// Author: Renato Avellar Nobre - 984405
/// Class Project: Artificial Inteligence for Video Games 2021-2022
/// Version 1.0.0
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class of possible crossovers functions for chormosomes reproduction
/// </summary>
public class Crossovers {

    /// <summary>
    /// Single Point Crossover breaks the gene in one random point and
    /// assign the first half to the first parent and the second halft to the
    /// second parent
    /// </summary>
    /// <param name="c1">First parent chromosome</param>
    /// <param name="c2">Second parent chromosome</param>
    /// <returns>The created children chromosome</returns>
    public static Chromosome SinglePoint(Chromosome c1, Chromosome c2) {
        int break_point = (int)Mathf.Round(Random.Range(0, c1.ChromosomeSize()));

        Chromosome child_chromosome = new Chromosome();

        for (int i = 0; i < break_point; i++) {
            child_chromosome.Genes.Add(c1.Genes[i]);
        }
        for (int i=break_point; i < c2.ChromosomeSize(); i++) {
            child_chromosome.Genes.Add(c2.Genes[i]);
        }

        return child_chromosome;
    }

    /// <summary>
    /// Two Point Crossover breaks the gene in one random point and
    /// assign the first part to the first parent, the second part to the
    /// second parent, and the third part to the first parent again
    /// </summary>
    /// <param name="c1">First parent chromosome</param>
    /// <param name="c2">Second parent chromosome</param>
    /// <returns>The created children chromosome</returns>
    public static Chromosome TwoPoints(Chromosome c1, Chromosome c2) {
        int break_point_1 = (int)Mathf.Floor(Random.Range(0, c1.ChromosomeSize()));
        int break_point_2 = (int)Mathf.Ceil(Random.Range(break_point_1, c1.ChromosomeSize()));

        Chromosome child_chromosome = new Chromosome();

        for (int i = 0; i < break_point_1; i++) {
            child_chromosome.Genes.Add(c1.Genes[i]);
        }
        for (int i=break_point_1; i <  break_point_2; i++) {
            child_chromosome.Genes.Add(c2.Genes[i]);
        }
        for (int i=break_point_2; i < c2.ChromosomeSize(); i++) {
            child_chromosome.Genes.Add(c1.Genes[i]);
        }

        return child_chromosome;
    }

    /// <summary>
    /// Uniform Crossover assign randomly to the childrean each gene.
    /// If the probability is less then 0.5 it assigns the first parent gene
    /// Else it assigns the second parent gene.
    /// </summary>
    /// <param name="c1">First parent chromosome</param>
    /// <param name="c2">Second parent chromosome</param>
    /// <returns>The created children chromosome</returns>
    public static Chromosome Uniform(Chromosome c1, Chromosome c2) {

        Chromosome child_chromosome = new Chromosome();

        for(int i = 0; i < c1.ChromosomeSize(); i++) {
            float flip_coin = Random.Range(0.0f, 1.0f);
            if (flip_coin < 0.5) {
                child_chromosome.Genes.Add(c1.Genes[i]);
            } else {
                child_chromosome.Genes.Add(c2.Genes[i]);
            }
        }

        return child_chromosome;
    }
}
