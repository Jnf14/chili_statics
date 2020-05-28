using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindSliderDrag : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void ValueChangeHandler(float value)
    {
        if (System.Math.Abs(value - WindManager.instance.Strength) > 0.01)
        {
            WindManager.instance.Strength = value;
        }
    }

    private void Start()
    {
        slider.onValueChanged.AddListener(ValueChangeHandler);
    }
}
