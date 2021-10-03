using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkDiffuse : MonoBehaviour
{
    public GameObject spark;
    
    private float direction = 0;
    private float intensity = 150;
    private float intenseReduc;
    private bool copy = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(intensity <= 0)
        {
            Destroy(this.gameObject);
        }
        Color temp = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, intensity / 150);

        if (!copy)
        {
            float curIntensity = intensity;
            int at = 1;
            int total = Random.Range(4, 12);
            while (curIntensity > 0 && at < total)
            {
                curIntensity -= 5;
                GameObject nextSpark = (GameObject)Instantiate(spark, new Vector3(transform.position.x + Mathf.Cos(direction) * 0.1F * at, transform.position.y + Mathf.Sin(direction) * 0.1F * at, -1), transform.rotation);
                nextSpark.SendMessage("setupDir", direction);
                nextSpark.SendMessage("setupIntense", curIntensity);
                nextSpark.SendMessage("setupSpark", spark);
                at++;
            }
            copy = true;
            curIntensity -= 5;
            at--;
            if (curIntensity > 0)
            {
                int amt = Random.Range(0, 10);
                float x = transform.position.x + Mathf.Cos(direction) * 0.1F * at;
                float y = transform.position.y + Mathf.Sin(direction) * 0.1F * at;
                if (amt < 8)
                {
                    float deviation = Random.Range(5, 35) / 180F * Mathf.PI;
                    GameObject nextSparkA = (GameObject)Instantiate(spark, new Vector3(x + Mathf.Cos(direction + deviation) * 0.1F, y + Mathf.Sin(direction + deviation) * 0.1F, -1), transform.rotation);
                    nextSparkA.SendMessage("setupDir", direction + deviation);
                    nextSparkA.SendMessage("setupIntense", curIntensity);
                    nextSparkA.SendMessage("setupSpark", spark);
                    nextSparkA.SendMessage("setJoint");
                    deviation = Random.Range(5, 35) / 180F * Mathf.PI;
                    GameObject nextSparkB = (GameObject)Instantiate(spark, new Vector3(x + Mathf.Cos(direction - deviation) * 0.1F, y + Mathf.Sin(direction - deviation) * 0.1F, -1), transform.rotation);
                    nextSparkB.SendMessage("setupDir", direction - deviation);
                    nextSparkB.SendMessage("setupIntense", curIntensity);
                    nextSparkB.SendMessage("setupSpark", spark);
                    nextSparkB.SendMessage("setJoint");
                }
                else if(intensity > 50)
                {
                    GameObject nextSpark = (GameObject)Instantiate(spark, new Vector3(x + Mathf.Cos(direction) * 0.1F, y + Mathf.Sin(direction) * 0.1F, -1), transform.rotation);
                    nextSpark.SendMessage("setupDir", direction);
                    nextSpark.SendMessage("setupIntense", curIntensity);
                    nextSpark.SendMessage("setupSpark", spark);
                    nextSpark.SendMessage("setJoint");
                    float deviation = Random.Range(10, 45) / 180F * Mathf.PI;
                    GameObject nextSparkA = (GameObject)Instantiate(spark, new Vector3(x + Mathf.Cos(direction + deviation) * 0.1F, y + Mathf.Sin(direction + deviation) * 0.1F, -1), transform.rotation);
                    nextSparkA.SendMessage("setupDir", direction + deviation);
                    nextSparkA.SendMessage("setupIntense", curIntensity);
                    nextSparkA.SendMessage("setupSpark", spark);
                    nextSparkA.SendMessage("setJoint");
                    deviation = Random.Range(10, 45) / 180F * Mathf.PI;
                    GameObject nextSparkB = (GameObject)Instantiate(spark, new Vector3(x + Mathf.Cos(direction - deviation) * 0.1F, y + Mathf.Sin(direction - deviation) * 0.1F, -1), transform.rotation);
                    nextSparkB.SendMessage("setupDir", direction - deviation);
                    nextSparkB.SendMessage("setupIntense", curIntensity);
                    nextSparkB.SendMessage("setupSpark", spark);
                    nextSparkB.SendMessage("setJoint");
                }
            }
        }
        intensity -= intenseReduc;
    }

    // Sets up the spark's direction
    void setupDir(float dir)
    {
        direction = dir;
    }

    // Sets up the spark's intensity
    void setupIntense(float intense)
    {
        intensity = intense;
        intenseReduc = intensity / 20;
    }

    // Sets up the spark's spark Prefab
    void setupSpark(GameObject sparky)
    {
        spark = sparky;
    }

    // Makes it so the game understand that the spark is a joint or not.
    void setJoint()
    {
        copy = false;
    }
}
