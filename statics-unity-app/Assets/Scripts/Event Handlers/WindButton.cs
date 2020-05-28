using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() => { WindManager.instance.Strength = 0; });
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
