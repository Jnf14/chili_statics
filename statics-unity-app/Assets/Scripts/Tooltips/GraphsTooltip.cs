using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphsTooltip : MonoBehaviour {

    private static GraphsTooltip instance;

    [SerializeField]
    private Camera graphCamera;
    [SerializeField]
    private RectTransform canvasRectTransform;

    private Text tooltipText;
    private RectTransform backgroundRectTransform;

    private void Awake() {
        instance = this;
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        tooltipText = transform.Find("Text").GetComponent<Text>();

        HideTooltip();
        // ShowTooltip("Random tooltip text");
        
    }

    private void Update() {
        Vector2 localPoint;
        // Follow mouse position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, graphCamera, out localPoint);
        transform.localPosition = localPoint;

        // Always in screen
        Vector3 anchoredPos = transform.GetComponent<RectTransform>().anchoredPosition3D;
        if (anchoredPos.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width){
            anchoredPos.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }
        if (anchoredPos.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height){
            anchoredPos.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }
        anchoredPos = anchoredPos + new Vector3(0,0,-2f); // To be on top of other object in scene
        transform.GetComponent<RectTransform>().anchoredPosition3D = anchoredPos;
    }

    private void ShowTooltip(string tooltipString) {
        gameObject.SetActive(true);
        canvasRectTransform.SetAsLastSibling();

        tooltipText.text = tooltipString;
        float textPaddingSize = 10f;
        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 2f, tooltipText.preferredHeight + textPaddingSize * 2f);
        backgroundRectTransform.sizeDelta = backgroundSize;
        this.Update();
    }

    private void HideTooltip() {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(string tooltipString) {
        instance.ShowTooltip(tooltipString);
    }

    public static void HideTooltip_Static() {
        instance.HideTooltip();
    }

}
