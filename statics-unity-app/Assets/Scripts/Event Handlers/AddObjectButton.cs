using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddObjectButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
    }

    private void ClickHandler()
    {
        if (UIManager.instance.Mode == UIManager.UIMode.ObjectPanel)
        {
            UIManager.instance.SetUIMode(UIManager.UIMode.None);
            button.gameObject.SetActive(false);
        }
        else
        {
            UIManager.instance.SetUIMode(UIManager.UIMode.ObjectPanel);
            button.gameObject.SetActive(false);
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
