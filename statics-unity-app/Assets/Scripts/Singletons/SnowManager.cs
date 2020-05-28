using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnowManager : MonoBehaviour
{
    public static SnowManager instance;

    [SerializeField]
    private bool snow;
    [SerializeField]
    private float density;         //Snow density: 300kg/m^3 https://www.sciencelearn.org.nz/resources/1391-snow-and-ice-density
    [SerializeField]
    private float maxHeight;
    [SerializeField]
    private float snowPileScale;
    [SerializeField]
    private float angle;
    private string originName;
    public List<GameObject> elements;
    public ParticleSystem snowParticles;
    public List<GameObject> snowPiles;
    public List<GameObject> snowCurves;

    private bool shouldRecalculate = false;
    private float inactiveTime = 60;
    private float waitTime = 0.5f;

    [SerializeField]
    private Slider slider;

    public bool Snow
    {
        get
        {
            return snow;
        }

        set
        {
            if (snow != value)
            {
                snow = value;
                    slider.gameObject.SetActive(value);
                RescaleSnowPiles();
                foreach (GameObject snowPile in snowPiles)
                {
                    snowPile.SetActive(value);
                }
                foreach (GameObject snowCurve in snowCurves)
                {
                    snowCurve.SetActive(value);
                }
                shouldRecalculate = true;
                inactiveTime = 0;
                if (value)
                {
                    snowParticles.Play();
                }
                else
                {
                    snowParticles.Stop();
                }
            }
        }
    }

    public float Angle
    {
        set
        {
            if (angle != value)
            {
                angle = value;
                snowParticles.transform.rotation = Quaternion.Euler(-90, -value, 0);
                if (Snow)
                {
                    RescaleSnowPiles();
                    shouldRecalculate = true;
                    inactiveTime = 0;
                }
            }
        }

        get
        {
            return angle;
        }
    }

    public float Density
    {
        get
        {
            return density;
        }
    }

    public string OriginName
    {
        get
        {
            return originName;
        }
    }

    public float MaxHeight
    {
        get
        {
            return maxHeight;
        }

        set
        {
            if (maxHeight != value)
            {
                maxHeight = value;
                if (System.Math.Abs(slider.value - value) > 0.01)
                {
                    slider.value = value;
                }

                //18 to 22 for value == 500, 36 to 44 for value == 1000
                snowParticles.GetComponent<ParticleSystemInterface>().SetEmissionRateOverTime(9 * value / 250, 11 * value / 250);
                RescaleSnowPiles();
                shouldRecalculate = true;
                inactiveTime = 0;
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        originName = "snow";
        angle = 0;
        density = 3e-7f;
        snow = false;
        snowPileScale = 0.0025f;
        maxHeight = 500;
        RescaleSnowPiles();
	}

    private void Update()
    {
        inactiveTime += Time.deltaTime;
        if (shouldRecalculate && inactiveTime > waitTime)
        {
            RecalculateLoads();
            shouldRecalculate = false;
        }
    }

    private void RescaleSnowPiles()
    {
        foreach (GameObject snowPile in snowPiles)
        {
            float newXScale = snowPileScale * MaxHeight * (float)System.Math.Cos((Angle - snowPile.transform.rotation.eulerAngles.y) * System.Math.PI / 180);
            snowPile.transform.Translate((newXScale - snowPile.transform.localScale.x) / 2, 0, 0, Space.Self);
            snowPile.transform.localScale = new Vector3(newXScale, snowPile.transform.localScale.y, snowPile.transform.localScale.z);
        }
        foreach(GameObject snowCurve in snowCurves)
        {
            snowCurve.GetComponent<ScriptedMeshObjectCreator>().Recalculate();
        }
    }

    private void RecalculateLoads()
    {
        GameManager.RemoveUniformDistributedLoadOrigin(OriginName);
        if (Snow)
        {
            foreach (GameObject element in elements)
            {
                float componentAngle = element.GetComponent<ElementNodes>().getAngle();
                int id = element.GetComponent<IdComponent>().id;
                double width = element.GetComponent<CrossSection>().Width;
                GameManager.AddUniformDistributedLoad(new UniformDistributedLoad(id, Density * MaxHeight * (float)width * (float)System.Math.Cos((Angle - componentAngle) * System.Math.PI / 180), componentAngle, OriginName, element));
                // GameManager.AddUniformDistributedLoad(new UniformDistributedLoad(id, Density * MaxHeight * (float)width, componentAngle, OriginName));

            }
        }
        GameManager.CalculateStatics();
    }
}
