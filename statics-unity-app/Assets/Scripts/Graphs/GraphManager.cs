using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphManager : MonoBehaviour
{
    [SerializeField]
    private int graphId;
    [SerializeField]
    private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform graphPoint;
    private LineRenderer lineRenderer;
    private List<GameObject> gameObjectList;
    // Limit number of values in the graphs
    private int maxVisibleValues = 6;

    // Graph points list
    private List<GraphPoint> points;

    // Values counter (For LOG)
    private int valuesCounter;

    // Drawn values list
    private List<float> valueList;

    // Axial
    private List<List<float>> axialStresses;

    // Shear
    private List<List<float>> shearStresses;

    // Bending
    private List<List<float>> bendingStresses;

    // Loads abstract in string format
    private List<List<string>> loads;

    // Stress selector (0 = axial, 1 = shear, 2 = bending)
    private int stressSelector;

    // Beam selector (beamId - 1)
    private int beamSelector;

    private void Awake()
    {
        valuesCounter = 0;
        stressSelector = 0;
        beamSelector = 0;

        lineRenderer = this.GetComponent<LineRenderer>();
        graphContainer = GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("Label Template X").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("Label Template Y").GetComponent<RectTransform>();
        graphPoint = graphContainer.Find("Graph Point").GetComponent<RectTransform>();
        gameObjectList = new List<GameObject>();

        // Initialize stress lists
        axialStresses = new List<List<float>>();
        shearStresses = new List<List<float>>();
        bendingStresses = new List<List<float>>();

        // Initialize loads list
        loads = new List<List<string>>();

        // List<float> valueList = new List<float>() { 5 ,98, 56, 45, 30, 22, 17, 15, 25, 37, 40, 36, 33 };
        // ShowGraph(valueList);
    }

    public void AddGraphLog(List<float> axial, List<float> shear, List<float> bending, List<string> loads)
    {
        this.valuesCounter++;
        this.axialStresses.Add(axial);
        this.shearStresses.Add(shear);
        this.bendingStresses.Add(bending);
        this.loads.Add(loads);

        DrawGraph();
    }

    private void CreateCircle(int index, Vector2 anchoredPosition)
    {
        // Instantiate the graphPoint serialized object
        RectTransform pt = Instantiate(graphPoint);

        // Choose stress & point loads label on graph points
        pt.GetComponent<GraphPoint>().SetGraphPointValues(index, stressSelector, valueList[index], loads[index]);
        pt.gameObject.SetActive(true);
        
        GameObject gameObject = pt.gameObject;
        gameObjectList.Add(gameObject);
        gameObject.transform.SetParent(graphContainer, false);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.anchoredPosition3D = rectTransform.anchoredPosition3D + new Vector3(0,0,-2);
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        lineRenderer.SetPosition(index, rectTransform.anchoredPosition3D + new Vector3(0,0,1));
    }

    private void ShowGraph(List<float> valueList){   

        // Delete every graph each time we redraw a new one
        foreach (GameObject o in gameObjectList){
            Destroy(o);
        }
        gameObjectList.Clear();

        // Save the shown values
        this.valueList = valueList;

        maxVisibleValues = valueList.Count;

        // Get graph width and height
        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;
        
        // Set LineRenederer # of positions
        lineRenderer.positionCount = valueList.Count;

        // X axis scale
        float xSize = graphWidth / (valueList.Count - 1);

        // Y axis scale 
        float yMax = valueList[0];
        float yMin = valueList[0];
        for (int i = Mathf.Max(valueList.Count - maxVisibleValues, 0); i < valueList.Count; i++){
            float v = valueList[i];
            yMax = Mathf.Max(yMax, v);
            yMin = Mathf.Min(yMin, v);
        }

        float yDiff = yMax - yMin;
        if (yDiff <= 0){
            yDiff = 5f;
        }
        yMax = yMax + yDiff * 0.1f;
        yMin = yMin - yDiff * 0.1f;

        if(yMax != yMin){
            int xIndex = 0;
            for (int i = Mathf.Max(valueList.Count - maxVisibleValues, 0); i < valueList.Count; i++){
                float xPos = xIndex == 0 ? 0 : xIndex * xSize;
                float yPos = ((valueList[i]-yMin) / (yMax-yMin)) * graphHeight;
                CreateCircle(i, new Vector2(xPos, yPos));

                if (i != 0){
                    RectTransform labelX = Instantiate(labelTemplateX);
                    labelX.gameObject.SetActive(true);
                    labelX.gameObject.transform.SetParent(graphContainer, false);
                    labelX.anchoredPosition3D = new Vector3(xPos, -12f, labelTemplateX.anchoredPosition3D.z);
                    labelX.GetComponent<Text>().text = i.ToString();
                    gameObjectList.Add(labelX.gameObject);
                }

                xIndex++;
            }

            int separatorCount = 10;
            for (int i = 0; i <= separatorCount; i++){
                RectTransform labelY = Instantiate(labelTemplateY);
                labelY.gameObject.SetActive(true);
                labelY.gameObject.transform.SetParent(graphContainer, false);
                float normalizedValue = i*1f/separatorCount;
                labelY.anchoredPosition3D = new Vector3(-7f, normalizedValue*graphHeight, labelTemplateY.anchoredPosition3D.z);
                float labelValue = (yMin + normalizedValue * (yMax-yMin));

                // Y axis text
                Text textComponent = labelY.GetComponent<Text>();
                string labelText;
                if(Mathf.Abs(labelValue) >= 1000000f){
                    labelText = (labelValue / 1000000).ToString("0.0M");
                    textComponent.fontSize = 19;
                }
                else if(Mathf.Abs(labelValue) < 1000000f && Mathf.Abs(labelValue) >= 100000f){
                    labelText = (labelValue / 1000).ToString("0k");
                    textComponent.fontSize = 18;
                }
                else if(Mathf.Abs(labelValue) < 100000f && Mathf.Abs(labelValue) >= 1000f){
                    labelText = (labelValue / 1000).ToString("0.0k");
                    textComponent.fontSize = 19;
                }
                else {
                    labelText = labelValue.ToString("0");
                }

                
                textComponent.text = labelText;
                // labelY.sizeDelta = new Vector2(textComponent.preferredWidth + 2f, textComponent.preferredHeight + 2f);

                gameObjectList.Add(labelY.gameObject);
            }
        }
    }

    private void DrawGraph()
    {
        List<float> valueList = new List<float>();
        for(int i = 0; i < valuesCounter; i++){
            switch(stressSelector)
            {   
                // Axial
                case 0 : 
                    valueList.Add(axialStresses[i][beamSelector]);
                    break;
                case 1 :
                    valueList.Add(shearStresses[i][beamSelector]);
                    break;
                case 2 :
                    valueList.Add(bendingStresses[i][beamSelector]);
                    break;

            }
        }

        ShowGraph(valueList);
    }

    public void ResetGraph()
    {
        // Delete every graph each time we redraw a new one
        foreach (GameObject o in gameObjectList){
            Destroy(o);
        }
        gameObjectList.Clear();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);

        valuesCounter = 0;
        axialStresses = new List<List<float>>();
        shearStresses = new List<List<float>>();
        bendingStresses = new List<List<float>>();
        loads = new List<List<string>>();
    }

    // Called by dropdown for Beam selection
    public void SelectBeam(int beamId)
    {
        this.beamSelector = beamId;
        DrawGraph();
        GameManager.instance.ExportLog("beam", "select", "Selected beam " + beamId.ToString() + " on graph " + graphId.ToString());
    }

    // Called by dropdown for Stress selection
    public void SelectStress(int stressId)
    {
        this.stressSelector = stressId;
        DrawGraph();
        string stressType;
        if(stressId == 0)
            stressType = "axial";
        else if(stressId == 1)
            stressType = "shear";
        else
            stressType = "bending";

        GameManager.instance.ExportLog("stress", "select", "Selected " + stressType + " stress on graph " + graphId.ToString());
    }
}