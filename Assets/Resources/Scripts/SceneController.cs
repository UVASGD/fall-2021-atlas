using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController: MonoBehaviour
{
    static int curLevelNum = 1;
    static string sceneDiversion = "Menu"; //used when you leave a level scene for a non-level scene and would like to return when you are done.
    //Can be used for stuff like menus, secret rooms, etc, anything that doesn't fit into the "Level1, Level2, ..." paradigm
    const bool debug = true;
    static Dictionary<string, string> scenes = new Dictionary<string, string>
    {
        {"Menu", "Title Screen" },
        {"Level1", "Grant Testing"} ,
        {"Level2", "Jimmy Testing" } ,
        {"Level3", "Ian Testing" }
    };

    public void Start()
    {
        if (debug)//Note: this code is written so you don't have to start every line of execution from the menu scene. 
                    // This is a very fast operation but it is not strictly necessary. If we assume we start in the menu scene,
                   // then the starting condition (curLevelNum = 1, sceneDiversion = "Menu") makes sense
                   // If we assume we always load the next level using LoadLevel(), then there is no need to update it here. However,
                   // there is probably no reason to not have it, unless it is giving you trouble, in which case
                   // TO DISABLE **SET DEBUG == FALSE**
        {

            string curName = SceneManager.GetActiveScene().name;
            bool flag = true;
            foreach (string key in scenes.Keys)
            {
                if (scenes[key] == curName)
                {
                    if (key.ToLower().Contains("level"))
                    {
                        curLevelNum = int.Parse(key.Substring(5)); //set the value of curLevelNum
                        flag = false;
                        break;
                    } else
                    {
                        sceneDiversion = key;
                    }
                }
            }
            if (flag)
            {
                Debug.LogError("It appears your current scene is not logged as a level in the SceneController class. " +
                    "To add it, simply put a string in the dict at the top of the class with a string \"LevelX\" as your key" +
                    "(where X is the number of your level; you can also use a custom scene name if you follow the rules from the class comments)" +
                    " and the name of your scene as a value. You can also set SceneController.debug to " +
                    "false to disable this error message, but SceneController may not work properly (see comments for more).");
            }
        }
        
    }
    
    public static void LoadLevel(int levelNum)
    {
        sceneDiversion = null;
        curLevelNum = levelNum;
        SceneManager.LoadScene(scenes["Level"+ levelNum]);
    }
    public static void LoadNextLevel()
    {
        LoadLevel(curLevelNum + 1);
    }
    public static void DivertToNonLevel(string sceneKey)
    {
        sceneDiversion = sceneKey;
        SceneManager.LoadScene(sceneKey);
    }
    public static void ReturnToCurLevel()
    {
        LoadLevel(curLevelNum);
    }

    //non-static methods for use in buttons
    public void loadLevel(int levelNum)
    {
        LoadLevel(levelNum);
    }
    public void loadNextLevel()
    {
        LoadNextLevel();
    }
    public void divertToNonLevel(string sceneKey)
    {
        DivertToNonLevel(sceneKey);
    }
    public void returnToCurLevel()
    {
        ReturnToCurLevel();
    }
}
