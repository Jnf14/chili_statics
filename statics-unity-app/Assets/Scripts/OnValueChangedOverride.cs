using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnValueChangedOverride : MonoBehaviour {

    Dropdown dropdown;

    [SerializeField]
    private List<Material> materials;

	void Start()
    {
        dropdown = GetComponent<Dropdown>();

        dropdown.onValueChanged.AddListener(DropdownValueChanged);
	}

    private void DropdownValueChanged(int change)
    {
        MaterialType newMaterialType;
        switch (change)
        {
            case 0:
                newMaterialType = MaterialType.C18;
                break;
            case 1:
                newMaterialType = MaterialType.C22;
                break;
            case 2:
                newMaterialType = MaterialType.C30;
                break;
            case 3:
                newMaterialType = MaterialType.C40;
                break;
            case 4:
                newMaterialType = MaterialType.D30;
                break;
            case 5:
                newMaterialType = MaterialType.D50;
                break;
            case 6:
                newMaterialType = MaterialType.D70;
                break;
            default:
                return;
        }
        foreach (Material material in materials)
        {
            material.Type = newMaterialType;
        }
        GameManager.CalculateStatics();
    }
}
