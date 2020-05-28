using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringRegulation : MonoBehaviour
{
    private int force = 0;
    private float startTime;
    private readonly float lerpCoef = 1f;

    private readonly Vector3 tensed = new Vector3(0.1f, 0.16f, 0.12f);
    private readonly Vector3 normal = new Vector3(0.1f, 0.1f, 0.12f);
    private readonly Vector3 compressed = new Vector3(0.1f, 0.05f, 0.12f);
    private Vector3 startScale;

    public int Force
    {
        set
        {
            startScale = transform.localScale;
            startTime = Time.time;
            force = value;
        }
    }

    private void Start()
    {
        startScale = normal;
    }

    void Update()
    {
        Vector3 targetScale;
        switch (force)
        {
            case -1:
                targetScale = compressed;
                break;
            case 1:
                targetScale = tensed;
                break;
            case 0:
                targetScale = normal;
                break;
            default:
                targetScale = normal;
                break;
        }
        transform.localScale = Vector3.Lerp(startScale, targetScale, (Time.time - startTime) * lerpCoef);
    }
}
