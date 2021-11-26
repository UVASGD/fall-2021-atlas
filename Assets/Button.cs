using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject activator;
    public bool canActivate;
    public float yGoal;

    public int timeToDrop = 10;
    public int timeToDeactivate = 50;
    public bool activated;
    private Transform transform;
    private float variation;
    private int time;

    void Start()
    {
        transform = GetComponent<Transform>();

        variation = yGoal - transform.position.y;
    }

    void Update()
    {
        if (time > 0)
        {
            if (activated)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + variation / (float)timeToDrop);
            } else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - variation / (float)timeToDeactivate);

            }
            time--;
        } else if (time == 0)
        {
            activated = false;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (canActivate && coll.collider.tag == "Player")
        {
            print("button on");
            Activate();
        }
    }
    void OnCollisionExit2D(Collision2D coll)
    {
        print("EXIT!!");
        if (canActivate && coll.collider.tag == "Player")
        {
            print("button off");
            Deactivate();
        }
    }
    void Activate()
    {
        activated = true;
        time = timeToDrop;
    }
    void Deactivate()
    {
        activated = false;
        time = timeToDeactivate;
    }

}
