using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSection : MonoBehaviour
{
    [SerializeField]
    private double width = 60;
    [SerializeField]
    private double height = 40;

    public int Width
    {
        get
        {
            return (int)width;
        }

        set
        {
            width = value;
        }
    }

    public int Height
    {
        get
        {
            return (int)height;
        }

        set
        {
            height = value;
        }
    }

    //Cross-sectional area
    public double Ax
    {
        get
        {
            return width * height;
        }
    }

    //Shear area in local y-axis
    public double Asy
    {
        get
        {
            return Ax * 2 / 3;
        }
    }

    //Shear area in local z-axis
    public double Asz
    {
        get
        {
            return Ax * 2 / 3;
        }
    }

    //Torsional moment of inertia
    public double Jx
    {
        get
        {
            //https://en.wikipedia.org/wiki/Torsion_constant
            //https://www.fxsolver.com/browse/formulas/Torsion+constant+%28Rectangle%29
            double a = System.Math.Max(height, width), b = System.Math.Min(height, width);
            return (1D / 3D - 0.224D / (a / b + 0.161D)) * a * b * b * b;
        }
    }

    //Moment of inertia for bending about y-axis
    public double Iy
    {
        get
        {
            return height * width * width * width / 12;
        }
    }

    //Moment of inertia for bending about z-axis
    public double Iz
    {
        get
        {
            return width * height * height * height / 12;
        }
    }
}
