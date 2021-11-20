using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject door;

    
    bool isOpened = false;

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (isOpened == false)
            {
                //Debug.Log("Inside COllider");
                isOpened = true;
                door.transform.position += new Vector3(0, 5, 0);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (isOpened == true)
            {
                isOpened = false;
                door.transform.position += new Vector3(0, -5, 0);
            }
        }
    }

}
