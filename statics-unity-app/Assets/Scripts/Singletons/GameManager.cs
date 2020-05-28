using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Diagnostics;

public class GameManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void GameManagerStarted();

    public Vector3[][] starters;

    public static GameManager instance;

    public static readonly float arrowDistance = 3;
    public static readonly float arrowHeight = 1.5f;

    private static readonly float nodeToUnityFactor = 0.006f;

    public GameObject vectorArrow;

    [SerializeField]
    private List<GameObject> elements;
    [SerializeField]
    private List<SpringRegulation> springs;
    [SerializeField]
    private List<FloatingText> stressTexts;

    [SerializeField]
    private List<Node> joints;

    [SerializeField]
    private List<GraphManager> graphManagers;
    [SerializeField]
    private List<int> beamSelector;

    private bool pointLoadsChanged = false;
    private bool shouldRecalculate = false;
    private float inactiveTime = 60;
    private float waitTime = 0.5f;
    [SerializeField]
    private float staticsExagg;
    [SerializeField]
    private float staticsdx;

    private List<Material> materials = new List<Material>();
    private List<CrossSection> crossSections = new List<CrossSection>();
    private List<ElementNodes> elementNodes = new List<ElementNodes>();
    private List<ElementArrows> elementArrows = new List<ElementArrows>();
    private List<float> axialStress;
    private List<float> bendingStress;
    private List<float> shearStress;

    private Dictionary<int, PointLoad> pointLoads = new Dictionary<int, PointLoad>();
    private List<UniformDistributedLoad> uniformDistributedLoads = new List<UniformDistributedLoad>();
    private List<UniformDistributedLoad> distributedToRemove = new List<UniformDistributedLoad>();
    
    private List<PeakElementInternalForce> elementPeakForces;

    private string savePath;
    private string loadPath;
    private string frame3DDPath; // Path for frame3DD node app
    private string mysqlPath; // Path for mysql node app

    public List<PeakElementInternalForce> ElementPeakForces
    {
        set
        {
            elementPeakForces = value;
            UpdateBeamArrows();
            //UpdateSprings();
            UpdateStress();
            UpdateGraphs();
        }
    }

    private void Awake()
    {
        instance = this;
        foreach (GameObject element in elements)
        {
            materials.Add(element.GetComponent<Material>());
            crossSections.Add(element.GetComponent<CrossSection>());
            elementNodes.Add(element.GetComponent<ElementNodes>());
            elementArrows.Add(element.GetComponent<ElementArrows>());
        }
    }

    private void Start()
    {
        SetFrame3DDPath("http://127.0.0.1:1337/");
        SetMySQLPath("http://127.0.0.1:1338/");
        SetSavePath("https://62000bf6.ngrok.io/hooks/realto/statics/save/muyvwuXpAWeiFjGtJ_1553079508038/task/jyKFCyzmhmTgNLGXr");
        LoadFrom("https://62000bf6.ngrok.io/hooks/realto/statics/load/1234");
    }

    private void Update()
    {
        inactiveTime += Time.deltaTime;
        if (pointLoadsChanged || pointLoadsChangedTransform())
        {
            inactiveTime = 0;
            shouldRecalculate = true;
            pointLoadsChanged = false;
        }
        if (instance.frame3DDPath != null && inactiveTime >= waitTime && shouldRecalculate)
        {
            CalculateStatics();
            shouldRecalculate = false;
        }
    }

    private bool pointLoadsChangedTransform()
    {
        bool hasChanged = false;
        foreach (KeyValuePair<int, PointLoad> pointLoad in pointLoads)
        {
            if (pointLoad.Value.transform.hasChanged)
            {
                hasChanged = true;
                pointLoad.Value.transform.hasChanged = false;
            }
        }
        return hasChanged;
    }

    public static void CalculateStatics()
    {
        List<string> data = new List<string>();
        data.Add("Statics");

        // Number of nodes
        data.Add(instance.joints.Count.ToString());
        // Node #, X, Y, Z, radius
        for (int i = 0; i < instance.joints.Count; i++)
        {
            data.Add("" + (i + 1) + " " + instance.joints[i].x + " " + instance.joints[i].y + " " + instance.joints[i].z + " " + instance.joints[i].r);
        }

        // Number of nodes with Reactions
        data.Add(instance.joints.Count.ToString());
        // Node #, RX, RY, RZ, RXX, RYY, RZZ
        for (int i = 0; i < instance.joints.Count; i++)
        {
            data.Add("" + (i + 1) + " " + (instance.joints[i].Fx ? "1" : "0") + " " + (instance.joints[i].Fy ? "1" : "0") + " " + (instance.joints[i].Fz ? "1" : "0") + " " 
                + (instance.joints[i].Mx ? "1" : "0") + " " + (instance.joints[i].My ? "1" : "0") + " " + (instance.joints[i].Mz ? "1" : "0"));
        }

        // Number of frame elements
        data.Add(instance.elements.Count.ToString());
        
        for (int i = 0; i < instance.elements.Count; i++)
        {
            data.Add("" + (i + 1) + " " + (int.Parse(instance.elementNodes[i].n1.name.Substring(5)) + 1) + " " + (int.Parse(instance.elementNodes[i].n2.name.Substring(5)) + 1)
                + " " + instance.crossSections[i].Ax + " " + instance.crossSections[i].Asy + " " + instance.crossSections[i].Asz
                + " " + instance.crossSections[i].Jx + " " + instance.crossSections[i].Iy + " " + instance.crossSections[i].Iz
                + " " + instance.materials[i].Constants.Young + " " + instance.materials[i].Constants.G + " 0 " + instance.materials[i].Constants.density);
        }

        data.Add("1");      //shear
        data.Add("1");      //geom
        data.Add(instance.staticsExagg.ToString());   //exagg_static
        data.Add("1.0");    //scale
        data.Add(instance.staticsdx.ToString());   //dx
        data.Add("1");      //load data
        data.Add("0 0 -9.81");  //gravity
        data.Add("0");      //loaded nodes

        data.Add(instance.uniformDistributedLoads.Count.ToString());      //uniformly distributed loads
        foreach (UniformDistributedLoad uniformDistributedLoad in instance.uniformDistributedLoads)
        {
            data.Add("" + (uniformDistributedLoad.target + 1) + " " + uniformDistributedLoad.Px + " " + uniformDistributedLoad.Py + " " + uniformDistributedLoad.Pz);
        }

        data.Add("0");      //trapezoidally distributed loads

        data.Add(instance.pointLoads.Count.ToString());     //point loads
        foreach (KeyValuePair<int, PointLoad> keyValuePair in instance.pointLoads)
        {
            data.Add("" + (keyValuePair.Value.target + 1) + " " + keyValuePair.Value.Px + " " + keyValuePair.Value.Py + " " + keyValuePair.Value.Pz + " " + keyValuePair.Value.x);
        }

        data.Add("0");      //temperature changes
        data.Add("0");      //prescribed displacements
        data.Add("0");
        data.Add("0");
        data.Add("0"); 

        // UnityEngine.Debug.Log(String.Join("\n", data));

        instance.gameObject.GetComponent<WebRequest>().Call(instance.frame3DDPath, string.Join("\n", data));
    }

    public static void AddPointLoad(PointLoad load)
    {
        instance.pointLoads.Add(int.Parse(load.name.Substring(7)), load);
        load.SetId(instance.pointLoads.Count);
        instance.pointLoadsChanged = true;
        GameManager.instance.ExportLog("point load", "attach", "Attached "+ load.typeText +" "+ load.id + " on beam " + load.beamId + " at " + load.positionFromLeftEnd.ToString("0.0") + "%");
    }

    public static void RemovePointLoad(PointLoad load)
    {
        instance.pointLoads.Remove(int.Parse(load.name.Substring(7)));
        instance.pointLoadsChanged = true;
        GameManager.instance.ExportLog("point load", "detach", "Detached "+ load.typeText + " " + load.id +  " from beam " + load.beamId);
    }

    public static void AddUniformDistributedLoad(UniformDistributedLoad load)
    {
        instance.uniformDistributedLoads.Add(load);
        GameManager.instance.ExportLog("uniform load", "add", "Added uniform load on beam " + load.beamId);
    }

    public static void RemoveUniformDistributedLoadOrigin(string origin)
    {
        foreach (UniformDistributedLoad uniformDistributedLoad in instance.uniformDistributedLoads)
        {
            if (uniformDistributedLoad.Origin == origin)
            {
                instance.distributedToRemove.Add(uniformDistributedLoad);
            }
        }
        foreach (UniformDistributedLoad uniformDistributedLoad in instance.distributedToRemove)
        {
            instance.uniformDistributedLoads.Remove(uniformDistributedLoad);
            GameManager.instance.ExportLog("uniform load", "remove", "Removed uniform load from beam " + uniformDistributedLoad.beamId);
        }
        instance.distributedToRemove.Clear();
    }

    private static void UpdateArrow(ref GameObject arrow, int forceType, Node n1, Node n2)              //forceType: 1 tensile, 0 none, -1 compressive
    {
        Vector3 nodeLine = new Vector3(n2.x - n1.x, 0, n2.y - n1.y);
        float rotation = (float)(-System.Math.Atan2(nodeLine.z, nodeLine.x) * 180 / System.Math.PI);
        Vector3 position = new Vector3(nodeToUnityFactor * n1.x, arrowHeight, nodeToUnityFactor * n1.y - 7);  //World object coordinates shift
        if (forceType == -1)
        {
            if (arrow != null && arrow.tag == "Vector Arrow")
            {
                Destroy(arrow);
                arrow = Instantiate(instance.vectorArrow, position , Quaternion.Euler(0f, rotation + 180f, 0f));
            }
            else if (arrow == null)
            {
                arrow = Instantiate(instance.vectorArrow, position, Quaternion.Euler(0f, rotation + 180f, 0f));
            }
        }
        else if (forceType == 1)
        {
            if (arrow != null && arrow.tag == "Vector Arrow")
            {
                Destroy(arrow);
                arrow = Instantiate(instance.vectorArrow, position, Quaternion.Euler(0f, rotation, 0f));
            }
            else if (arrow == null)
            {
                arrow = Instantiate(instance.vectorArrow, position, Quaternion.Euler(0f, rotation, 0f));
            }
        }
        else
        {
            if (arrow != null)
            {
                Destroy(arrow);
            }
        }
    }
    
    public static void UpdateBeamArrows()
    {
        float maxScale = 125;
        float minScale = 20;
        float maximalForce = 0;
        for (int i = 0; i < instance.elementPeakForces.Count; i++)
        {
            maximalForce = Math.Max(maximalForce, (float)Math.Abs(instance.elementPeakForces[i].Nx));
        }
        Vector3 currentScale;
        for (int i = 0; i < instance.elementPeakForces.Count; i++)
        {
            UpdateArrow(ref instance.elementArrows[i].arrow1, Math.Sign(instance.elementPeakForces[i].Nx), instance.elementNodes[i].n1, instance.elementNodes[i].n2);
            UpdateArrow(ref instance.elementArrows[i].arrow2, Math.Sign(instance.elementPeakForces[i].Nx), instance.elementNodes[i].n2, instance.elementNodes[i].n1);
            if (instance.elementArrows[i].arrow1 != null && maximalForce != 0)
            {
                float xScale = Mathf.Max(maxScale * (float)Math.Abs(instance.elementPeakForces[i].Nx) / maximalForce, minScale);
                currentScale = instance.elementArrows[i].arrow1.transform.localScale;
                instance.elementArrows[i].arrow1.transform.localScale = new Vector3(xScale, currentScale.y, currentScale.z);
                currentScale = instance.elementArrows[i].arrow2.transform.localScale;
                instance.elementArrows[i].arrow2.transform.localScale = new Vector3(xScale, currentScale.y, currentScale.z);
            }
        }
    }

    public static void UpdateGraphs()
    {   
        // Set stresses
        List<float> axial = new List<float>();
        List<float> shear = new List<float>();
        List<float> bending = new List<float>();

        foreach(PeakElementInternalForce e in instance.elementPeakForces){
            axial.Add((float)e.Nx);
            shear.Add((float)e.Vy);
            bending.Add((float)e.Mzz);
        }

        // Add point loads
        List<string> loads = new List<string>();
        foreach(var pt in instance.pointLoads){
            loads.Add(pt.Value.getString(true));
        }

        // Add distributed loads
        foreach(var l in instance.uniformDistributedLoads){
            loads.Add(l.getString(true));
        }
        
        // Add logs to graphs
        instance.graphManagers[0].AddGraphLog(axial, shear, bending, loads);
        instance.graphManagers[1].AddGraphLog(axial, shear, bending, loads);

        // instance.graph1Values.Add((float)instance.elementPeakForces[4].Nx);
        // instance.graph2Values.Add((float)instance.elementPeakForces[4].Mzz);

        // if (instance.graph1Values.Count > 1){
        //     instance.graphManagers[0].ShowGraph(instance.graph1Values);
        //     instance.graphManagers[1].ShowGraph(instance.graph2Values);
        // }
    }

    public static void UpdateStress()
    {
        if (instance.axialStress == null)
        {
            instance.axialStress = new List<float>(instance.elements.Count);
            instance.bendingStress = new List<float>(instance.elements.Count);
            instance.shearStress = new List<float>(instance.elements.Count);
        }
        float axial, bending, shear;
        for (int i = 0; i < instance.elements.Count; i++)
        {
            //factor 9.81 for gravity (gravity is not considered when calling frame3dd)
            
            //Axial stress
            axial = (float)(9.81 * instance.elementPeakForces[i].Nx / instance.crossSections[i].Ax / ((instance.elementPeakForces[i].Nx > 0) ? instance.materials[i].Constants.ft0 : instance.materials[i].Constants.fc0));
            //Bending stress
            bending = (float)(9.81 * 6 * instance.elementPeakForces[i].Mzz / instance.crossSections[i].Height / instance.crossSections[i].Width / instance.crossSections[i].Width / instance.materials[i].Constants.fmk);
            //Shear stress
            shear = (float)(9.81 * instance.elementPeakForces[i].Vy / instance.crossSections[i].Asy / instance.materials[i].Constants.fvk);
            if (instance.axialStress.Count <= i)
            {
                instance.axialStress.Add(axial);
                instance.bendingStress.Add(bending);
                instance.shearStress.Add(shear);
            }
            else
            {
                instance.axialStress[i] = axial;
                instance.bendingStress[i] = bending;
                instance.shearStress[i] = shear;
            }
        }
    }

    private void ShowStress(List<float> stress)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            stressTexts[i].Text = Math.Round(100 * Math.Abs(stress[i])).ToString() + "%";
        }
    }

    public void ShowAxialStress()
    {
        ShowStress(axialStress);
    }

    public void ShowBendingStress()
    {
        ShowStress(bendingStress);
    }

    public void ShowShearStress()
    {
        ShowStress(shearStress);
    }

    public void HideStress()
    {
        for (int i = 0; i < elements.Count; i++)
        {
            stressTexts[i].Text = "";
        }
    }

    public void SetSavePath(string path)
    {
        savePath = path;
        UIManager.instance.EnableSaving();
    }

    private IEnumerator CaptureScreenshot(Action<string> callback)
    {
        yield return new WaitForEndOfFrame();
        Texture2D imageTexture = ScreenCapture.CaptureScreenshotAsTexture(1);
        byte[] imageBytes = imageTexture.EncodeToPNG();
        string encodedImage = Convert.ToBase64String(imageBytes);
        callback(encodedImage);
    }

    public void Save()
    {
        StartCoroutine(CaptureScreenshot((string encodedImage) =>
        {
            GameData data = new GameData();
            data.projectVersion = Application.version;
            data.SetMaterials(materials);
            data.SetCrossSections(crossSections);
            data.SetLoads(pointLoads);
            data.SetSnow(SnowManager.instance.Snow, SnowManager.instance.MaxHeight);
            data.SetWind(WindManager.instance.Strength);
            data.image = encodedImage;
            data.comment = "Greetings world!";

            string json = JsonUtility.ToJson(data);
            //UnityEngine.Debug.Log(json);
            //UnityEngine.Debug.Log(json.Length);
            StartCoroutine(WebRequest.PutText(savePath, json, (string s) => { UnityEngine.Debug.Log(s); UnityEngine.Debug.Log("Saved successfully!"); }));
        }));
    }

    public void LoadFrom(string path)
    {
        loadPath = path;
        StartCoroutine(WebRequest.GetText(path, (encodedData) => { UnityEngine.Debug.Log(encodedData); Load(JsonUtility.FromJson<GameData>(encodedData)); }));
    }

    // Exports the UI data into Mysql database
    public void ExportLog(string objectType, string action, string details){
        BackendLog log = new BackendLog();

        log.objectType = objectType;
        log.action = action;
        log.details = details;

        string json = JsonUtility.ToJson(log);

        // instance.gameObject.GetComponent<WebRequest>().Call(instance.mysqlPath, json);
        StartCoroutine(WebRequest.PutText(instance.mysqlPath, json, (string s) => { UnityEngine.Debug.Log(s); UnityEngine.Debug.Log("added to mysql"); }));
    }


    public void SetFrame3DDPath(string path)
    {
        frame3DDPath = path;
        shouldRecalculate = true;   //first calculate statics with exagg=0 for reference state
    }

    public void SetMySQLPath(string path)
    {
        mysqlPath = path;
    }

    private void Load(GameData data)
    {

        for (int i = 0; i < elements.Count; i++)
        {
            elements[i].GetComponent<Material>().Type = data.MaterialTypes[i];
            elements[i].GetComponent<CrossSection>().Width = data.BeamWidths[i];
            elements[i].GetComponent<CrossSection>().Height = data.BeamHeights[i];
        }

        foreach (GameData.Load load in data.Loads)
        {
            GameObject newObject = ObjectManager.instance.Spawn(load.id, load.Position, load.Rotation, load.Type);
            newObject.GetComponent<Attachment>().Attach(elements[load.Target].GetComponent<Collider>());
        }

        SnowManager.instance.Snow = data.Snow;
        SnowManager.instance.MaxHeight = data.SnowHeight;
        WindManager.instance.Strength = data.WindStrength;

        UnityEngine.Debug.Log("Loaded successfully!");
    }

    public void SetDeformations(List<List<Vector3> > vectors)
    {
        if (staticsExagg == 0)
        {
            staticsExagg = 1;
            starters = new Vector3[elements.Count][];
            for (int i = 0; i < starters.Length; i++)
            {
                starters[i] = vectors[i].ToArray();
            }
        }
        DeformationManager.instance.SetDeformations(vectors);
    }
}

[Serializable]
class GameData
{
    public string projectVersion;
    public string comment;
    public string image;

    public List<MaterialType> materialTypes;
    public List<int> beamWidths;
    public List<int> beamHeights;
    public List<Load> loads;

    public bool snow;
    public float snowHeight;

    public float windStrength;

    [Serializable]
    public class Load
    {
        public int target;
        public int id;
        public int type;

        public float posX;
        public float posY;
        public float posZ;

        public float rotX;
        public float rotY;
        public float rotZ;

        public Load(int target, int id, int type, Transform transform)
        {
            this.target = target;
            this.id = id;
            this.type = type;
            this.Position = transform.position;
            this.Rotation = transform.rotation;
        }

        public int Target
        {
            get
            {
                return target;
            }
        }

        public int Type
        {
            get
            {
                return type;
            }
        }

        public Vector3 Position
        {
            get
            {
                return new Vector3(posX, posY, posZ);
            }

            set
            {
                posX = value.x;
                posY = value.y;
                posZ = value.z;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return Quaternion.Euler(rotX, rotY, rotZ);
            }

            set
            {
                rotX = value.eulerAngles.x;
                rotY = value.eulerAngles.y;
                rotZ = value.eulerAngles.z;
            }
        }
    }

    public List<Load> Loads
    {
        get
        {
            return loads;
        }
    }

    public List<MaterialType> MaterialTypes
    {
        get
        {
            return materialTypes;
        }
    }

    public List<int> BeamWidths
    {
        get
        {
            return beamWidths;
        }
    }

    public List<int> BeamHeights
    {
        get
        {
            return beamHeights;
        }
    }

    public bool Snow
    {
        get
        {
            return snow;
        }
    }

    public float SnowHeight
    {
        get
        {
            return snowHeight;
        }
    }

    public float WindStrength
    {
        get
        {
            return windStrength;
        }
    }

    public void SetMaterials(List<Material> materials)
    {
        materialTypes = new List<MaterialType>();
        for (int i = 0; i < materials.Count; i++)
        {
            materialTypes.Add(materials[i].Type);
        }
    }

    public void SetCrossSections(List<CrossSection> crossSections)
    {
        beamWidths = new List<int>();
        beamHeights = new List<int>();
        for (int i = 0; i < crossSections.Count; i++)
        {
            beamWidths.Add(crossSections[i].Width);
            beamHeights.Add(crossSections[i].Height);
        }
    }

    public void SetLoads(Dictionary<int, PointLoad> pointLoads)
    {
        loads = new List<Load>();
        foreach (KeyValuePair<int, PointLoad> pointLoad in pointLoads)
        {
            Load newLoad = new Load(pointLoad.Value.target, pointLoad.Key, pointLoad.Value.Type, pointLoad.Value.transform);
            loads.Add(newLoad);
        }
    }

    public void SetSnow(bool snow, float maxHeight)
    {
        this.snow = snow;
        this.snowHeight = maxHeight;
    }

    public void SetWind(float strength)
    {
        windStrength = strength;
    }
}

[Serializable]
class BackendLog{
    public int objectId;
    public string objectType;
    public string action;
    public string details;
}