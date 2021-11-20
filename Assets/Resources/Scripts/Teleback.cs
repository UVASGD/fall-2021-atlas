using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleback : MonoBehaviour
{
    //public Camera camera;
    public Transform[] path;
    public GameObject pathIndicator;
    public int pathSizeFrames = 150;
    public int travelBackSpeedFrames = 5;
    Pose[] prevPath;
    Sprite[] remImage;
    bool[] rot;
    int[] ats;
    Sprite ret;
    SpriteRenderer[] pathsp;
    SpriteRenderer sp;
    bool revInit = false;
    int top = 0;
    int oldTop = -1;
    int backCharge = -1;
    int curStackSize;
    bool backCharged = false;
    //Vector2 camSize;
    // Start is called before the first frame update

    void Start()
    {
        //camSize = camera.v;
        prevPath = new Pose[pathSizeFrames];
        remImage = new Sprite[pathSizeFrames];
        rot = new bool[pathSizeFrames];
        ats = new int[path.Length];
        sp = GetComponent<SpriteRenderer>();
        pathsp = new SpriteRenderer[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            pathsp[i] = path[i].GetComponent<SpriteRenderer>();
        }
        ret = pathsp[0].sprite;
    }

    // Update is called once per frame
    void Update()
    {
        //camera.sensorSize = camSize;
        if (!backCharged)
        {

            if (Input.GetKey(KeyCode.E))
            {
                backCharge++;
            }
            else
            {

                oldTop = -1;
                if (prevPath[top] != null)
                {
                    Destroy(prevPath[top].indicator);
                    curStackSize--;
                }
                prevPath[top] = new Pose(transform.position);
                curStackSize += 1;

                prevPath[top].indicator = Instantiate(pathIndicator, transform.position, Quaternion.identity);
                remImage[top] = sp.sprite;
                rot[top] = sp.flipX;

                revInit = false;
                for (int i = 0; i < path.Length; i++)
                {
                    int at = (top + 1 + i * 30) % pathSizeFrames;
                    if (prevPath[at] != null)
                    {
                        Vector3 pos = prevPath[at].position;
                        path[i].position = new Vector3(pos.x, pos.y, pos.z + 1);
                        if (pathsp[i].sprite.name != ret.name)
                            pathsp[i].sprite = ret;
                    }
                }

                top = (top + 1) % pathSizeFrames;

            }
            backCharged = Input.GetKeyUp(KeyCode.E) || backCharge >= curStackSize - 1;
        }
        else 
        {
            
            //camera.sensorSize = 0.49F*camSize;
            if (!revInit)
            {
                for (int i = 0; i < path.Length; i++)
                    ats[i] = (top + 1 + i * 30) % pathSizeFrames;
            }
            revInit = true;
            if (oldTop == -1)
            {
                oldTop = top;
                top = (top - 1 + pathSizeFrames) % pathSizeFrames;

            }
            if (prevPath[top] != null)
            {
                this.transform.position = prevPath[top].position;
                for (int i = 0; i < travelBackSpeedFrames && top != oldTop; i++)
                {
                    if (prevPath[top] != null && backCharge > 0)
                    {
                        Destroy(prevPath[top].indicator);
                        prevPath[top] = null;
                        top = (top - 1 + pathSizeFrames) % pathSizeFrames;
                        curStackSize--;
                        backCharge--;
                    }
                }

                for (int i = 0; i < path.Length; i++)
                {
                    if (prevPath[ats[i]] == null && pathsp[i].sprite.name == ret.name)
                    {
                        pathsp[i].sprite = remImage[ats[i]];
                        pathsp[i].flipX = rot[ats[i]];
                    }
                }
                if (backCharge == 0)
                {
                    backCharge = -1;
                    backCharged = false;
                }
            }
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
