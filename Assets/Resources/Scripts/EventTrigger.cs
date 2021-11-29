using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public string activationType;
    public GameObject activator;
    public bool canActivate;
    public float variation;
    public float timeToAct = 60;
    private bool activated;
    private Transform obj;
    private int time;

    void Start()
    {
        if (activator != null)
            obj = activator.GetComponent<Transform>();
    }

    void Update()
    {
        if(activated && time > 0 && activationType == "Panel")
        {
            obj.position = new Vector3(obj.position.x, obj.position.y + variation / timeToAct);
            time--;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (canActivate && other.tag == "Player")
        {
            Activate();
            canActivate = false;
        }
    }

    void Activate()
    {
        switch (activationType)
        {
            case "Speech":
                activator.SendMessage("readDialogue", (int)variation);
                break;
            case "Panel":
                activated = true;
                AudioManager.PlaySound("door");
                time = 60;
                break;
            case "Trigger":
                if(variation != 0)
                    activator.GetComponent<EventTrigger>().canActivate = true;
                else
                    activator.GetComponent<EventTrigger>().canActivate = false;
                break;
        }
    }

}
