using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GraphPoint : MonoBehaviour
{
    private string stressType;
    private float id; 
    private float value; 
    private string text;

    public bool mouseOver = false;

    public void SetGraphPointValues(int id, int stressSelector, float value, List<string> loadsToString)
    {
        this.id = id;
        this.value = value;
        switch(stressSelector){
            case 0: this.stressType = "Axial"; this.text = "Nx = "; break;
            case 1: this.stressType = "Shear"; this.text = "Vy = "; break;
            case 2: this.stressType = "Bending"; this.text = "Mzz = "; break;
        }
        this.text += value.ToString("# ###.0 N");
        foreach(string s in loadsToString){
            this.text += "\n" + s;
        }
    }

    public void OnMouseOver()
    {   
        GraphsTooltip.ShowTooltip_Static(this.text);
        if(!mouseOver)
            GameManager.instance.ExportLog("graph point", "hover", "Hovered on Graph point " + id.ToString() + ": " + stressType + " stress at " + value.ToString("# ###.0 N"));
        mouseOver = true;
        
    }

    public void OnMouseExit()
    {
        GraphsTooltip.HideTooltip_Static();
        mouseOver = false;
    } 
}
