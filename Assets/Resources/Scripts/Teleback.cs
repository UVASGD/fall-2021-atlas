using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleback : MonoBehaviour
{
    public GameObject pathIndicator;
    public int pathSizeFrames = 150;
    Pose[] prevPath;
    int top = 0;
    int oldTop = -1;
    // Start is called before the first frame update
    void Start()
    {
        prevPath = new Pose[pathSizeFrames];
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.E))
        {
            if (oldTop == -1)
            {
                oldTop = top;
                top = (top - 1 + pathSizeFrames) % pathSizeFrames;

            }
            if (top != oldTop && prevPath[top]!=null)
            {
                Destroy(prevPath[top].indicator);
                this.transform.position = prevPath[top].position;
                prevPath[top] = null;
                top = (top - 1 + pathSizeFrames) % pathSizeFrames;
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
