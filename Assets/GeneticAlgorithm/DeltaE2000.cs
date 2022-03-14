/// <summary>
/// File for usage in DeltaE2000 evaluation and RGB to LAB Conversiong
///
/// Author: Renato Avellar Nobre - 984405
/// Class Project: Artificial Inteligence for Video Games 2021-2022
/// Version 1.0.0
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class for usage in color fitness evaluation using the DeltaE2000 formula.
/// </summary>
public class DeltaE2000 {

    /// <summary>
    /// Simple color magnitude diference for testing purposes
    /// </summary>
    /// <param name="env">The environment rgb color</param>
    /// <param name="golem">The golem rgb color</param>
    /// <returns>The magnitude fitness value between the colors</returns>
    public static float notDeltaE2000(Color env, Color golem) {
        float fitness = (new Vector3(env.r, env.g, env.b) - new Vector3(golem.r, golem.g, golem.b)).magnitude;
        return fitness;
    }

    /// <summary>
    /// Performs the DeltaE2000 color diference for two RGB colors.
    /// Handles appropriate color conversions to LAB.
    ///
    /// Math based on:
    /// http://www.brucelindbloom.com/index.html?Eqn_DeltaE_CIE2000.html
    /// </summary>
    /// <param name="c_env">The environment rgb color</param>
    /// <param name="c_golem">The golem rgb color</param>
    /// <returns>The DeltaE2000 fitness value [0, 100] between the colors</returns>
    public static float EvaluateFitness(Color c_env, Color c_golem){
        float L1, a1, b1, L2, a2, b2;
        float h1, h2, H_mean, delta_h;

        Lab_Color env = new Lab_Color(c_env);
        L1 = env.L;
        a1 = env.a;
        b1 = env.b;

        Lab_Color golem = new Lab_Color(c_golem);
        L2 = golem.L;
        a2 = golem.a;
        b2 = golem.b;

        float L_mean = (L1 + L2)/2.0f;

        float C1 = Mathf.Sqrt(Mathf.Pow(a1, 2) + Mathf.Pow(b1, 2));
        float C2 = Mathf.Sqrt(Mathf.Pow(a2, 2) + Mathf.Pow(b2, 2));
        float C_mean = (C1 + C2) / 2.0f;

        float G = 0.5f*(1-Mathf.Sqrt(Mathf.Pow(C_mean, 7.0f) / (Mathf.Pow(C_mean, 7.0f) + Mathf.Pow(25.0f, 7.0f))));

        float new_a1 = a1*(1 + G);
        float new_a2 = a2*(1 + G);

        float new_C1 = Mathf.Sqrt(Mathf.Pow(new_a1, 2) + Mathf.Pow(b1, 2));
        float new_C2 = Mathf.Sqrt(Mathf.Pow(new_a2, 2) + Mathf.Pow(b2, 2));
        float new_C_mean = (new_C1 + new_C2)/2;

        if(Mathf.Atan2(b1, new_a1)*Mathf.Rad2Deg >= 0) {
            h1 = (Mathf.Atan2(b1, new_a1)*Mathf.Rad2Deg);
        } else {
            h1 = (Mathf.Atan2(b1, new_a1)*Mathf.Rad2Deg) + 360; // +360 = 2pi
        }

        if(Mathf.Atan2(b2, new_a2)*Mathf.Rad2Deg >= 0) {
            h2 = (Mathf.Atan2(b2, new_a2)*Mathf.Rad2Deg);
        } else {
            h2 = (Mathf.Atan2(b2, new_a2)*Mathf.Rad2Deg) + 360; // +360?
        }

        if (Mathf.Abs(h1 - h2) > 180) {
            H_mean = (h1 + h2 + 360)/2.0f;
        } else {
            H_mean = (h1 + h2)/2.0f;
        }

        float T = (float)(1 -
            (0.17*Mathf.Cos((H_mean-30)*Mathf.Deg2Rad)) +
            (0.24*Mathf.Cos((2*H_mean)*Mathf.Deg2Rad)) +
            (0.32*Mathf.Cos((3*H_mean +6)*Mathf.Deg2Rad)) -
            (0.20*Mathf.Cos((4*H_mean - 63)*Mathf.Deg2Rad))
        );

        if (Mathf.Abs(h2-h1) <= 180) {
            delta_h = h2 - h1;
        } else if (Mathf.Abs(h2-h1) > 180 && h2 <= h1) {
            delta_h = h2 - h1 + 360;
        } else {
            delta_h = h2 - h1 - 360;
        }

        float delta_L = L2 - L1;
        float delta_C = new_C2 - new_C1;

        float delta_H = 2 * Mathf.Sqrt(new_C1*new_C2) * (Mathf.Sin((delta_h/2.0f)*Mathf.Deg2Rad));

        float S_L = (float)(1 + (0.015*Mathf.Pow(L_mean-50, 2))/Mathf.Sqrt(20 + Mathf.Pow(L_mean-50, 2)));
        float S_C = (float)(1 + (0.045*new_C_mean));
        float S_H = (float)(1 + (0.015*new_C_mean*T));

        float delta_omega = 30 * Mathf.Exp(-Mathf.Pow(((H_mean-275)/25),2));

        float R_C = 2 * Mathf.Sqrt(Mathf.Pow(new_C_mean, 7.0f) / (Mathf.Pow(new_C_mean, 7.0f) + Mathf.Pow(25.0f, 7.0f)));

        float R_T = -R_C * Mathf.Sin((2*delta_omega)*Mathf.Deg2Rad);

        float K_L = 1;
        float K_C = 1;
        float K_H = 1;

        float delta_E = Mathf.Sqrt(
            Mathf.Pow(delta_L/(K_L*S_L),2) +
            Mathf.Pow(delta_C/(K_C*S_C),2) +
            Mathf.Pow(delta_H/(K_H*S_H),2) +
            R_T*(delta_C/(K_C*S_C))*(delta_H/(K_H*S_H))
        );

        return delta_E;
    }

}


/// <summary>
/// To Calculate the DeltaE2000 fitness score we need first to convert
/// the rgb color to the CIELAB standard
/// </summary>
public class Lab_Color {

    /// <summary>
    /// The L represents the Lightness
    /// The a* axis is relative to the green–red opponent colors,
    /// with negative values toward green and positive values toward red.
    /// The b* axis represents the blue–yellow opponents,
    /// with negative numbers toward blue and positive toward yellow.
    /// </summary>
    public float L, a, b;

    /// <summary>
    /// Creates the Lab Color from an RGB
    /// </summary>
    /// <param name="rgb">The RGB Color to convert</param>
    public Lab_Color(Color rgb) {
        Vector3 xyz_col = Lab_Color.RGBtoXYZ(rgb);
        Vector3 lab_col = Lab_Color.XYZtoLAB(xyz_col);

        this.L = lab_col[0];
        this.a = lab_col[1];
        this.b = lab_col[2];
    }

    /// <summary>
    /// Convert RGB to LAB is not straightfoward, we need first to convert
    /// RGB to XYZ and from there on convert XYZ to LAB.
    ///
    /// Math based on:
    /// http://www.easyrgb.com/en/math.php
    /// http://www.brucelindbloom.com/index.html?Eqn_RGB_to_XYZ.html
    /// </summary>
    /// <param name="color">RGB Color to Convert to XYZ</param>
    /// <returns>Resulting XYZ color</returns>
    private static Vector3 RGBtoXYZ(Color color) {
        double var_R, var_G, var_B;
        double X, Y, Z;
        double exponent = 2.4;

        var_R = color.r;
        var_G = color.g;
        var_B = color.b;

        if (var_R > 0.04045) {
            var_R = Math.Pow(((var_R + 0.055) / 1.055), exponent);
        } else {
            var_R = var_R / 12.92f;
        }

        if (var_G > 0.04045) {
            var_G = Math.Pow(((var_G + 0.055) / 1.055), exponent);
        } else {
            var_G = var_G / 12.92f;
        }

        if (var_B > 0.04045) {
            var_B = Math.Pow(((var_B + 0.055) / 1.055), exponent);
        } else {
            var_B = var_B / 12.92;
        }

        var_R = var_R * 100;
        var_G = var_G * 100;
        var_B = var_B * 100;

        X = var_R * 0.4124 + var_G * 0.3576 + var_B * 0.1805;
        Y = var_R * 0.2126 + var_G * 0.7152 + var_B * 0.0722;
        Z = var_R * 0.0193 + var_G * 0.1192 + var_B * 0.9505;

        return new Vector3((float)X, (float)Y, (float)Z);
    }

    /// <summary>
    /// Convert XYZ colors to LAB using a Adobe-sRGB (XYZ) color as reference
    ///
    /// Math based on:
    /// http://www.easyrgb.com/en/math.php
    /// http://www.brucelindbloom.com/index.html?Eqn_XYZ_to_Lab.html
    /// </summary>
    /// <param name="color_xyz">The vector3 XYZ color</param>
    /// <returns>A Vector3 of the Created LAB color</returns>
    private static Vector3 XYZtoLAB(Vector3 color_xyz) {
        double var_X, var_Y, var_Z;
        double L, a, b;
        double ref_x = 94.811;
        double ref_y = 100.0;
        double ref_z = 107.304;


        var_X = color_xyz[0] / ref_x;
        var_Y = color_xyz[1] / ref_y;
        var_Z = color_xyz[2] / ref_z;

        if (var_X > 0.008856) {
            var_X = Math.Pow(var_X, (1.0/3.0));
        } else {
            var_X = (7.787 * var_X) + (16.0/116.0);
        }

        if (var_Y > 0.008856f) {
            var_Y = Math.Pow(var_Y, (1.0/3.0));
        } else {
            var_Y = ( 7.787 * var_Y) + (16.0/116.0);
        }

        if (var_Z > 0.008856f){
            var_Z = Math.Pow(var_Z, (1.0/3.0));
        } else {
            var_Z = (7.787 * var_Z) + (16.0/116.0);
        }

        L = (116 * var_Y) - 16;
        a = 500 * (var_X - var_Y);
        b = 200 * (var_Y - var_Z);

        return new Vector3((float)L, (float)a, (float)b);
    }
}
