using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressButton : MonoBehaviour
{
    [SerializeField]
    private Sprite activatedSprite;
    [SerializeField]
    private Sprite disabledSprite;
    [SerializeField]
    private Button axialButton;
    [SerializeField]
    private Button bendingButton;
    [SerializeField]
    private Button shearButton;
    private Image image;
    private Button button;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
        image = gameObject.GetComponent<Image>();
    }

    private void ClickHandler()
    {
        if (UIManager.instance.Mode == UIManager.UIMode.Stress)
        {
            UIManager.instance.SetUIMode(UIManager.UIMode.None);
            image.sprite = disabledSprite;
            axialButton.gameObject.SetActive(false);
            bendingButton.gameObject.SetActive(false);
            shearButton.gameObject.SetActive(false);
            GameManager.instance.HideStress();
        }
        else
        {
            UIManager.instance.SetUIMode(UIManager.UIMode.Stress);
            image.sprite = activatedSprite;
            axialButton.gameObject.SetActive(true);
            bendingButton.gameObject.SetActive(true);
            shearButton.gameObject.SetActive(true);
            axialButton.interactable = true;
            bendingButton.interactable = true;
            shearButton.interactable = true;
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
