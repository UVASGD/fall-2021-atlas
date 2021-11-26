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
    int backCharge = -1;
    int curStackSize;
    bool backCharged = false;
    //Vector2 camSize;
    // Start is called before the first frame update

    void Start()
    {
        //camSize = camera.v;
        prevPath = new Pose[pathSizeFrames];
        rot = new bool[pathSizeFrames];
        ats = new int[path.Length];
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //camera.sensorSize = camSize;
        if (!backCharged)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            if (Input.GetKey(KeyCode.E))
            {
                for (int i = 0; i < backChargeDelta && !backCharged; i++)
                {
                    GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                    backCharge++;
                    try
                    {
                        prevPath[((top - backCharge) + pathSizeFrames) % pathSizeFrames].indicator.SetActive(true);
                    } catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }

                    backCharged = Input.GetKeyUp(KeyCode.E) || backCharge >= curStackSize - 2;

                }

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
                SpriteRenderer sp2 = prevPath[top].indicator.GetComponent<SpriteRenderer>();
                sp2.sprite = sp.sprite;
                sp2.flipX = sp.flipX;
                

                

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
                        //sp.sprite = prevPath[top].GetComponent<SpriteRenderer>().;

                        prevPath[top] = null;
                        top = (top - 1 + pathSizeFrames) % pathSizeFrames;
                        curStackSize--;
                        backCharge--;
                    }
                }

                
                if (backCharge <= 0)
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
