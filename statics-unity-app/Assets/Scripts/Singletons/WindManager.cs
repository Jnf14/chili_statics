using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindManager : MonoBehaviour
{
    public static WindManager instance;

    [SerializeField]
    private float strength;
    [SerializeField]
    public Slider slider;

    public float Strength
    {
        get
        {
            return strength;
        }
        set
        {
            strength = value;
            if (System.Math.Abs(slider.value - value) > 0.01)
            {
                slider.value = value;
            }
            SnowManager.instance.Angle = value * 10;
        }
    }

    private void Awake()
    {
        instance = this;
    }
}
