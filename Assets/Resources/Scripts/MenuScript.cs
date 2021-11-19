using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;


public class MenuScript : MonoBehaviour
{

    public GameObject mainPrefab;
    public GameObject optionsPrefab;
    public GameObject pausePrefab;
    public static GameObject currentMenu;

    public Dictionary<string, MenuAction> actions;


    public List<string> menuOptions;
    public GameObject dummyString;
    public GameObject cursor;

    private SpriteRenderer sp;
    private Transform tf;
    private List<Renderer> listConts;
    private Vector3 location;
    private float height;
    private float width;
    private int size;
    private int pos;
    private bool hidden = true;
    private bool growingH;
    private bool growingW;
    private bool growingTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        currentMenu = this.gameObject;

        actions = new Dictionary<string, MenuAction>() {
        { "Options", new MenuAction(ActionType.OPEN_MENU, optionsPrefab) },
        { "Pause", new MenuAction(ActionType.OPEN_MENU, pausePrefab) },
        { "Back", new MenuAction(ActionType.OPEN_MENU, mainPrefab) },

        {"Main Menu", new MenuAction(ActionType.LOAD_MENU_SCENE, "Title Screen")},
        {"Credits", new MenuAction(ActionType.LOAD_MENU_SCENE, "Credits") },
        {"Start", new MenuAction(ActionType.LOAD_LEVEL_SCENE, -1) },



        {"Quit", new MenuAction(ActionType.MISC, (Action)( () =>  Application.Quit() )) },
        { "Resume", new MenuAction(ActionType.MISC, (Action)(() => { Time.timeScale = 1; Destroy(currentMenu); }))}

    };
        // Initiate listConts List
        listConts = new List<Renderer>();

        // Gets info on the menu components
        sp = GetComponent<SpriteRenderer>();
        tf = transform;
        location = new Vector3(tf.position.x, tf.position.y, tf.position.z);
        height = sp.size.y;
        width = sp.size.x;
        tf.position = new Vector3(location.x - (width - 0.5F)/2, location.y + (height -0.5F)/2, location.z);
        sp.size = new Vector2(0, 0);


        // Sets up cursor position
        cursor.transform.position = new Vector3(tf.position.x - (width * 0.35F), tf.position.y + height * 0.3F, 0);
        listConts.Add(cursor.GetComponent<Renderer>());
        listConts[0].material.color = new Color(255, 255, 255, 0);

        // Sets up options of menu and spacing
        size = menuOptions.Count;
        for (int i = 0; i < size; i++)
        {
            GameObject str = Instantiate(dummyString, new Vector3(tf.position.x - (width * 0.3F), tf.position.y + (height * 0.3F) - (height * 0.6F) / (size - 1) * i, 0), Quaternion.identity);
            str.transform.parent = tf;
            Renderer ren = str.GetComponent<MeshRenderer>();
            ren.sortingLayerName = "UI";
            ren.sortingOrder = 2;
            TextMesh txt = str.GetComponent<TextMesh>();
            txt.text = menuOptions[i];
            listConts.Add(ren);
            listConts[i+1].material.color = new Color(255, 255, 255, 0);
        }
    }
    public void StartGrowing()
    {
        growingTriggered = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (hidden && (growingTriggered || Input.anyKey) && !growingW && !growingH)
        {
            growingH = true;
            sp.size = new Vector2(0.5F, 0.5F);
        }

        if(growingH && sp.size.y < height)
        {
            sp.size = new Vector2(0.5F, (height - 0.5F) / 15 + sp.size.y);
            tf.position = new Vector3(tf.position.x, location.y + (height - 0.5F) / 2 - (sp.size.y - 0.5F) / 2, location.z);
            if (sp.size.y >= height)
            {
                sp.size = new Vector2(0.5F, height);
                tf.position = new Vector3(tf.position.x, location.y, location.z);
                growingW = true;
            }
        }

        if (growingW && sp.size.x < width)
        {
            sp.size = new Vector2((width + 0.5F) / 15 + sp.size.x, height);
            tf.position = new Vector3(location.x - (width - 0.5F) / 2 + (sp.size.x - 0.5F) / 2, location.y, location.z);
            if (sp.size.x >= width)
            {
                sp.size = new Vector2(width, height);
                tf.position = new Vector3(location.x, location.y, location.z);
                foreach(Renderer ren in listConts)
                {
                    ren.material.color = new Color(255, 255, 255, 1);
                }
                hidden = false;
            }
        }

        if (!hidden)
        {
            // Updates cursor positions based off of input
            if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
            {
                if (pos >= size - 1)
                    pos = -1;
                pos++;
                cursor.transform.position = new Vector3(tf.position.x - (width * 0.35F), tf.position.y + height * 0.3F - (height * 0.6F) / (size - 1) * pos, 0);
            }
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
            {
                if (pos <= 0)
                    pos = size;
                pos--;
                cursor.transform.position = new Vector3(tf.position.x - (width * 0.35F), tf.position.y + height * 0.3F - (height * 0.6F) / (size - 1) * pos, 0);
            }

            // Activates based off of entry value
            if (Input.GetKeyDown(KeyCode.Return))
            {
                takeAction(menuOptions[pos]);
            }
        }
    }

    public void takeAction(String actionName)
    {
        MenuAction actionToTake = actions[actionName];
        switch (actionToTake.type)
        {
            case ActionType.LOAD_LEVEL_SCENE:
                int levelNum = (int)actionToTake.storedInfo;
                if (levelNum == -1)
                {
                    SceneController.ReturnToCurLevel();
                }
                else
                {
                    SceneController.LoadLevel(levelNum);
                }
                break;
            case ActionType.LOAD_MENU_SCENE:
                string menuName = (string)actionToTake.storedInfo;
                SceneController.DivertToNonLevel(menuName);
                break;
            case ActionType.OPEN_MENU:
                GameObject newMenu = (GameObject)actionToTake.storedInfo;
                if (currentMenu != null)
                {
                    Destroy(currentMenu);
                }
                currentMenu = Instantiate(newMenu);
                currentMenu.SendMessage("StartGrowing");
                break;

        }
    }
}

[ExecuteInEditMode]
public class MenuAction {
    public ActionType type;
    public object storedInfo;
    public MenuAction(ActionType type, object loadGoal)
    {
        this.type = type;
        this.storedInfo = loadGoal;
    }
}
public enum ActionType
{
    LOAD_LEVEL_SCENE, LOAD_MENU_SCENE, OPEN_MENU, MISC
}
