using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sparkTest : MonoBehaviour
{
    public GameObject spark;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float nextDirection = Random.Range(0, 360) / 180F * Mathf.PI;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject nextSpark = (GameObject)Instantiate(spark, new Vector3(worldPosition.x, worldPosition.y, -1), transform.rotation);
            nextSpark.SendMessage("setupDir", nextDirection);
            nextSpark.SendMessage("setupSpark", spark);
            nextSpark.SendMessage("setupIntense", 150);
            nextSpark.SendMessage("setJoint");
        }
    }
}
