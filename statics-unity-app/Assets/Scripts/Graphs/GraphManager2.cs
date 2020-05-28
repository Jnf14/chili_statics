// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class GraphManager : MonoBehaviour
// {
//     private RectTransform graphPoint;
//     private RectTransform graphContainer;
//     private RectTransform labelTemplateX;
//     private RectTransform labelTemplateY;
//     private LineRenderer lineRenderer;
//     private List<GameObject> gameObjectList;
//     // Limit number of values in the graphs
//     private int maxVisibleValues = 6;

//     // Graph points list
//     private List<GraphPoint> points;

//     // 

//     private void Awake()
//     {
//         lineRenderer = this.GetComponent<LineRenderer>();
//         graphContainer = GetComponent<RectTransform>();
//         labelTemplateX = graphContainer.Find("Label Template X").GetComponent<RectTransform>();
//         labelTemplateY = graphContainer.Find("Label Template Y").GetComponent<RectTransform>();
//         graphPoint = graphContainer.Find("Graph Point").GetComponent<RectTransform>();
//         gameObjectList = new List<GameObject>();

//         // List<float> valueList = new List<float>() { 5 ,98, 56, 45, 30, 22, 17, 15, 25, 37, 40, 36, 33 };
//         // ShowGraph(valueList);
//     }

    // private void CreateCircle(int index, Vector2 anchoredPosition)
    // {
    //     RectTransform pt = Instantiate(graphPoint);
    //     pt.gameObject.SetActive(true);
        
    //     GameObject gameObject = pt.gameObject;
    //     gameObjectList.Add(gameObject);
    //     gameObject.transform.SetParent(graphContainer, false);
    //     RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
    //     rectTransform.anchoredPosition = anchoredPosition;
    //     rectTransform.anchoredPosition3D = rectTransform.anchoredPosition3D + new Vector3(0,0,-2);
    //     rectTransform.sizeDelta = new Vector2(11, 11);
    //     rectTransform.anchorMin = new Vector2(0, 0);
    //     rectTransform.anchorMax = new Vector2(0, 0);
    //     lineRenderer.SetPosition(index, rectTransform.anchoredPosition3D + new Vector3(0,0,1));
    // }

//     public void ShowGraph(List<float> valueList){   
//         ResetGraph();

//         maxVisibleValues = valueList.Count;

//         float graphWidth = graphContainer.sizeDelta.x;
//         float graphHeight = graphContainer.sizeDelta.y;
        
//         lineRenderer.positionCount = valueList.Count;

//         // X axis scale
//         float xSize = graphWidth / (valueList.Count - 1);

//         // Y axis scale 
//         float yMax = valueList[0];
//         float yMin = valueList[0];
//         for (int i = Mathf.Max(valueList.Count - maxVisibleValues, 0); i < valueList.Count; i++){
//             float v = valueList[i];
//             yMax = Mathf.Max(yMax, v);
//             yMin = Mathf.Min(yMin, v);
//         }

//         float yDiff = yMax - yMin;
//         if (yDiff <= 0){
//             yDiff = 5f;
//         }
//         yMax = yMax + yDiff * 0.1f;
//         yMin = yMin - yDiff * 0.1f;

//         int xIndex = 0;
//         for (int i = Mathf.Max(valueList.Count - maxVisibleValues, 0); i < valueList.Count; i++){
//             float xPos = xIndex * xSize;
//             float yPos = ((valueList[i]-yMin) / (yMax-yMin)) * graphHeight;

//             CreateCircle(i, new Vector2(xPos, yPos));

//             if (i != 0){
//                 RectTransform labelX = Instantiate(labelTemplateX);
//                 labelX.gameObject.SetActive(true);
//                 labelX.gameObject.transform.SetParent(graphContainer, false);
//                 labelX.anchoredPosition3D = new Vector3(xPos, -12f, labelTemplateX.anchoredPosition3D.z);
//                 labelX.GetComponent<Text>().text = i.ToString();
//                 gameObjectList.Add(labelX.gameObject);
//             }

//             xIndex++;
//         }

//         int separatorCount = 10;
//         for (int i = 0; i <= separatorCount; i++){
//             RectTransform labelY = Instantiate(labelTemplateY);
//             labelY.gameObject.SetActive(true);
//             labelY.gameObject.transform.SetParent(graphContainer, false);
//             float normalizedValue = i*1f/separatorCount;
//             labelY.anchoredPosition3D = new Vector3(-7f, normalizedValue*graphHeight, labelTemplateX.anchoredPosition3D.z);
//             labelY.GetComponent<Text>().text = (yMin + normalizedValue * (yMax-yMin)).ToString("0");
//             gameObjectList.Add(labelY.gameObject);
//         }
//     }

//     public void ResetGraph()
//     {
//         // Delete every graph each time we redraw a new one
//         foreach (GameObject o in gameObjectList){
//             Destroy(o);
//         }
//         gameObjectList.Clear();
//         lineRenderer.positionCount = 2;
//         lineRenderer.SetPosition(0, Vector3.zero);
//         lineRenderer.SetPosition(1, Vector3.zero);
//     }


//     // Start is called before the first frame update
//     void Start()
//     {

//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }

