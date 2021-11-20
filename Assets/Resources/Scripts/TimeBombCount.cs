using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeBombCount : MonoBehaviour
{ 

    public Collider2D col;

    

    private int timer = 60;

    void Start()
    {
        col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        timer--;
        if (timer == 0 && col.enabled)
        {
            Debug.Log("kaboom");
            Destroy(gameObject);
        }

        if (timer == 0)
        {
            col.enabled = true;
            timer = 5;
        }

       
    }

   
}
