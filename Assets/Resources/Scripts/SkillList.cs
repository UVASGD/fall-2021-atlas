using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillList : MonoBehaviour
{

    public List<string> skillList = new List<string>();

    public int size = 5;

    int index = 0;

    bool isPaused = false;
   


    void Start()
    {
        skillList.Add("bomb");
        skillList.Add("knife");
        skillList.Add("gun");
        skillList.Add("sword");
        skillList.Add("shoe");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.LeftControl))
        {
            pause();
            isPaused = true;
        }
        if(Input.GetKey(KeyCode.LeftControl))
        { 
            if (Input.GetKeyDown(KeyCode.W) && index < size - 1)
            {
                index++;
                Debug.Log(skillList[index]);
            }

            if (Input.GetKeyDown(KeyCode.S) && index > 0)
            {
                index--;
                Debug.Log(skillList[index]);
            }
        }
 
        

    }

    void pause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            Movement.canMove = false;
            
        }

        if (isPaused)
        {
            Time.timeScale = 1f;
            Movement.canMove = true;
            isPaused = false;
        }
        
    }
}
