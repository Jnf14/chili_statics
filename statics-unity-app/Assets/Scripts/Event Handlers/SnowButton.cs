﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnowButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() => { SnowManager.instance.Snow = !SnowManager.instance.Snow; });
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
