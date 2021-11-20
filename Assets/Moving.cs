using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            myRigidbody.AddForce(new Vector2(-10, 0));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            myRigidbody.AddForce(new Vector2(10, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            myRigidbody.AddForce(new Vector2(0,10));
        }
    }
}
