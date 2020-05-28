using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformationManager : MonoBehaviour
{
    public static DeformationManager instance;

    [SerializeField]
    private List<DeformationExaggerator> deformationLines;
    [SerializeField]
    private float exagg = 1.0f;
    [SerializeField]
    private TMPro.TextMeshProUGUI sliderText;

    public float Exagg
    {
        get
        {
            return exagg;
        }

        set
        {
            exagg = value;
            sliderText.text = exagg.ToString("n2") + "x";
            foreach (DeformationExaggerator line in deformationLines)
            {
                line.Recalculate();
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void SetDeformations(List<List<Vector3> > vectors)
    {
        for (int i = 0; i < deformationLines.Count; i++)
        {
            deformationLines[i].SetPositions(vectors[i]);
        }
    }
}
