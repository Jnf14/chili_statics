using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebUICanvas : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
    }

    private void ClickHandler()
    {
        if (UIManager.instance.Mode == UIManager.UIMode.SnowWind)
        {
            UIManager.instance.SetUIMode(UIManager.UIMode.None);
        }
        else
        {
            UIManager.instance.SetUIMode(UIManager.UIMode.SnowWind);
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
