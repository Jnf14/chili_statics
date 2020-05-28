using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingPanel;

    public void Call(string path, string data)
    {
        StartCoroutine(Upload(path, data));
    }

    public static IEnumerator GetData(string path, Action<byte[]> callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            callback(www.downloadHandler.data);
        }
    }

    public static IEnumerator GetText(string path, Action<string> callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            callback(www.downloadHandler.text);
        }
    }

    public static IEnumerator PutText(string path, string text)
    {
        UnityWebRequest www = UnityWebRequest.Put(path, text);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
    }

    public static IEnumerator PutText(string path, string text, Action<string> callback)
    {
        UnityWebRequest www = UnityWebRequest.Put(path, text);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            callback(www.downloadHandler.text);
        }
    }

    public static IEnumerator PutData(string path, byte[] data)
    {
        UnityWebRequest www = UnityWebRequest.Put(path, data);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
    }

    public static IEnumerator PutData(string path, byte[] data, Action<byte[]> callback)
    {
        UnityWebRequest www = UnityWebRequest.Put(path, data);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            callback(www.downloadHandler.data);
        }
    }

  
    IEnumerator Upload(string path, string data)
    {
        loadingPanel.SetActive(true);
        
        UnityWebRequest www = UnityWebRequest.Put(path, data);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            List<PeakElementInternalForce> peakElementInternalForces;
            List<List<Vector3>> deformations;
            ParseData(www.downloadHandler.text, out peakElementInternalForces, out deformations);
            GameManager.instance.ElementPeakForces = peakElementInternalForces;
            GameManager.instance.SetDeformations(deformations);
        }
        loadingPanel.SetActive(false);
    }

    private void ParseData(string data, out List<PeakElementInternalForce> elementEndForces, out List<List<Vector3>> deformations)
    {
        UnityEngine.Debug.Log(String.Join("\n", data));
        string[] dataLines = data.Split('\n');
        string searchString = "P E A K   F R A M E   E L E M E N T   I N T E R N A L   F O R C E S";
        int startIndex = -1;
        for (int i = 0; i < dataLines.Length; i++)
        {
            if (dataLines[i].Length >= searchString.Length && dataLines[i].Substring(0, searchString.Length) == searchString)
            {
                startIndex = i + 2;
                break;
            }
        }
        elementEndForces = new List<PeakElementInternalForce>();
        string[] temp1, temp2;
        for (int i = startIndex; true; i += 2)//from PEAK FRAME ELEMENT INTERNAL FORCES until a hashtag appears
        {
            //Trim all whitespace
            dataLines[i] = System.Text.RegularExpressions.Regex.Replace(dataLines[i].Trim(), @"\s+", " ");
            dataLines[i + 1] = System.Text.RegularExpressions.Regex.Replace(dataLines[i + 1].Trim(), @"\s+", " ");

            if (dataLines[i].Length < 3)  //empty line, put 3 just to be sure
            {
                startIndex = i;
                break;
            }

            temp1 = dataLines[i].Split(' ');
            temp2 = dataLines[i + 1].Split(' ');

            int element = int.Parse(temp1[0]) - 1;
            List<double> values = new List<double>();
            for (int j = 2; j < 8; j++)
            {
                double x1 = double.Parse(temp1[j]);
                double x2 = double.Parse(temp2[j]);
                if (Math.Abs(x1) > Math.Abs(x2))
                {
                    values.Add(x1);
                }
                else
                {
                    values.Add(x2);
                }
            }

            elementEndForces.Add(new PeakElementInternalForce(
                element,
                values[0],
                values[1],
                values[2],
                values[3],
                values[4],
                values[5]
                ));

        }

        // UnityEngine.Debug.Log(String.Join("\n", data));

        deformations = new List<List<Vector3>>();
        searchString = "# element";
        for (int i = startIndex; i < dataLines.Length; i++)
        {
            if (dataLines[i].Length < searchString.Length || dataLines[i].Substring(0, searchString.Length) != searchString)
            {
                continue;
            }
            deformations.Add(new List<Vector3>());
            while (true)
            {
                i++;
                dataLines[i] = System.Text.RegularExpressions.Regex.Replace(dataLines[i].Trim(), @"\s+", " ");
                if (dataLines[i].Length < 3)   //empty line, put 3 just to be sure
                {
                    break;
                }
                temp1 = dataLines[i].Split(' ');
                deformations[deformations.Count - 1].Add(new Vector3(float.Parse(temp1[0]), float.Parse(temp1[1]), float.Parse(temp1[2])));
            }
        }
    }
}