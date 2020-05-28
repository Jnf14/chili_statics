using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnowSliderDrag : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void ValueChangeHandler(float value)
    {
        if (System.Math.Abs(value - SnowManager.instance.MaxHeight) > 0.01)
        {
            SnowManager.instance.MaxHeight = value;
        }
    }

    private void Start()
    {
        slider.onValueChanged.AddListener(ValueChangeHandler);
    }
}
