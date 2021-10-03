using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slashGraphic : MonoBehaviour
{
    SpriteRenderer slashSprite;
    public int slashFrames = 5;
    int currentFrames = 0;
    public playerCombat player;
    
    public void Attack(bool facingRight)
    {
        if (facingRight)
        {
            slashSprite.flipX = false;
            transform.localPosition = new Vector2(0, 0);
        }
        else
        {
            slashSprite.flipX = true;
            transform.localPosition = new Vector2(- 2 * player.attackPoint.localPosition.x, 0);
        }
        slashSprite.enabled = true;
    }

    void Start()
    {
        slashSprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if(slashSprite.enabled)
        {
            if(currentFrames > slashFrames)
            {
                slashSprite.enabled = false;
                currentFrames = 0;
            }
            else
            {
                currentFrames++;
            }
        }
    }
}
