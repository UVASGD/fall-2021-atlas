using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActOnButton : MonoBehaviour
{
    public Button button;
    public ButtonActionType action;
    private SpriteRenderer sr;
    private Collider2D cl;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cl = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (action)
        {
            case ButtonActionType.DISAPPEAR:
                sr.enabled = cl.enabled = !button.activated;
                break;
                    
        }
    }
}

public enum ButtonActionType
{
    DISAPPEAR, MOVE
}