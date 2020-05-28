using UnityEngine;
using UnityEngine.UI;

public class BeamSelector : MonoBehaviour
{
    Dropdown m_Dropdown;

    [SerializeField]
    private GraphManager graphManager;

    void Start()
    {
        //Fetch the Dropdown GameObject
        m_Dropdown = GetComponent<Dropdown>();
        //Add listener for when the value of the Dropdown changes, to take action
        m_Dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(m_Dropdown);
        });
    }

    //Ouput the new value of the Dropdown into Text
    void DropdownValueChanged(Dropdown change)
    {
        graphManager.SelectBeam(change.value);
    }
}