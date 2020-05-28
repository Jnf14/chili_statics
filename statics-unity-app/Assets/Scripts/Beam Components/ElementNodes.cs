using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ElementNodes : MonoBehaviour
{
    public Node n1, n2;

    public float getAngle()
    {
        return (float)(Math.Atan2(n2.y - n1.y, n2.x - n1.x) * 180 / Math.PI);
    }

    public float getLength()
    {
        return (float)(Math.Sqrt(Math.Pow(n2.x - n1.x, 2) + Math.Pow(n2.y - n1.y, 2)));
    }
}
