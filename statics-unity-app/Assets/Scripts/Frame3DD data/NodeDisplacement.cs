using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDisplacement
{
    public int node;
    public double xDsp;
    public double yDsp;
    public double zDsp;
    public double xRot;
    public double yRot;
    public double zRot;

    public NodeDisplacement(int node, double xDsp, double yDsp, double zDsp, double xRot, double yRot, double zRot)
    {
        this.node = node;
        this.xDsp = xDsp;
        this.yDsp = yDsp;
        this.zDsp = zDsp;
        this.xRot = xDsp;
        this.yRot = yRot;
        this.zRot = zRot;
    }
}
