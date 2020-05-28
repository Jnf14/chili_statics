using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    AR,
    Web
}

public class GameModeManager : MonoBehaviour
{

    public static GameModeManager instance;

    public Camera ArCamera;
    public Camera WebCamera;
    public GameObject ArPlane;
    public GameObject WebPlane;
    [SerializeField]
    private GameMode gameMode;

    public GameMode GameMode
    {
        get
        {
            return gameMode;
        }
        private set
        {
            gameMode = value;
        }
    }

    public GameMode SwitchMode()
    {
        if (GameMode == GameMode.AR)
        {
            GameMode = GameMode.Web;
            ArCamera.enabled = false;
            WebCamera.enabled = true;
            ArPlane.SetActive(false);
            WebPlane.SetActive(true);
            ObjectManager.instance.ActivateWebMode();
        }
        else
        {
            GameMode = GameMode.AR;
            ArCamera.enabled = true;
            WebCamera.enabled = false;
            ArPlane.SetActive(true);
            WebPlane.SetActive(false);
            ObjectManager.instance.ActivateArMode();
        }
        UIManager.instance.Refresh();
        return GameMode;
    }

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
