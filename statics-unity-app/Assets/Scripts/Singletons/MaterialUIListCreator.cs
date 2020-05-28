using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MaterialUIListCreator : MonoBehaviour
{
    private List<GameObject> materialPanels = new List<GameObject>();
    [SerializeField]
    private GameObject originalPanel;
    [SerializeField]
    private TextMeshProUGUI materialText;

    private void Start()
    {
        GameObject newObject;
        MaterialPanel newPanel;
        Button newButton;
        foreach (var materialType in Material.MATERIALS.Keys)
        {
            newObject = Instantiate(originalPanel, transform);
            materialPanels.Add(newObject);

            newPanel = newObject.GetComponent<MaterialPanel>();
            newPanel.Name = Material.GetName(materialType);
            newPanel.Density = Material.GetDensity(materialType);
            newPanel.Price = Material.GetPrice(materialType);
            newPanel.Icon = Material.GetImage(materialType);

            newButton = newObject.GetComponent<Button>();
            newButton.onClick.AddListener(() => BeamEdit.SetMaterial(materialType));
            newButton.onClick.AddListener(() => materialText.text = Material.GetName(materialType));
        }
    }
}
