using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
    }

    private void ClickHandler()
    {
        if (UIManager.instance.Mode == UIManager.UIMode.CameraRotation)
        {
            Camera.main.GetComponent<CameraDragRotation>().Dragging = false;
            UIManager.instance.SetUIMode(UIManager.UIMode.None);
        }
        else
        {
            Camera.main.GetComponent<CameraDragRotation>().Dragging = true;
            UIManager.instance.SetUIMode(UIManager.UIMode.CameraRotation);
        }
    }

    private void Start()
    {
        button.onClick.AddListener(ClickHandler);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
