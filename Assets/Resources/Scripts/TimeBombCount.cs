using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//count down based on frames
//for 1 frame enable collider
//destroy object

public class TimeBombCount : MonoBehaviour
{ 

    public Collider2D col;

    private float timer = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer == 1.0f)
        {
            col.enabled = false;
        }
    }
}
