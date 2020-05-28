using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(GameManager.instance.Save);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
