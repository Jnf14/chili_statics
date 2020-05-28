using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeakElementInternalForce
{

    public int element;
    public double Nx;               //positive is tensile, negative is compressive
    public double Vy;
    public double Vz;
    public double Txx;
    public double Myy;
    public double Mzz;              //positive means convex between node 1 and node 2

    public PeakElementInternalForce(int element, double Nx, double Vy, double Vz, double Txx, double Myy, double Mzz)
    {
        this.element = element;
        this.Nx = Nx;
        this.Vy = Vy;
        this.Vz = Vz;
        this.Txx = Txx;
        this.Myy = Myy;
        this.Mzz = Mzz;
    }

    public PeakElementInternalForce(PeakElementInternalForce original)
    {
        element = original.element;
        Nx = original.Nx;
        Vy = original.Vy;
        Vz = original.Vz;
        Txx = original.Txx;
        Myy = original.Myy;
        Mzz = original.Mzz;
    }
}