using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeButton : MonoBehaviour
{
    [SerializeField]
    private Sprite arSprite;
    [SerializeField]
    private Sprite webSprite;
    private Image image;
    private Button button;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
        image = gameObject.GetComponent<Image>();
    }

    private void ClickHandler()
    {
        if (GameModeManager.instance.SwitchMode() == GameMode.AR)
        {
            image.sprite = arSprite;
        }
        else
        {
            image.sprite = webSprite;
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
