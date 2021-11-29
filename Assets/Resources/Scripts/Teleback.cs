using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Teleback : MonoBehaviour
{
    //public Camera camera;
    public GameObject pathIndicator;
    public int pathSizeFrames = 150;
    public int travelBackSpeedFrames = 5;
    public int backChargeDelta = 2;
    public int minStackToBack = 10;
    public int indicatorDistance = 20;
    public Pose[] prevPath;
    SpriteRenderer sp;
    int top = 0;
    bool playedBustSound = false;
    private int backCharge = 0;
    private int curStackSize = 0;
    private bool backCharged = false;
    Rigidbody2D rb2d;
    
    //Vector2 camSize;
    // Start is called before the first frame update

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        //camSize = camera.v;
        prevPath = new Pose[pathSizeFrames];
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!backCharged) //Not moving backward
        {
            if (Input.GetKey(KeyCode.E) && curStackSize > minStackToBack) //charge backwards
            {
                playedBustSound = false;
                if (!AudioManager.IsPlaying("TelebackCharge"))
                {
                    AudioManager.PlaySound("TelebackCharge");
                }
                backCharged = Input.GetKeyUp(KeyCode.E) || backCharge >= curStackSize; 
                for (int i = 0; i < backChargeDelta && !backCharged; i++)
                {
                    rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                    backCharge++;
                    try
                    {
                        prevPath[((top - backCharge) + pathSizeFrames) % pathSizeFrames].indicator.SetActive(true);
                    } catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                        print(prevPath[((top - backCharge) + pathSizeFrames) % pathSizeFrames]);
                        
                    }
                    backCharged = Input.GetKeyUp(KeyCode.E) || backCharge >= curStackSize;

                }

            }
            else
            {
                for (int i = indicatorDistance; i <= curStackSize; i += indicatorDistance)
                {
                    int idx = ((top - i ) + pathSizeFrames) % pathSizeFrames;
                    prevPath[idx].indicator.SetActive(false);
                }
                if (prevPath[top] != null)//Destroy top of stack
                {
                    Destroy(prevPath[top].indicator);
                    curStackSize--;
                }
                prevPath[top] = new Pose(transform.position);
                curStackSize += 1;
                sp.color = new Color(1, 1, 1, 1);
                prevPath[top].indicator = Instantiate(pathIndicator, transform.position, Quaternion.identity);//add to top of stack
                SpriteRenderer sp2 = prevPath[top].indicator.GetComponent<SpriteRenderer>();
                sp2.sprite = sp.sprite;
                sp2.flipX = sp.flipX;
                sp2.color = new Color(1,1,1,.3F);
                
                top = (top + 1) % pathSizeFrames;//increment top
                for (int i = indicatorDistance; i <= curStackSize; i += indicatorDistance)
                {
                    int idx = ((top - i) + pathSizeFrames) % pathSizeFrames;
                    prevPath[idx].indicator.SetActive(true);
                }

            }
            backCharged = Input.GetKeyUp(KeyCode.E) || backCharge >= curStackSize;
        }
        else 
        {
            if (!playedBustSound)
            {
                playedBustSound = true;
                AudioManager.PlaySound("TelebackBust");
                AudioManager.StopSound("TelebackCharge");
            }
            //move back travelBackSpeedFrames per frame
            for (int i = 0; i < travelBackSpeedFrames && backCharge > 0; i++)
            {
                top = ((top - 1) + pathSizeFrames) % pathSizeFrames;
                transform.position = prevPath[top].position;
                SpriteRenderer sp2 = prevPath[top].indicator.GetComponent<SpriteRenderer>();
                sp.sprite = sp2.sprite;
                sp.flipX = sp2.flipX;
                sp.color = sp2.color;
                Destroy(prevPath[top].indicator);
                prevPath[top] = null;
                backCharge--;
                
                curStackSize--; 
            }

            if (backCharge == 0)
            {
                backCharged = false;
                rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }
}
public class Pose
{
    public GameObject indicator;
    public Vector3 position;
    public Pose(Vector3 position)
    {
        this.position = position;
    }
    
}
