/// <summary>
/// This file keeps track and handle the golems.
///
/// Author: Renato Avellar Nobre - 984405
/// Class Project: Artificial Inteligence for Video Games 2021-2022
/// Version 1.0.0
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class for a population individual, the Golem
///
/// Here each golem has a chromosome that contains its genes information. A golem also has
/// a ,aterial for its prefab, multipliers to fine tune the health and lifespan, and other meta
/// information.
/// </summary>
public class Golem : MonoBehaviour {

    /// <summary>
    /// The Golem Material
    /// </summary>
    public Material material;

    /// <summary>
    /// The Golem Chromosome with data from its caracteristics
    /// </summary>
    private Chromosome chromosome;

    /// <summary>
    ///  How many generations a golem can live through
    /// </summary>
    public float lifespan;

    /// <summary>
    /// The Golem health indicates how much damage it can takes from
    /// the environment befor dieing
    /// </summary>
    public float health;

    /// <summary>
    /// Flag to set rendering mode to transparent
    ///
    /// This has been disabled here because it just doesn't look good transparent
    /// </summary>
    public bool transparancy_flag = false;

    /// <summary>
    /// Multiplier for fine tuning golem life span
    /// </summary>
    public int base_lifeSpan = 1;

    /// <summary>
    /// Multiplier for fine tuning golem health
    /// </summary>
    public int base_Health = 1;

    /// <summary>
    /// Generation in which the golem war born
    /// </summary>
    public int genBorn;

    /// <summary>
    /// Variable to check whether this golem has reproduced or not in the generation
    /// </summary>
    public bool reproduced = false;

    /// <summary>
    /// Initialize the material propertie with the value from the golem component
    ///
    /// This function might set the renderer to transparent if the transfarency flag is on
    /// </summary>
    void Awake() {
        material = gameObject.GetComponent<Renderer>().material;
        if (transparancy_flag) {
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
        }
    }

    /// <summary>
    /// Set the birth generation of the Golem
    /// </summary>
    /// <param name="gen">Generation Number</param>
    public void SetBirthgen(int gen){
        this.genBorn = gen;
    }

    /// <summary>
    /// Initialize the parameter chromosome into the golem class.
    ///
    /// After the chromosome is set this function
    /// sets the golems proprieties: color, size, metalic and health
    /// </summary>
    /// <param name="chromosome">The Chromosome to be set to the Golem </param>
    public void SetChromosome(Chromosome chromosome) {
        this.chromosome = chromosome;
        SetColor();
        SetSize();
        SetMetalic();
        SetHealth();
    }

    /// <summary>
    /// Fetch the Golem Chromosome
    /// </summary>
    /// <returns>The Golem Chromosome</returns>
    public Chromosome GetChromosome() {
        return this.chromosome;
    }

    /// <summary>
    /// Set the Golem Color based on the value on its chromosome
    /// </summary>
    public void SetColor() {
        Color c = new Color(this.chromosome.Genes[(int)GeneIndex.Red],
                            this.chromosome.Genes[(int)GeneIndex.Green],
                            this.chromosome.Genes[(int)GeneIndex.Blue],
                            this.chromosome.Genes[(int)GeneIndex.Alpha]);
        material.color = c;
    }

    /// <summary>
    /// Set the Golem Metalic and Smoothness
    /// based on the value on its chromosome
    /// </summary>
    public void SetMetalic() {
        float metalic = this.chromosome.Genes[(int)GeneIndex.Metalic];
        float smooth = this.chromosome.Genes[(int)GeneIndex.Smooth];
        material.SetFloat("_Metallic", metalic);
        material.SetFloat("_Glossiness", smooth);
    }

    /// <summary>
    /// Set the Golem size based on the value on its chromosome
    /// </summary>
    public void SetSize() {
        float size = this.chromosome.Genes[(int)GeneIndex.Size];
        Vector3 scaleChange = new Vector3(size, size, size);
        gameObject.transform.localScale = scaleChange;
    }

    /// <summary>
    /// Set the Golem health and lifespan based on the value on its chromosome
    /// </summary>
    public void SetHealth() {
        this.lifespan = this.chromosome.Genes[(int)GeneIndex.Lifespan] * base_lifeSpan;
        this.health = this.chromosome.Genes[(int)GeneIndex.HP] * base_Health;
    }

}
