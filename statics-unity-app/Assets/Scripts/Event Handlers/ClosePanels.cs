using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClosePanels : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() => UIManager.instance.SetUIMode(UIManager.UIMode.None));
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
