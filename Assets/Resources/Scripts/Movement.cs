using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    /*
     * Little note up here for anyone looking at this right now.
     * 
     * Most of this code is stuff I've already tested (as of 8/30/21).
     * It a lot, but it works.
     * Therefore, unless we make design decisions that nullify the decisions I've made, I recommend not bothering with this file...
     */



    public KeyCode UP_BUTTON_CODE = KeyCode.W, DOWN_BUTTON_CODE = KeyCode.S, LEFT_BUTTON_CODE = KeyCode.A, RIGHT_BUTTON_CODE = KeyCode.D, JUMP_BUTTON_CODE = KeyCode.Space;

    public int maxWalkSpeed;
    public float dashSpeed;
    public int maxFallSpeed;
    public float jumpBurst;
    public float maxJump;
    public float directionFacing; //degree measure for the direction you're facing. Up = 90 degrees, positive = counterclockwise
    public bool lookingRight = true;
    public static bool canMove;
    public static bool grounded;
   
    private Vector3 respawnPos;
    private const int walkAccel = 100;
    public int dashLength = 10;
    private int dashTimer;
    private bool dashing;
    private Rigidbody2D rb;
    private Collider2D cl;
    private Animator anim;
    private bool jumping = true;
    private float startJump;
    private float xRangeMin;
    private float xRangeMax;
    private float minHeight;
    [SerializeField] private LayerMask walls;
    SpriteRenderer spriteRenderer;

    //this is the number of frames until the horizontal speed cap is enforced (when it's 0, speed is capped)
    private int framesUntilHorizontalCap = 0;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        respawnPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        rb = GetComponent<Rigidbody2D>();
        cl = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;
        Camera.main.GetComponent<AppManager>().follow = transform;
        Camera.main.SendMessage("setBounds");
        framesUntilHorizontalCap = 0;
    }
   
    // Update is called once per frame
    void Update()
    {
        bool upButton = Input.GetKey(UP_BUTTON_CODE), 
            downButton = Input.GetKey(DOWN_BUTTON_CODE), 
            leftButton = Input.GetKey(LEFT_BUTTON_CODE), 
            rightButton = Input.GetKey(RIGHT_BUTTON_CODE),
            jumpButton = Input.GetKey(JUMP_BUTTON_CODE);

        bool leftButtonPressed = Input.GetKeyDown(LEFT_BUTTON_CODE),
            rightButtonPressed = Input.GetKeyDown(RIGHT_BUTTON_CODE);

        grounded = onGround();

        if (rb.velocity.y > 0)
        {
            anim.SetBool("Rise", true);
            anim.SetBool("Fall", false);
        }
        if (rb.velocity.y < 0)
        {
            anim.SetBool("Rise", false);
            anim.SetBool("Fall", true);
        }
        if (grounded)
        {
            anim.SetBool("Fall", false);
            anim.SetBool("Rise", false);
        }
        if (!DialogueScript.canTalk)
            anim.SetBool("Running", false);

        if (rightButtonPressed)
        {
            lookingRight = true;
        }
        else if (leftButtonPressed)
        {
            lookingRight = false;
        }
        if (DialogueScript.canTalk && canMove)
        {
            spriteRenderer.flipX = !lookingRight;

            //TODO allow player to change which direction they are facing even when not able to move. Consider changing.
            if (upButton)
            {
                directionFacing = 90; // if up button is pressed, always face up.
            }
            else if (downButton)
            {
                directionFacing = -90;
            }
            else //decide whether we're facing right or left
            {
                directionFacing = lookingRight ? 0 : 180; //face in the direction of the last left/right buttom press. 
            }

            // Jump Code
            if (jumpButton && !jumping && grounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpBurst);
                startJump = Time.time;
                jumping = true;
            }
            else if (jumping && (Input.GetKeyUp(JUMP_BUTTON_CODE) || (startJump + maxJump < Time.time)))
            {
                jumping = false;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 1.75F);
            }


            if (leftButton && rightButton)
            {
                if (lookingRight)// move in the direction that was pressed first, not the one pressed more recently
                {
                    if (grounded)
                        rb.AddForce(Vector2.left * walkAccel);
                    else if (!onWallL())
                        rb.AddForce(Vector2.left * walkAccel / 1.25F);
                } else
                {
                    if (grounded)
                        rb.AddForce(Vector2.right * walkAccel);
                    else if (!onWallR())
                        rb.AddForce(Vector2.right * walkAccel / 1.25F);
                }
            }
            else if (leftButton)
            {
                if (grounded)
                {
                    rb.AddForce(Vector2.left * walkAccel);
                    anim.SetBool("Running", true);
                }
                else if (!onWallL())
                    rb.AddForce(Vector2.left * walkAccel / 1.25F);
            }
            else if (rightButton)
            {
                if (grounded)
                {
                    rb.AddForce(Vector2.right * walkAccel);
                    anim.SetBool("Running", true);
                }
                else if (!onWallR())
                    rb.AddForce(Vector2.right * walkAccel / 1.25F);
            }
            else
            {
                anim.SetBool("Running", false);
            }
        }

        /**
         * Dash Start Code:
         * 
         * Sets velocity = 0 in both x and y directions
         * Sets gravity = 0 and drag = 0
         * Starts timer for dash and sets dashing to true
         * Adds constant value force to player in the direction they are facing
         */
        if (Input.GetKeyDown(KeyCode.LeftShift) && !dashing)
        {
            dashing = true;
            dashTimer = dashLength;
            rb.gravityScale = 0;
            rb.velocity = Mathf.Cos(Mathf.Deg2Rad*directionFacing) * new Vector2(dashSpeed, 0);
            anim.SetTrigger("Dashing");
        }

        /**
         * Dash End Check Code
         * 
         * Checks if dash is over
         * if so
         *    Resets gravity and drag
         *    Sets dashing to false
         * if not
         *    Decrease timer
         */
        if (dashing)
        {
            if (dashTimer == 0)
            {
                dashing = false;
                rb.velocity = Vector2.zero;

                rb.gravityScale = 5;
            }
            else
            {
                rb.velocity = Mathf.Cos(Mathf.Deg2Rad * directionFacing) * new Vector2(dashSpeed, 0);
                dashTimer--;
            }
        }

        // Max Velocity Check

        if(framesUntilHorizontalCap > 0)
        {
            framesUntilHorizontalCap--;
        }

        float xSpeed = rb.velocity.x;
        if (Mathf.Abs(xSpeed) > maxWalkSpeed && !dashing && (framesUntilHorizontalCap<1))
        {
            if (xSpeed > 0)
            {
                rb.velocity = new Vector2(maxWalkSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-maxWalkSpeed, rb.velocity.y);
            }
        }

        float ySpeed = rb.velocity.y;
        if (ySpeed < -maxFallSpeed && ySpeed < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
        }


        // Bounds checking
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xRangeMin, xRangeMax), transform.position.y, transform.position.z);
        if (transform.position.y < minHeight)
            Die();
   
    }

    // Bounds setting
    void setMinLimit(float min)
    {
        xRangeMin = min;
    }

    void setMaxLimit(float max)
    {
        xRangeMax = max;
    }

    void setMinHeight(float min)
    {
        minHeight = min;
    }

    // Set Respawn Point
    void setRespawn(Vector3 respawn)
    {
        respawnPos = respawn;
    }

    // Death Code
    void Die()
    {
        
    }

    // Spatial Awareness code (ground, wall, etc.)
    bool onGround()
    {
        RaycastHit2D checker = Physics2D.BoxCast(cl.bounds.center, cl.bounds.size, 0F, Vector2.down, 0.1F, walls);
        Color draw;
        if (checker.collider != null)
            draw = Color.green;
        else
            draw = Color.red;
        Debug.DrawRay(cl.bounds.center, Vector2.down * (cl.bounds.extents.y + 0.1F), draw);
        return checker.collider != null;
    }

    bool onWallR()
    {
        RaycastHit2D checker = Physics2D.BoxCast(cl.bounds.center, cl.bounds.size, 0F, Vector2.right, 0.3F, walls);
        Color draw;
        if (checker.collider != null)
            draw = Color.green;
        else
            draw = Color.red;
        Debug.DrawRay(cl.bounds.center, Vector2.right * (cl.bounds.extents.x + 0.3F), draw);
        return checker.collider != null;
    }

    bool onWallL()
    {
        RaycastHit2D checker = Physics2D.BoxCast(cl.bounds.center, cl.bounds.size, 0F, Vector2.left, 0.3F, walls);
        Color draw;
        if (checker.collider != null)
            draw = Color.green;
        else
            draw = Color.red;
        Debug.DrawRay(cl.bounds.center, Vector2.left * (cl.bounds.extents.x + 0.3F), draw);
        return checker.collider != null;
    }
    
    // Remove the cap on horizontal speed for a supplied number of frames
    public void removeHorizonatalSpeedCapForFrames(int frames)
    {
        if (frames > framesUntilHorizontalCap)
        {
            framesUntilHorizontalCap = frames;
        }
    }

    public void cancelDashEarly()
    {
        if (dashing)
        {
            dashing = false;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 5;
            dashTimer = 0;
        }
    }
}
