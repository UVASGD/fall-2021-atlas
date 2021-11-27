using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Teleback : MonoBehaviour
{
    //public Camera camera;
    public Transform[] path;
    public GameObject pathIndicator;
    public int pathSizeFrames = 150;
    public int travelBackSpeedFrames = 5;
    public int backChargeDelta = 2;
    Pose[] prevPath;
    bool[] rot;
    int[] ats;
    Sprite ret;
    SpriteRenderer sp;
    bool revInit = false;
    int top = 0;
    int oldTop = -1;
    int backCharge = 0;
    int curStackSize;
    bool backCharged = false;
    Rigidbody2D rb2d;
    //Vector2 camSize;
    // Start is called before the first frame update

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        //camSize = camera.v;
        prevPath = new Pose[pathSizeFrames];
        rot = new bool[pathSizeFrames];
        ats = new int[path.Length];
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //print("StackSize " + curStackSize);
        if (!backCharged)
        {
            if (Input.GetKey(KeyCode.E))
            {
                rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                backCharged = Input.GetKeyUp(KeyCode.E) || backCharge >= curStackSize;
                for (int i = 0; i < backChargeDelta && !backCharged; i++)
                {
                    backCharge++;
                    try
                    {
                        prevPath[((top - backCharge) + pathSizeFrames) % pathSizeFrames].indicator.SetActive(true);
                    } catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                        print(prevPath[((top - backCharge) + pathSizeFrames) % pathSizeFrames]);
                        print("CHARGE " + backCharge + " TOP " + top + " SIZE "+ curStackSize);
                        
                    }
                    backCharged = Input.GetKeyUp(KeyCode.E) || backCharge >= curStackSize;

                }

            }
            else
            {

                oldTop = -1;
                if (prevPath[top] != null)//Destroy top of stack
                {
                    Destroy(prevPath[top].indicator);
                    curStackSize--;
                }
                prevPath[top] = new Pose(transform.position);
                curStackSize += 1;

                prevPath[top].indicator = Instantiate(pathIndicator, transform.position, Quaternion.identity);//add to top of stack
                SpriteRenderer sp2 = prevPath[top].indicator.GetComponent<SpriteRenderer>();
                sp2.sprite = sp.sprite;
                sp2.flipX = sp.flipX;
                sp2.color = new Color(1,1,1,.3F);
                

                

                top = (top + 1) % pathSizeFrames;//increment top

            }
            backCharged = Input.GetKeyUp(KeyCode.E) || backCharge >= curStackSize;
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
                        //sp.sprite = prevPath[top].GetComponent<SpriteRenderer>().;

                        prevPath[top] = null;
                        top = (top - 1 + pathSizeFrames) % pathSizeFrames;
                        curStackSize--;
                        backCharge--;
                    }
                }

                
                if (backCharge <= 0)
                {
                    rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

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
