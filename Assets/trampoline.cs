using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trampoline : MonoBehaviour
{
    // Start is called before the first frame update
    public float forceMagnnitude = 30f;
    public ParticleSystem ps;
    Transform trans;
    void Start()
    {
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        float rot = trans.rotation.eulerAngles.z + 90;
        Vector3 forceDirection = new Vector3(Mathf.Cos(rot * Mathf.Deg2Rad), Mathf.Sin(rot * Mathf.Deg2Rad));
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Movement>().cancelDashEarly();
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector3 force = -Vector3.Project(rb.velocity, forceDirection);
            Vector2 force2D = force;
            force2D += (Vector2)forceDirection * forceMagnnitude; /// total force = playerVelocityInDirectionOfTrampoline + forceMagnitude
            //print(force2D);
            // the following is a way to remove the horizontal speed cap on the player for a number of frames corresponding to how horizontal the trampoline is
            collision.gameObject.GetComponent<Movement>().removeHorizonatalSpeedCapForFrames((int)(force2D.x / 4));
            rb.AddForce(force2D, ForceMode2D.Impulse);
        }else if(collision.gameObject.tag == "Enemy")
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector3 force = -Vector3.Project(rb.velocity, forceDirection);
            Vector2 force2D = new Vector2(force.x, force.y)/2F;
            force2D += force2D.normalized * forceMagnnitude/2F; /// total force = enemyVelocityInDirectionOfTrampoline + forceMagnitude/2

            rb.AddForce(force2D, ForceMode2D.Impulse);
        }
        ps.Play();
    }
}
