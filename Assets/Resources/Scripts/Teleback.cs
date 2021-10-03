using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleback : MonoBehaviour
{
    //public Camera camera;
    public GameObject pathIndicator;
    public int pathSizeFrames = 150;
    public int travelBackSpeedFrames = 5;
    Pose[] prevPath;
    int top = 0;
    int oldTop = -1;
    //Vector2 camSize;
    // Start is called before the first frame update
    void Start()
    {
        //camSize = camera.v;
        prevPath = new Pose[pathSizeFrames];
    }

    // Update is called once per frame
    void Update()
    {
        //camera.sensorSize = camSize;
        
        if (Input.GetKey(KeyCode.E))
        {
            //camera.sensorSize = 0.49F*camSize;
            if (oldTop == -1)
            {
                oldTop = top;
                top = (top - 1 + pathSizeFrames) % pathSizeFrames;

            }
            if (prevPath[top]!=null)
            {
                this.transform.position = prevPath[top].position;
                for (int i = 0; i < travelBackSpeedFrames && top != oldTop; i++)
                {
                    if (prevPath[top] != null)
                    {
                        Destroy(prevPath[top].indicator);
                        prevPath[top] = null;
                        top = (top - 1 + pathSizeFrames) % pathSizeFrames;
                    }
                }
            }
        } else
        {
            oldTop = -1;
            if (prevPath[top] != null)
            {
                Destroy(prevPath[top].indicator);
            }
            prevPath[top] = new Pose(transform.position);
            prevPath[top].indicator = Instantiate(pathIndicator, transform.position, Quaternion.identity);

            top = (top + 1) % pathSizeFrames;
        }
    }
}
class Pose
{
    public GameObject indicator;
    public Vector3 position;
    public Pose(Vector3 position)
    {
        this.position = position;
    }
    
}
