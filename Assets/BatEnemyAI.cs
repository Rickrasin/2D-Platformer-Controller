using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BatEnemyAI : MonoBehaviour
{

    public Transform target;
    public Transform targetStart;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;


    public float shootingRange;
    public float lineOfSite;
    public bool idleFly;

   
    AIPath aiPath;
    

    AIDestinationSetter setTarget;

    Transform player;
    public LayerMask playerMask;
    Animator anim;
    Seeker seeker;
    Rigidbody2D rb;

    private void Awake()
    {
        
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        setTarget = GetComponent<AIDestinationSetter>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        aiPath = GetComponent<AIPath>();


        //aiPath.enabled = false;
       
    }

    private void Update()
    {
        Flip();



        /*
        if (idleFly)
        {
            anim.Play("FlyingIdle");
        }
        else
        {
            anim.Play("Flying");
            return;
        }
        */
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        bool agro = Physics2D.OverlapCircle(transform.position, lineOfSite, playerMask);

       
        if (distanceFromPlayer < lineOfSite && distanceFromPlayer > shootingRange)
        {
            aiPath.enabled = true;
            setTarget.target = target;
        }
            else if (distanceFromPlayer <= shootingRange)
        {
            aiPath.enabled = false;
        } else
        {
            setTarget.target = targetStart;
            //aiPath.enabled = false;
        }


        
}





    void Flip()
    {
        if (transform.position.x < player.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (transform.position.x > player.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);

        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, shootingRange);
        Gizmos.DrawWireSphere(transform.position, lineOfSite);

    }
}
