using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCombat : MonoBehaviour
{
    public float attackRange = 0.5f;
    public int attackDamage = 50;
    public float attackRate = 2f;
    public float attackRadius = 2f;
    public GameObject attackObject;
    float nextAttackTime = 0f;
    private Movement playerMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        // TODO: play attack animation

        // temp attack animation: just show a slash lol
        // this can be deleted later if we want to, just a temp thing to show a slash while attacking
        float rotation = playerMovement.directionFacing* Mathf.Deg2Rad;
        Vector3 instantiatePosition = attackRadius * new Vector3(Mathf.Cos(rotation), Mathf.Sin(rotation), 0) + Vector3.back;
        Quaternion instantiateRotation = Quaternion.Euler(0, 0, playerMovement.directionFacing);
        GameObject slash = Instantiate(attackObject, transform.position + instantiatePosition, instantiateRotation, transform);
        //TODO if the attack animation is directional, account for whether the player is flipped.   
        // slash.GetComponent<SpriteRenderer>().flipX = playerMovement.lookingRight;






    }

  
}

