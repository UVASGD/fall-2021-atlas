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
    Rigidbody2D rb;
    Animator anim;
    bool rewinding;
    int top = 0;
    public int backCharge = 0;
    public int curStackSize = 0;
    public bool backCharged = false;
    Rigidbody2D rb2d;
    
    //Vector2 camSize;
    // Start is called before the first frame update

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        //camSize = camera.v;
        prevPath = new Pose[pathSizeFrames];
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!backCharged) //Not moving backward
        {
            if (Input.GetKey(KeyCode.E) && curStackSize > minStackToBack) //charge backwards
            {
                anim.SetBool("Teleback", true);
                backCharged = Input.GetKeyUp(KeyCode.E) || backCharge >= curStackSize; 
                for (int i = 0; i < backChargeDelta && !backCharged; i++)
                {
                    rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                    backCharge++;
                    try
                    {
                        prevPath[((top - backCharge) + pathSizeFrames) % pathSizeFrames].indicator.SetActive(true);
                        if (rewinding)
                            prevPath[((top - backCharge + 1) + pathSizeFrames) % pathSizeFrames].indicator.SetActive(false);
                        rewinding = true;
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
                prevPath[top] = new Pose(transform.position, rb.velocity);
                curStackSize += 1;
                sp.color = new Color(1, 1, 1, 1);
                prevPath[top].indicator = Instantiate(pathIndicator, transform.position, Quaternion.identity);//add to top of stack
                SpriteRenderer sp2 = prevPath[top].indicator.GetComponent<SpriteRenderer>();
                sp2.sprite = sp.sprite;
                sp2.flipX = sp.flipX;
                sp2.color = new Color(1,1,1,.2F);
                
                top = (top + 1) % pathSizeFrames;//increment top
                for (int i = indicatorDistance; i <= curStackSize; i += indicatorDistance)
                {
                    int idx = ((top - i) + pathSizeFrames) % pathSizeFrames;
                    if (Vector3.Distance(prevPath[idx].position, transform.position) > 1)
                        prevPath[idx].indicator.SetActive(true);
                }

            }
            backCharged = Input.GetKeyUp(KeyCode.E) || backCharge >= curStackSize;
        }
        else 
        {
            rewinding = false;
            anim.SetBool("Teleback", false);

            //move back travelBackSpeedFrames per frame
            for (int i = 0; i < travelBackSpeedFrames && backCharge > 0; i++)
            {
                top = ((top - 1) + pathSizeFrames) % pathSizeFrames;
                transform.position = prevPath[top].position;
                rb.velocity = prevPath[top].velocity;
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
    public Vector3 velocity;
    public Pose(Vector3 position, Vector3 velocity)
    {
        this.position = position;
        this.velocity = velocity;
    }
    
}
