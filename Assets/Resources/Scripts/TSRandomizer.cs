using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSRandomizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        int val = Random.Range(0, 6);
        string img = "Sprites/TS" + val;
        Debug.Log(img);
        sp.sprite = Resources.Load<Sprite>(img);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
