using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageRandomizer : MonoBehaviour
{
    public int range;
    public string imgName;
    
    // Start is called before the first frame update
    void Start()
    {
        int select = Random.Range(0, range + 1);
        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        sp.sprite = Resources.Load<Sprite>("Sprites/" + imgName + select);
    }
}
