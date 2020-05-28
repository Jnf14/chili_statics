using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Material: MonoBehaviour
{
    public static readonly Dictionary<MaterialType, MaterialConstants> MATERIALS;

    public static Texture conifersImage;
    public static Texture broadleafImage;

    public static UnityEngine.Material conifersMaterial;
    public static UnityEngine.Material broadleafMaterial;

    private static void InitializeMaterials()
    {
        MATERIALS.Add(MaterialType.C18, new MaterialConstants(          //Conifers
            3.80E-10,
            9000,
            560,
            18,
            2.2,
            11,
            0.4,
            18,
            3.4
            )
        );
        MATERIALS.Add(MaterialType.C22, new MaterialConstants(
            4.1E-10,
            10000,
            630,
            20,
            2.4,
            13,
            0.4,
            22,
            3.8
            )
        );
        MATERIALS.Add(MaterialType.C30, new MaterialConstants(
            4.6E-10,
            12000,
            750,
            23,
            2.7,
            18,
            0.4,
            30,
            4
            )
        );
        MATERIALS.Add(MaterialType.C40, new MaterialConstants(
            5E-10,
            14000,
            940,
            26,
            2.9,
            24,
            0.4,
            40,
            4
            )
        );
        MATERIALS.Add(MaterialType.D30, new MaterialConstants(          //Broadleaf
            6.4E-10,
            11000,
            690,
            23,
            8,
            18,
            0.6,
            30,
            4
            )
        );
        MATERIALS.Add(MaterialType.D50, new MaterialConstants(
            7.5E-10,
            14000,
            880,
            29,
            9.3,
            30,
            0.6,
            50,
            4
            )
        );
        MATERIALS.Add(MaterialType.D70, new MaterialConstants(
            1.08E-9,
            20000,
            1250,
            34,
            13.5,
            42,
            0.6,
            70,
            5
            )
        );
    }

    static Material()
    {
        MATERIALS = new Dictionary<MaterialType, MaterialConstants>();
        InitializeMaterials();
    }

    public static string GetName(MaterialType type)
    {
        string materialName = "";
        switch (type)
        {
            case MaterialType.C18:
                materialName = "Conifers (C18)";
                break;
            case MaterialType.C22:
                materialName = "Conifers (C22)";
                break;
            case MaterialType.C30:
                materialName = "Conifers (C30)";
                break;
            case MaterialType.C40:
                materialName = "Conifers (C40)";
                break;
            case MaterialType.D30:
                materialName = "Broadleaf (D30)";
                break;
            case MaterialType.D50:
                materialName = "Broadleaf (D50)";
                break;
            case MaterialType.D70:
                materialName = "Broadleaf (D70)";
                break;
            default:
                materialName = "Conifers (C18)";
                break;
        }
        return materialName;
    }

    public static string GetPrice(MaterialType type)
    {
        string materialPrice;
        switch (type)
        {
            case MaterialType.C18:
                materialPrice = "$";
                break;
            case MaterialType.C22:
                materialPrice = "$";
                break;
            case MaterialType.C30:
                materialPrice = "$$";
                break;
            case MaterialType.C40:
                materialPrice = "$$";
                break;
            case MaterialType.D30:
                materialPrice = "$";
                break;
            case MaterialType.D50:
                materialPrice = "$$";
                break;
            case MaterialType.D70:
                materialPrice = "$$$";
                break;
            default:
                materialPrice = "Conifers (C18)";
                break;
        }
        return materialPrice;
    }

    public static double GetDensity(MaterialType type)
    {
        return MATERIALS[type].density;
    }

    public static Texture GetImage(MaterialType type)
    {
        if (!conifersImage)
        {
            conifersImage = Resources.Load<Texture>("Old Resources/Images/ORIGINALS/woodBackground");
        }
        if (!broadleafImage)
        {
            broadleafImage = Resources.Load<Texture>("Old Resources/Images/ORIGINALS/pineWood");
        }
        Texture texture;
        switch (type)
        {
            case MaterialType.C18:
                texture = conifersImage;
                break;
            case MaterialType.C22:
                texture = conifersImage;
                break;
            case MaterialType.C30:
                texture = conifersImage;
                break;
            case MaterialType.C40:
                texture = conifersImage;
                break;
            case MaterialType.D30:
                texture = broadleafImage;
                break;
            case MaterialType.D50:
                texture = broadleafImage;
                break;
            case MaterialType.D70:
                texture = broadleafImage;
                break;
            default:
                texture = conifersImage;
                break;
        }
        return texture;
    }

    public static UnityEngine.Material GetMaterial(MaterialType type)
    {
        if (!conifersMaterial)
        {
            conifersMaterial = Resources.Load<UnityEngine.Material>("Mezanix/RealisticWoodTextures/0_0_Textures/Exterior_Flooring_Wood_1/Materials/Exterior_Flooring_Wood_1_basecolor_MaxColorIntensity_MedLightness");
        }
        if (!broadleafMaterial)
        {
            broadleafMaterial = Resources.Load<UnityEngine.Material>("Mezanix/RealisticWoodTextures/0_0_Textures/Exterior_Flooring_Wood_1/Materials/Exterior_Flooring_Wood_1_basecolor_MedColorIntensity_MinLightness");
        }
        UnityEngine.Material texture;
        switch (type)
        {
            case MaterialType.C18:
                texture = conifersMaterial;
                break;
            case MaterialType.C22:
                texture = conifersMaterial;
                break;
            case MaterialType.C30:
                texture = conifersMaterial;
                break;
            case MaterialType.C40:
                texture = conifersMaterial;
                break;
            case MaterialType.D30:
                texture = broadleafMaterial;
                break;
            case MaterialType.D50:
                texture = broadleafMaterial;
                break;
            case MaterialType.D70:
                texture = broadleafMaterial;
                break;
            default:
                texture = conifersMaterial;
                break;
        }
        return texture;
    }

    [SerializeField]
    private MaterialType type;
    public MaterialType Type
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
            MATERIALS.TryGetValue(value, out constants);
            GetComponent<Renderer>().material = GetMaterial(value);
        }
    }

    private MaterialConstants constants;
    public MaterialConstants Constants
    {
        get
        {
            return constants;
        }
    }

    

    private void Start()
    {
        MATERIALS.TryGetValue(type, out constants);
    }
}
