using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeamEdit : MonoBehaviour
{
    public static BeamEdit instance;

    [SerializeField]
    private List<GameObject> allBeams;
    [SerializeField]
    private GameObject selectedBeam;
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    private MaterialType materialType;
    [SerializeField]
    private InputField widthInput;
    [SerializeField]
    private InputField heightInput;
    [SerializeField]
    private TMPro.TextMeshProUGUI materialText;

    public void SetHeight(string height)
    {
        int newHeight;
        if (int.TryParse(height, out newHeight))
        {
            SetHeight(newHeight);
        }
    }

    public void SetWidth(string width)
    {
        int newWidth;
        if (int.TryParse(width, out newWidth))
        {
            SetWidth(newWidth);
        }
    }

    public static void SetHeight(int height)
    {
        instance.height = height;
        instance.heightInput.text = height.ToString();
    }

    public static void SetWidth(int width)
    {
        instance.width = width;
        instance.widthInput.text = width.ToString();
    }

    private void Apply(GameObject beam)
    {
        CrossSection crossSection = beam.GetComponent<CrossSection>();
        Material material = beam.GetComponent<Material>();
        crossSection.Width = width;
        crossSection.Height = height;
        material.Type = materialType;
    }

    public void Apply()
    {
        Apply(selectedBeam);
        UIManager.instance.SetUIMode(UIManager.UIMode.None);
        GameManager.CalculateStatics();
    }

    public void ApplyAll()
    {
        foreach (GameObject beam in allBeams)
        {
            Apply(beam);
        }
        UIManager.instance.SetUIMode(UIManager.UIMode.None);
        GameManager.CalculateStatics();
    }

    public static void SetMaterial(MaterialType type)
    {
        instance.materialType = type;
        instance.materialText.text = Material.GetName(type);
    }

    public static void SetBeam(GameObject beam)
    {
        instance.selectedBeam = beam;
        SetMaterial(beam.GetComponent<Material>().Type);
        SetHeight(beam.GetComponent<CrossSection>().Height);
        SetWidth(beam.GetComponent<CrossSection>().Width);
    }

    private void Awake()
    {
        instance = this;
    }
}
