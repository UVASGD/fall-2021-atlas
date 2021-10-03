using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 50;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public slashGraphic slash;     //note: may delete this depending on how the attack animation works
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
        slash.Attack(playerMovement.facingRight);

        Collider2D[] hitEnemies;

        // Detect enemies in range of attack
        if (playerMovement.facingRight)
        {
            hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        }
        else
        {
            hitEnemies = Physics2D.OverlapCircleAll(new Vector2(attackPoint.position.x - 2 * transform.localScale.x * attackPoint.localPosition.x, attackPoint.position.y), attackRange, enemyLayers);
        }

        // Damage them
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<IDamageable>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(new Vector2(attackPoint.position.x -2*transform.localScale.x * attackPoint.localPosition.x, attackPoint.position.y), attackRange);
    }
}

