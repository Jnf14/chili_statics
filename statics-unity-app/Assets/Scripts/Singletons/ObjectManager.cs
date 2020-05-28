using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;

    [SerializeField]
    private GameObject tonweight;
    [SerializeField]
    private GameObject solarPanel;
    [SerializeField]
    private GameObject waterTank;
    [SerializeField]
    private GameObject jacuzzi;
    private Dictionary<int, GameObject> gameObjects = new Dictionary<int, GameObject>();
    private List<string> toAdd = new List<string>();
    private List<string> toMove = new List<string>();
    private List<string> toRemove = new List<string>();

    private Vector3 defaultLocation = new Vector3(9, 0.4f, 6);

    private HashSet<int> idPool = new HashSet<int>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        resetIdPool();
    }

    public void WebDeleteDone(int id)
    {
        gameObjects.Remove(id);
        idPool.Add(id);
    }

    private void resetIdPool()
    {
        idPool.Clear();
        for (int i = 60; i <= 75; i++)
        {
            idPool.Add(i);
        }
    }

    public void ObjectEvent(string dataString)
    {
        if (dataString[0] == 'A')       //Add,...
        {
            Add(dataString.Substring(4));
        }
        else if (dataString[0] == 'M')  //Move,...
        {
            Move(dataString.Substring(5));
        }
        else if (dataString[0] == 'R') //Remove,...
        {
            Remove(dataString.Substring(7));
        }
    }

    void Add(string dataString)
    {
        toAdd.Add(dataString);
    }

    void Move(string dataString)
    {
        toMove.Add(dataString);
    }

    void Remove(string dataString)
    {
        toRemove.Add(dataString);
    }

    void DelayedAdd(string dataString)
    {
        string[] values = dataString.Split(',');
        int id = int.Parse(values[0]);
        float[] coordinates = { float.Parse(values[1]), float.Parse(values[2]) };
        float rotation = float.Parse(values[3]);

        GameObject gameObject = Spawn(id, new Vector3(coordinates[0], 0.4f, coordinates[1]), Quaternion.Euler(90f, rotation, 0f), 0);
        gameObject.GetComponent<Dragging>().enabled = false;
        gameObject.GetComponent<WebOnlyRemoval>().enabled = false;

        gameObjects.Add(id, gameObject);
        idPool.Remove(id);
    }

    void DelayedMove(string dataString)
    {
        string[] values = dataString.Split(',');
        int id = int.Parse(values[0]);
        float[] coordinates = { float.Parse(values[1]), float.Parse(values[2]) };
        float rotation = float.Parse(values[3]);

        if (gameObjects.ContainsKey(id))
        {
            gameObjects[id].GetComponent<Movement>().Move(coordinates, rotation);
        }
        else
        {
            DelayedAdd(dataString);
        }
    }

    void DelayedRemove(string dataString)
    {
        string[] values = dataString.Split(',');
        int id = int.Parse(values[0]);
        //float[] coordinates = { float.Parse(values[1]), float.Parse(values[2]) };
        //float rotation = float.Parse(values[3]);

        if (gameObjects.ContainsKey(id))
        {
            gameObjects[id].GetComponent<Removal>().Remove();
            gameObjects.Remove(id);
            idPool.Add(id);
        }

    }

    private void Update()
    {
        if (GameModeManager.instance.GameMode == GameMode.Web)
        {
            toAdd.Clear();
            toMove.Clear();
            toRemove.Clear();
        }
        else
        {
            foreach (string dataString in toAdd)
            {
                DelayedAdd(dataString);
            }
            toAdd.Clear();
            foreach (string dataString in toMove)
            {
                DelayedMove(dataString);
            }
            toMove.Clear();
            foreach (string dataString in toRemove)
            {
                DelayedRemove(dataString);
            }
            toRemove.Clear();
        }
    }

    public void ActivateWebMode()
    {
        foreach (GameObject gameObject in gameObjects.Values)
        {
            gameObject.GetComponent<Dragging>().enabled = true;
            gameObject.GetComponent<WebOnlyRemoval>().enabled = true;
        }
    }

    public void ActivateArMode()
    {
        foreach (GameObject gameObject in gameObjects.Values)
        {
            gameObject.GetComponent<Removal>().Remove();
        }
        gameObjects.Clear();
        resetIdPool();
    }

    public GameObject Spawn(int objectId, Vector3 position, Quaternion rotation, int objectType)
    {
        GameObject original;
        switch (objectType)
        {
            case 0:
                original = tonweight;
                break;
            case 1:
                original = solarPanel;
                break;
            case 2:
                original = waterTank;
                break;
            case 3:
                original = jacuzzi;
                break;
            default:
                original = tonweight;
                break;
        }
        GameObject newObject = Instantiate(original, position, rotation);
        newObject.name = "Object " + objectId;
        return newObject;
    }

    public GameObject Spawn(int objectId, Vector3 position, int objectType)
    {
        return Spawn(objectId, position, Quaternion.Euler(90, 0, 0), objectType);
    }

    public GameObject Spawn(int objectId, int objectType)
    {
        return Spawn(objectId, defaultLocation, objectType);
    }

    public void WebSpawn(int objectType)
    {
        if (idPool.Count > 0)
        {
            int objectId = 0;
            foreach (int id in idPool)
            {
                objectId = id;
                break;
            }
            GameObject gameObject = Spawn(objectId, objectType);
            gameObjects.Add(objectId, gameObject);
            idPool.Remove(objectId);

            string text;
            if(objectType == 0) text = "weight";
            else if(objectType == 1) text = "solar panel";
            else if(objectType == 2) text = "water tank";
            else text = "jacuzzi";

            GameManager.instance.ExportLog("point load", "add", "Added " + text + " to scene");
        }
    }
}
