using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slashGraphic : MonoBehaviour
{
    public int slashFrames = 30;
    int currentFrames = 0;

    void Start()
    {

    }

    void FixedUpdate()
    {
        if(currentFrames > slashFrames)
        {
            Destroy(this.gameObject);
        }
        else
        {
            currentFrames++;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            try
            {
                collision.gameObject.SendMessage("TakeDamage", 15);
            }
            catch
            {

            }
        }
        //collision.gameObject.SendMessage("TakeDamage", 30);
    }
}
