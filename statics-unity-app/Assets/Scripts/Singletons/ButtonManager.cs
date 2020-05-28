using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject snowCanvas;
    public GameObject windCanvas;
    public GameObject materialCanvas;

    public void EnableSnowButtons()
    {
        snowCanvas.SetActive(true);
    }

    public void DisableSnowButtons()
    {
        snowCanvas.SetActive(false);
    }

    public void EnableWindButtons()
    {
        windCanvas.SetActive(true);
    }

    public void DisableWindButtons()
    {
        windCanvas.SetActive(false);
    }

    public void EnableMaterialButtons()
    {
        materialCanvas.SetActive(true);
    }

    public void DisableMaterialButtons()
    {
        materialCanvas.SetActive(false);
    }
}
