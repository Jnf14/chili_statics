using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PointLoad : MonoBehaviour
{
    //local
    public int target; // element number
    public int beamId; // Beam id (target + 1)
    public float weight;
    public float Px;
    public float Py;
    public float Pz;
    public float x; // X position on beam
    public float positionFromLeftEnd; // Percentage
    public ElementNodes elementNodes;
    private float displacement;
    [SerializeField]
    private int type;
    public string typeText;
    public int id = 0;

    public bool mouseOver = false;

    public int Type
    {
        get
        {
            return type;
        }
    }

    public void SetPointLoad(ElementNodes elementNodes, int target, float angle, float displacement)
    {
        this.target = target;
        this.beamId = target + 1;
        this.elementNodes = elementNodes;
        this.displacement = displacement;

        if(type == 0) typeText = "weight";
        else if(type == 1) typeText = "solar panel";
        else if(type == 2) typeText = "water tank";
        else typeText = "jacuzzi";

        Py = -(float)System.Math.Cos(angle * System.Math.PI / 180) * weight;
        Px = (float)System.Math.Sin(angle * System.Math.PI / 180) * weight;
        x = Vector3.Distance(gameObject.transform.position, elementNodes.n1.transform.position);
        x = (float)System.Math.Sqrt(x * x - displacement * displacement);
        x = Vector3.Distance(new Vector3(elementNodes.n1.x, elementNodes.n1.y), new Vector3(elementNodes.n2.x, elementNodes.n2.y)) * x / Vector3.Distance(elementNodes.n1.transform.position, elementNodes.n2.transform.position);
   
        if (elementNodes != null)
        {
            positionFromLeftEnd = (x / elementNodes.getLength()) * 100;
        }
    }

    public void SetId(int id)
    {
        this.id = id;
    }

    public void Detach()
    {
        elementNodes = null;
    }

    public void Update()
    {
        if (elementNodes != null)
        {
            x = Vector3.Distance(gameObject.transform.position, elementNodes.n1.transform.position);
            x = (float)System.Math.Sqrt(x * x - displacement * displacement);
            x = Vector3.Distance(new Vector3(elementNodes.n1.x, elementNodes.n1.y), new Vector3(elementNodes.n2.x, elementNodes.n2.y)) * x / Vector3.Distance(elementNodes.n1.transform.position, elementNodes.n2.transform.position);
            positionFromLeftEnd = (x / elementNodes.getLength()) * 100;
        }
    }

    public void OnMouseOver()
    {   
        if(id != 0 && elementNodes != null){
            WorldTooltip.ShowTooltip_Static(this.getString(false));
            if(!mouseOver)
                GameManager.instance.ExportLog("point load", "hover", "Hovered on " + typeText + " " + id);
            mouseOver = true;
        } 
    }

    public void OnMouseExit()
    {
        WorldTooltip.HideTooltip_Static();
        mouseOver = false;
    }

    public string getString(bool tiny)
    {
        if(tiny)
            return id + " Beam" + beamId + " " + positionFromLeftEnd.ToString("0.0") + "%"; 
        else
            return "Beam " + beamId + "\n" +"id " + id + "\n" + weight + "kg" + "\n" + positionFromLeftEnd.ToString("0.0") + "%"; 
    }  
}
