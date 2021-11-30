using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject activator;
    public float yGoal;

    public float upReturnVelocity = 10;
    public bool activated;
    private Transform trans;
    private Rigidbody2D rb2d;
    public float yStart;
    public int timer;
    private bool playerOn = false;
    public int timeToUp = 50;
    bool canClick = true;
    void Start()
    {

        trans = GetComponent<Transform>();
        rb2d = GetComponent<Rigidbody2D>();
        yStart = transform.position.y;
    }

    void Update()
    {

        if (trans.position.y >= yStart)
        {
            if (activated)
            {
                AudioManager.StopSound("ButtonTick");
                AudioManager.PlaySound("ButtonUp");
            }
            activated = false;
            rb2d.velocity = Vector2.zero;
            
        }
        else if (trans.position.y <= yGoal && playerOn) {
            activated = true;
            if (canClick)
            {
                AudioManager.PlaySound("ButtonDown");
                canClick = false;
            }

            timer = timeToUp;
            
        }
        if (timer > 0)
        {
            if (!AudioManager.IsPlaying("ButtonTick"))
            {

                print("playing button tick 1");
                AudioManager.PlaySound("ButtonTick");
            }

            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            timer--;
        }
        else if (trans.position.y < yStart)
        {
            canClick = true;
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
            rb2d.AddForce(Vector2.up * upReturnVelocity);
        } 


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            playerOn = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            playerOn = false;
    }
}
