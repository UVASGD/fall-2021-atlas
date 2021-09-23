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
    
    
    public int maxWalkSpeed;
    public int maxFallSpeed;
    public float jumpBurst;
    public float maxJump;
    public bool facingRight;
    public static bool canMove;

    private Vector3 respawnPos;
    private const int walkAccel = 100;
    private const int dashLength = 10;
    private int dashTimer;
    private bool dashing;
    private Rigidbody2D rb;
    private BoxCollider2D cl;
    //private Animator anim;
    private bool jumping = true;
    private float startJump;
    private float xRangeMin;
    private float xRangeMax;
    private float minHeight;
    [SerializeField] private LayerMask walls;

    // Start is called before the first frame update
    void Start()
    {
        respawnPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        rb = GetComponent<Rigidbody2D>();
        cl = GetComponent<BoxCollider2D>();
        //anim = GetComponent<Animator>();
        rb.freezeRotation = true;
        this.gameObject.layer = 7;
        Camera.main.GetComponent<AppManager>().follow = transform;
        Camera.main.SendMessage("setBounds");
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            // Jump Code
            if (Input.GetKeyDown(KeyCode.W) && !jumping && onGround())
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpBurst);
                startJump = Time.time;
                jumping = true;
            }
            else if (jumping && (Input.GetKeyUp(KeyCode.W) || (startJump + maxJump < Time.time)))
            {
                jumping = false;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 1.75F);
            }

            // Walk Code
            if (Input.GetKey(KeyCode.A))
            {
                if (onGround())
                    rb.AddForce(Vector2.left * walkAccel);
                else if (!onWallL())
                    rb.AddForce(Vector2.left * walkAccel / 1.25F);
                facingRight = false;
                //anim.SetBool("walking", true);
                //transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (onGround())
                    rb.AddForce(Vector2.right * walkAccel);
                else if (!onWallR())
                    rb.AddForce(Vector2.right * walkAccel / 1.25F);
                facingRight = true;
                //anim.SetBool("walking", true);
                //transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                //anim.SetBool("walking", false);
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
            rb.velocity = new Vector2(0, 0);
            if (facingRight)
            {
                rb.velocity = new Vector2(maxWalkSpeed * 2F, 0);
            }
            else
            {
                rb.velocity = new Vector2(-maxWalkSpeed * 2F, 0);
            }
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
                rb.gravityScale = 5;
            }
            else
            {
                dashTimer--;
            }
        }

        // Max Velocity Check
        float xSpeed = rb.velocity.x;
        if (Mathf.Abs(xSpeed) > maxWalkSpeed && !dashing)
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
        RaycastHit2D checker = Physics2D.BoxCast(cl.bounds.center, cl.bounds.size, 0F, Vector2.down, 0.3F, walls);
        Color draw;
        if (checker.collider != null)
            draw = Color.green;
        else
            draw = Color.red;
        Debug.DrawRay(cl.bounds.center, Vector2.down * (cl.bounds.extents.y + 0.3F), draw);
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
}
