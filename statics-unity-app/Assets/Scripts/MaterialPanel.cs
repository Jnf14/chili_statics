using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MaterialPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI materialName;
    [SerializeField]
    private TextMeshProUGUI density;
    [SerializeField]
    private TextMeshProUGUI price;
    [SerializeField]
    private RawImage icon;
    
    public string Name
    {
        get
        {
            return materialName.text;
        }

        set
        {
            materialName.text = value;
        }
    }

    public double Density
    {
        get
        {
            return double.Parse(density.text.Substring("Density: ".Length));
        }

        set
        {
            density.text = "Density: " + value.ToString();
        }
    }

    public string Price
    {
        get
        {
            return price.text;
        }

        set
        {
            price.text = value;
        }
    }

    public Texture Icon
    {
        get
        {
            return icon.texture;
        }

        set
        {
            icon.texture = value;
        }
    }
}
