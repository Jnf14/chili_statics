using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialConstants
{
    public readonly double density;

    //Young's modulus
    public readonly double Young;

    //Shear modulus
    public readonly double G;

    //Compressive strength (parallel) MPa
    public readonly double fc0;

    //Compressive strength (perpendicular) MPa
    public readonly double fc90;

    //Tensile strength (parallel) MPa
    public readonly double ft0;

    //Tensile strength (perpendicular) MPa
    public readonly double ft90;

    //Bending strength MPa
    public readonly double fmk;

    //Shear strength MPa
    public readonly double fvk;

    public MaterialConstants(double density, double Young, double G, double fc0, double fc90, double ft0, double ft90, double fmk, double fvk)
    {
        this.density = density;
        this.Young = Young;
        this.G = G;
        this.fc0 = fc0;
        this.fc90 = fc90;
        this.ft0 = ft0;
        this.ft90 = ft90;
        this.fmk = fmk;
        this.fvk = fvk;
    }
}
