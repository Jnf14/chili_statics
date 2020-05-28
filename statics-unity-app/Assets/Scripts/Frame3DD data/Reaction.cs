using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction
{
    public int node;
    public double Fx;
    public double Fy;
    public double Fz;
    public double Mx;
    public double My;
    public double Mz;

    public Reaction(int node, double Fx, double Fy, double Fz, double Mx, double My, double Mz)
    {
        this.node = node;
        this.Fx = Fx;
        this.Fy = Fy;
        this.Fz = Fz;
        this.Mx = Mx;
        this.My = My;
        this.Mz = Mz;
    }
}
