using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformDistributedLoad
{
    public int target;
    public int beamId; // Beam id (target + 1)
    public float load;
    public float weight;
    public float Px;
    public float Py;
    public float Pz;
    public string Origin { get; set; }
    public GameObject gameObject;

    public UniformDistributedLoad(int target, float load, float angle, string origin, GameObject gameObject)
    {
        SetUniformDistributedLoad(target, load, angle, origin, gameObject);
    }

    public void SetUniformDistributedLoad(int target, float load, float angle, string origin, GameObject gameObject)
    {
        //angle==0 => force is perpendicular
        this.target = target;
        this.beamId = target + 1;
        this.load = load;
        this.gameObject = gameObject;
        float componentLength = (float)gameObject.GetComponent<ElementNodes>().getLength();
        this.weight = load * componentLength ;
        Origin = origin;
        Py = -(float)System.Math.Cos(angle * System.Math.PI / 180) * load;
        Px = -(float)System.Math.Sin(angle * System.Math.PI / 180) * load;
    }

    public string getString(bool tiny)
    {
        if(tiny)
            return "S Beam" + beamId.ToString() + " " +weight.ToString("0.0") + "kg"; 
        else
            return "Beam " + beamId.ToString() + "\n" +"id S" + "\n" + weight.ToString("0.0e0") + "kg"; 
    }  
}
