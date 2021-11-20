using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerCombat : MonoBehaviour
{
    public int maxHealth = 100;
    public int health;
    public float attackRange = 0.5f;
    public int attackDamage = 50;
    public float attackRate = 2f;
    public float attackRadius = 2f;
    public GameObject attackObject;
    float nextAttackTime = 0f;
    private Movement playerMovement;
    private float hitForce = 20f;     //force that's applied to the player when hit by an attack (knockback)
    public int iFrames = 20;  //number of invincibility frames after getting hit by an attack
    private int currentIFrames = 0;

    void Awake()
    {
        health = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<Movement>();
        health = maxHealth;
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

    private void FixedUpdate()
    {
        if(currentIFrames > 0)
        {
            currentIFrames--;
        }
    }

    void Attack()
    {
        // TODO: play attack animation

        // temp attack animation: just show a slash lol
        // this can be deleted later if we want to, just a temp thing to show a slash while attacking
        if (playerMovement.directionFacing != -90)
        {
            float rotation = playerMovement.directionFacing * Mathf.Deg2Rad;
            Vector3 instantiatePosition = attackRadius * new Vector3(Mathf.Cos(rotation), Mathf.Sin(rotation), 0) + Vector3.back;
            Quaternion instantiateRotation = Quaternion.Euler(0, 0, playerMovement.directionFacing);
            GameObject slash = Instantiate(attackObject, transform.position + instantiatePosition, instantiateRotation, transform);
        }
        //TODO if the attack animation is directional, account for whether the player is flipped.   
        // slash.GetComponent<SpriteRenderer>().flipX = playerMovement.lookingRight;


    }

    //function controling player behavior when it gets hit by an attack- damage is the amount to subtract from health, source is where the player should get knocked back from (set it to null for no knockback)
    public void getHit(int damage, Vector3 source)
    {
        if(currentIFrames < 1)
        {
            health -= damage;
            currentIFrames = iFrames;
            if(health <= 0f)
            {
                Die();
            }
        }
        if(source == null)
        {
            source = transform.position;
        }
        Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 sourcePos = new Vector2(source.x, source.y);
        Vector3 dir = (sourcePos - myPos).normalized;
        dir = dir * -1 * hitForce;
        gameObject.GetComponent<Rigidbody2D>().AddForce(dir, ForceMode2D.Impulse);
    }

    public void Die()
    {
        //TODO: Death animation

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restart the scene
    }

}

