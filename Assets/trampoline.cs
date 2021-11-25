using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trampoline : MonoBehaviour
{
    // Start is called before the first frame update
    public float force = 30f;
    public ParticleSystem ps;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(0, -1*rb.velocity.y + force), ForceMode2D.Impulse);
        }else if(collision.gameObject.tag == "Enemy")
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(0, -1 * rb.velocity.y + force/2f), ForceMode2D.Impulse);
        }
        ps.Play();
    }
}
