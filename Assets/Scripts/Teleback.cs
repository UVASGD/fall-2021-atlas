using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleback : MonoBehaviour
{
    Movement movement;
    public bool collecting = true;
    public int pathSizeFrames = 150;
    Pose[] prevPath;
    int top = 0;
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        prevPath = new Pose[pathSizeFrames];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            collecting = false;
                
        }
        if (collecting)
        {
            prevPath[top] = new Pose(this.transform.position);
            top = (top + 1) % pathSizeFrames;
        }
    }
}
class Pose
{
    Vector3 position;
    public Pose(Vector3 position)
    {
        this.position = position;
    }
}
