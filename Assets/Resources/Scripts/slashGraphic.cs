using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slashGraphic : MonoBehaviour
{
    SpriteRenderer slashSprite;
    PolygonCollider2D collider;
    public int slashFrames = 5;
    int currentFrames = 0;
    public playerCombat player;

    public void Attack(float direction, bool facingRight)
    {
        slashSprite.flipX = !facingRight;
        
    }

    void Start()
    {
        collider = GetComponent<PolygonCollider2D>();
        slashSprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if(slashSprite.enabled)
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
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SendMessage("TakeDamage", 30);
    }
}
