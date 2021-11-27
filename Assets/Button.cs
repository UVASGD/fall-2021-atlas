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
    private int timer;
    private bool playerOn = false;
    public int timeToUp = 50;

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
            activated = false;
            rb2d.velocity = Vector2.zero;
        }
        else if (trans.position.y <= yGoal && playerOn) {
            activated = true;

            timer = timeToUp;
            
        }
        if (timer > 0)
        {
            rb2d.velocity = Vector2.zero;
            timer--;
        }
        else if (trans.position.y < yStart)
        {
            print("Awefawef");
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
