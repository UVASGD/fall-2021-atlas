using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject player;
    public GameObject bombPrefab;
    public float maxDistanceToPlayer = 10;
    int attackPos = 0;
    int attackIdx = 0;
    // Start is called before the first frame update
    void Start()
    {
       
            
    }

    // Update is called once per frame
    void Update()
    {
        Transform enemyTransform = this.gameObject.GetComponent<Transform>();
        Transform goalTransform = player.GetComponent<Transform>();
        // operator override means we can just subtract directly
        Vector3 vecToGoal = goalTransform.position - enemyTransform.position;
        float distance = vecToGoal.magnitude;
        if (distance > maxDistanceToPlayer)
        {
            vecToGoal.z = 0;
            vecToGoal.Normalize();
            vecToGoal *= DASH_LENGTH;
            dashDirectionUpdate = vecToGoal / DASH_MOVE_FRAMES;
            dashing = true;

            this.transform.position += dashDirectionUpdate/5;
    }
        switch (attackIdx)
        {
            case 0:
                bool attackDone = DashAttack();
                if (attackDone)
                {
                    attackIdx++;
                }
                break;
            case 1:
                attackDone = BombAttack();
                if (attackDone)
                {
                    attackIdx++;
                }
                break;
            case 2:
                attackDone = DashAttack();
                if (attackDone)
                {
                    attackIdx = 0;
                }
                break;
        }
}


    public int DASH_IDLE_FRAMES = 20, DASH_MOVE_FRAMES = 10, DASH_ENDLAG_FRAMES = 10;
    public int DASH_LENGTH = 1;

    Vector3 dashDirectionUpdate;
    bool dashing = false;
    bool DashAttack()
    {
        if (attackPos < DASH_IDLE_FRAMES)
        {
            if (dashing == false)
            {
                Transform enemyTransform = this.gameObject.GetComponent<Transform>();
                Transform goalTransform = player.GetComponent<Transform>();
                // operator override means we can just subtract directly
                Vector3 vecToGoal = goalTransform.position - enemyTransform.position;
                // don't care about Z difference
                vecToGoal.z = 0;
                vecToGoal.Normalize();
                vecToGoal *= DASH_LENGTH;
                dashDirectionUpdate = vecToGoal / DASH_MOVE_FRAMES;
                dashing = true;
            }

            //todo idle animation
        }
        else if (attackPos < DASH_IDLE_FRAMES + DASH_MOVE_FRAMES)
        {
            this.transform.position += dashDirectionUpdate;
        } else if (attackPos < DASH_IDLE_FRAMES + DASH_MOVE_FRAMES + DASH_ENDLAG_FRAMES)
        {
            dashing = false;
            //todo lagged animation
        }
        else
        {
            attackPos = 0;
            return true;
        }
        attackPos++;
        return false;
    }
    public int BOMB_IDLE_FRAMES = 20, BOMB_REPEATS = 4;
    bool BombAttack()
    {
        attackPos++;
        if (attackPos % BOMB_IDLE_FRAMES == 0)
        {
            throwBomb();
        }
        if (attackPos == BOMB_REPEATS * (BOMB_IDLE_FRAMES))
        {
            return true;
        }
        return false;

    }
    public float minBombVelocity = 0, maxBombVelocity = 10;
    void throwBomb()
    {
        Transform enemyTransform = this.gameObject.GetComponent<Transform>();
        Transform goalTransform = player.GetComponent<Transform>();
        // operator override means we can just subtract directly
        Vector3 vecToGoal = goalTransform.position - enemyTransform.position;
        float angleToPlayer = Mathf.Atan2(vecToGoal.y, vecToGoal.x);
        GameObject bomb = Instantiate(bombPrefab, this.transform.position, this.transform.rotation);
        angleToPlayer = ((angleToPlayer * Mathf.Rad2Deg) % 360 + 360) % 360; //normalize angle
        float offsetAngle = Random.Range(Mathf.PI / 8F, Mathf.PI / 4F) * ((angleToPlayer > 90 && angleToPlayer < 270) ? -1 : 1) ;
        float angle = offsetAngle + angleToPlayer * Mathf.Deg2Rad;

        float v = Random.Range(minBombVelocity, maxBombVelocity);
        float vy = v * Mathf.Sin(angle);
        float vx = v * Mathf.Cos(angle);

        bomb.GetComponent<Rigidbody2D>().velocity = new Vector3(vx, vy);

        
    }
}
