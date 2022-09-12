using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float speed;
    public float lineOfSite;
    public float shootingRange;
    public float fireRate = 1f;

    public float ceilingDistance;
    public float topDistance;

    private float nextFireTime;
    private bool isCeiling;
    private RaycastHit2D isTopHit;
    private Vector2 hitPos;
    private bool runTo;

    public GameObject bullet;
    public GameObject bulletParent;
    public Transform CeilingCheck;
    public Transform topPos;


    public LayerMask whatIsGround;

    private Transform playerGO;


    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player").transform;
        runTo = false;
    }

    // Update is called once per frame
    void Update()
    {

        isCeiling = Physics2D.Raycast(CeilingCheck.position, Vector2.up, ceilingDistance, whatIsGround);
        isTopHit = Physics2D.Raycast(topPos.position, Vector2.up, topDistance, whatIsGround);

        float distanceFromPlayer = Vector2.Distance(playerGO.position, transform.position);
        
        if(distanceFromPlayer< lineOfSite && distanceFromPlayer > shootingRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, playerGO.position, speed * Time.deltaTime);

        }
        else if(distanceFromPlayer <= shootingRange && nextFireTime < Time.time)
        {
            Instantiate(bullet, bulletParent.transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
        } else if(isCeiling)
        {
            print("in Idle");
            runTo = false;
        }
        if(isTopHit)
        {
            hitPos = isTopHit.point;

            runTo = true;

        }

        if(runTo)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, hitPos, speed * Time.deltaTime);

        }

    }




    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(CeilingCheck.position, Vector2.up);
        Gizmos.DrawRay(topPos.position, Vector2.up);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
