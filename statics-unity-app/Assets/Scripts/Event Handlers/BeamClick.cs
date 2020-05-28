using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamClick : MonoBehaviour
{
    private void OnMouseOver()
    {
        string text = "Beam " + (this.GetComponent<IdComponent>().id + 1).ToString();
        WorldTooltip.ShowTooltip_Static(text);
    }

    public void OnMouseExit()
    {
        WorldTooltip.HideTooltip_Static();
    }

    private void OnMouseDown()
    {
        UIManager.instance.SetUIMode(UIManager.UIMode.BeamPanel);
        BeamEdit.SetBeam(gameObject);
    }
}
