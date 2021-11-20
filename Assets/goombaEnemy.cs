using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goombaEnemy : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update
    private float currentHealth;
    public float maxHealth = 50;
    public int attackDamage = 25;
    public Collider2D hitbox;
    public float speed = 1.0f;
    private float sightDistanceToWall = 1.0f;
    private Rigidbody2D rb;
    private bool colliding = false;
    public bool startFacingRight = true;
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        sightDistanceToWall = transform.localScale.x / 1.9f;
        if (!startFacingRight)
        {
            speed = -1f * speed;
            sightDistanceToWall = -1f * sightDistanceToWall;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);    //set velocity
        colliding = Physics2D.Linecast(transform.position, new Vector2(transform.position.x + sightDistanceToWall, transform.position.y), LayerMask.GetMask("Wall"));
        if (Physics2D.Linecast(transform.position, new Vector2(transform.position.x + sightDistanceToWall, transform.position.y), LayerMask.GetMask("Default"))) {
            print("hello");
        } //check if hit a wall
        if (colliding)  // turn around if you hit a wall or player
        {
            speed = -1f * speed;
            sightDistanceToWall = -1f * sightDistanceToWall;
        }

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //TODO: play hurt animation

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        //TODO: play death animation

        //Disable the enemy bc it's dead
        // (we can change this to add corpse mechanics, work with rewind mechanic, etc)
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        playerCombat pc = col.gameObject.GetComponent<playerCombat>();
        if(pc != null)
        {
            pc.getHit(attackDamage, transform.position);
        }
    }
}

