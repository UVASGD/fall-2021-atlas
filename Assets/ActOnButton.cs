using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActOnButton : MonoBehaviour
{
    public Button button;
    public ButtonActionType action;
    private SpriteRenderer sr;
    private Collider2D cl;
    public bool upWhenOn = true;
    public float framesToMove = 50;
    public float maxY, minY;
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
            case ButtonActionType.MOVE:
                Transform tr = GetComponent<Transform>();
                bool moveUp = button.activated;
                if (!upWhenOn)
                {
                    moveUp = !moveUp;
                }
                if (moveUp && tr.position.y < maxY)
                {
                    tr.position -= Vector3.up * (tr.position.y - maxY) / framesToMove;
                    if (tr.position.y > maxY)
                    {
                        tr.position = new Vector3(tr.position.x, maxY, tr.position.z);
                    }
                }
                else if (!moveUp && tr.position.y > minY)
                {
                    tr.position -= Vector3.up * (tr.position.y - minY) / framesToMove;
                    if (tr.position.y < minY)
                    {
                        tr.position = new Vector3(tr.position.x, minY, tr.position.z);
                    }
                }
                break;


        }
    }
}

public enum ButtonActionType
{
    DISAPPEAR, MOVE
}