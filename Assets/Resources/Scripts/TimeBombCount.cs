using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeBombCount : MonoBehaviour
{ 

    public Collider2D col;


    public float hitBackSpeed;
    public int timer = 60;

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
            Destroy(gameObject);
        }

        if (timer == 0)
        {
            col.enabled = true;
            timer = 5;
        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player Attack")
        {
            Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
            Vector3 curVel = rb2d.velocity;
            curVel.Normalize();
            curVel *= -hitBackSpeed;
            rb2d.velocity = curVel;
            print("collided!!!");
        }   
    }

}
