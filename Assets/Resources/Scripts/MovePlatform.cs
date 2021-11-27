using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class MovePlatform : MonoBehaviour
{
    public bool moveToTarget1; //moving towards target 1?
    public bool moveToTarget2; //moving towards target 2?
    public Transform target; // the target position
    public Transform target2; // the second target position
    public float speed; // speed - units per second (gives you control of how fast the object will move in the inspector)
    public bool moveObj; // a public bool that allows you to toggle this script on and off in the inspector
   
    // Update is called once per frame
    void Update () {
        if(moveToTarget1 == true)
        {
            float step = speed * Time.deltaTime; // step size = speed * frame time
            transform.position = Vector3.MoveTowards(transform.position, target.position, step); // moves position a step closer to the target position
        }
        else if(moveToTarget2 == true) {
            float step = speed * Time.deltaTime; // step size = speed * frame time
            transform.position = Vector3.MoveTowards(transform.position, target2.position, step); // moves position a step closer to the target position
        }
        if(transform.position==target.position){
            moveToTarget1=false;
            moveToTarget2=true;
        } 
        else if(transform.position==target2.position){
            moveToTarget2=false;
            moveToTarget1=true;
        }      
    }
}