using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillList : MonoBehaviour
{

    public List<string> skillList = new List<string>();

    public int size = 5;

    public Transform playerTransform;

    int index = 0;

    bool isPaused = false;

    bool timeBombActive = false;

    public GameObject timeBombPrefab;


    void Start()
    {
        skillList.Add("bomb");
        skillList.Add("knife");
        skillList.Add("gun");
        skillList.Add("sword");
        skillList.Add("shoe");

        setSkill();

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.LeftControl))
        {
            pause();
        }
        if(Input.GetKey(KeyCode.LeftControl))
        { 
            if (Input.GetKeyDown(KeyCode.W) && index < size - 1)
            {
                index++;
                setSkill();
               
                Debug.Log(skillList[index]);
            }

            if (Input.GetKeyDown(KeyCode.S) && index > 0)
            {
                index--;
                setSkill();
               
                Debug.Log(skillList[index]);
            }
        }

        if (timeBombActive && !isPaused)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Instantiate(timeBombPrefab, playerTransform.position, playerTransform.rotation);
                Debug.Log("bomb set");
            }
        }

    }

    void pause()
    {
        if (!isPaused)
        {
            isPaused = true;
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

    void setSkill()
    {
        switch(index)
        {
            case 0: timeBombActive = true;
                    break;
            case 1: timeBombActive = false;
                     break;

        }
    }

    
}