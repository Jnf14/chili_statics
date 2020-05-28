using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public enum UIMode
    {
        None,
        ObjectPanel,
        BeamPanel,
        CameraRotation,
        SnowWind,
        Stress
    }

    [SerializeField]
    private GameObject clickBlocker;
    [SerializeField]
    private GameObject snowWindButton;
    [SerializeField]
    private GameObject snowWindPanels;
    [SerializeField]
    private GameObject objectPanel;
    [SerializeField]
    private GameObject beamPanel;
    [SerializeField]
    private GameObject saveButton;
    [SerializeField]
    private GameObject stressButton;
    [SerializeField]
    private GameObject addButton;
    [SerializeField]
    private GameObject rotateButton;
    [SerializeField]
    private GameObject deformationSlider;
    [SerializeField]
    private GameObject deformationLines;
    [SerializeField]
    private GameObject gameModeButton;
    [SerializeField]
    private GameObject trash;

    private UIMode mode = UIMode.None;

    private bool canSave = false;

    public UIMode Mode
    {
        get
        {
            return mode;
        }
    }

    public void SetUIMode(UIMode type)
    {
        mode = type;
        switch (type)
        {
            case UIMode.None:
                objectPanel.SetActive(false);
                beamPanel.SetActive(false);
                stressButton.SetActive(true);
                addButton.SetActive(true);
                snowWindButton.SetActive(true);
                snowWindPanels.SetActive(false);
                //snowWindButton.transform.rotation = Quaternion.identity;
                rotateButton.SetActive(true);
                clickBlocker.SetActive(false);
                deformationSlider.SetActive(false);
                deformationLines.SetActive(false);
                //gameModeButton.SetActive(true);
                break;
            case UIMode.ObjectPanel:
                objectPanel.SetActive(true);
                beamPanel.SetActive(false);
                stressButton.SetActive(false);
                addButton.SetActive(true);
                snowWindButton.SetActive(false);
                snowWindPanels.SetActive(false);
                rotateButton.SetActive(false);
                clickBlocker.SetActive(true);
                deformationSlider.SetActive(false);
                deformationLines.SetActive(false);
                //gameModeButton.SetActive(false);
                break;
            case UIMode.BeamPanel:
                objectPanel.SetActive(false);
                beamPanel.SetActive(true);
                stressButton.SetActive(false);
                addButton.SetActive(false);
                snowWindButton.SetActive(false);
                snowWindPanels.SetActive(false);
                rotateButton.SetActive(false);
                clickBlocker.SetActive(true);
                deformationSlider.SetActive(false);
                deformationLines.SetActive(false);
                //gameModeButton.SetActive(false);
                break;
            case UIMode.CameraRotation:
                objectPanel.SetActive(false);
                beamPanel.SetActive(false);
                stressButton.SetActive(false);
                addButton.SetActive(false);
                snowWindButton.SetActive(false);
                snowWindPanels.SetActive(false);
                rotateButton.SetActive(true);
                clickBlocker.SetActive(true);
                deformationSlider.SetActive(false);
                deformationLines.SetActive(false);
                //gameModeButton.SetActive(false);
                break;
            case UIMode.SnowWind:
                objectPanel.SetActive(false);
                beamPanel.SetActive(false);
                stressButton.SetActive(false);
                addButton.SetActive(false);
                snowWindButton.SetActive(true);
                snowWindPanels.SetActive(true);
                //snowWindButton.transform.rotation = Quaternion.Euler(0, 0, 90);
                rotateButton.SetActive(false);
                clickBlocker.SetActive(true);
                deformationSlider.SetActive(false);
                deformationLines.SetActive(false);
                //gameModeButton.SetActive(false);
                break;
            case UIMode.Stress:
                objectPanel.SetActive(false);
                beamPanel.SetActive(false);
                stressButton.SetActive(true);
                addButton.SetActive(false);
                snowWindButton.SetActive(false);
                snowWindPanels.SetActive(false);
                rotateButton.SetActive(false);
                clickBlocker.SetActive(true);
                deformationSlider.SetActive(true);
                deformationLines.SetActive(true);
                //gameModeButton.SetActive(false);
                break;
            default:
                break;
        }
        
        // if (GameModeManager.instance.GameMode == GameMode.AR)
        // {
        //     addButton.SetActive(false);
        //     rotateButton.SetActive(false);
        //     trash.SetActive(false);
        // }
        // else
        // {
        //     trash.SetActive(true);
        // }
    }

    public void Refresh()
    {
        SetUIMode(mode);
    }

    public void EnableSaving()
    {
        canSave = true;
        Refresh();
    }

    private void Awake()
    {
        instance = this;
    }
}