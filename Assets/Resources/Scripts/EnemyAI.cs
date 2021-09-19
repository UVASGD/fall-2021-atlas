using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public bool facingRight;
    public float sightRange;
    public float sightWidth;

    private Rigidbody2D rb;
    private BoxCollider2D cl;

    // To Delete
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cl = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (detection())
        {
            timer = 3;
        }

        // To Delete
        if (timer != 0)
            timer -= Time.deltaTime;
        if(timer < 0)
        {
            timer = 0;
            facingRight = !facingRight;
        }
        // End Delete
    }

    // Cone based vision detection based off of the direction the entity faces.
    private bool detection()
    {
        int amt = 10;

        // Sets up cone
        bool found = false;
        for(int i = 0; i < amt; i++)
        {
            float angle;
            if (facingRight) 
            {
                // Right angle calculation and raycast check
                angle = sightWidth - i * (sightWidth * 2 / amt);
                angle = angle * Mathf.PI / 180;
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(cl.bounds.max.x + 0.05F, cl.bounds.center.y + 0.4F), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), sightRange);;

                // Checks if player detected
                if (hit.collider != null && hit.collider.gameObject.tag == "Player")
                {
                    Debug.DrawRay(new Vector2(cl.bounds.max.x + 0.05F, cl.bounds.center.y + 0.4F), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * sightRange, Color.green);
                    found = true;
                }
                else
                    Debug.DrawRay(new Vector2(cl.bounds.max.x + 0.05F, cl.bounds.center.y + 0.4F), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * sightRange, Color.red);
            }
            else 
            {
                // Left angle calculation
                angle = 180 - sightWidth + i * (sightWidth * 2 / amt);
                angle = angle * Mathf.PI / 180;
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(cl.bounds.min.x - 0.05F, cl.bounds.center.y + 0.4F), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), sightRange);

                // Checks if player detected
                if (hit.collider != null && hit.collider.gameObject.tag == "Player")
                {
                    Debug.DrawRay(new Vector2(cl.bounds.min.x - 0.05F, cl.bounds.center.y + 0.4F), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * sightRange, Color.green);
                    found = true;
                }
                else
                    Debug.DrawRay(new Vector2(cl.bounds.min.x - 0.05F, cl.bounds.center.y + 0.4F), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * sightRange, Color.red);
            }
        }
        
        return found;
    }
}
